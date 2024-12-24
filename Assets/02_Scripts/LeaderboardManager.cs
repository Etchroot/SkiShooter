using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using System.Collections.Generic;
using System.Threading.Tasks;

public class LeaderboardManager
 : MonoBehaviour
{
    [SerializeField] private Transform contentParent; // ScrollView의 Content
    [SerializeField] private GameObject itemPrefab;   // ScrollView 항목 Prefab
    [SerializeField] private Button refreshButton;    // 새로고침 버튼

    private async void Start()
    {
        // Unity Services 초기화
        await UnityServices.InitializeAsync();
        Debug.Log("Unity Services Initialized");

        // 익명 로그인
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await SignInAnonymously();
        }

        // 새로고침 버튼 클릭 이벤트 연결
        refreshButton.onClick.AddListener(() => FetchAndDisplayLeaderboard());

        // 첫 화면에서 리더보드 표시
        await FetchAndDisplayLeaderboard();
    }

    private async Task SignInAnonymously()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log($"익명 로그인 성공! Player ID: {AuthenticationService.Instance.PlayerId}");
        }
        catch (AuthenticationException e)
        {
            Debug.LogError($"익명 로그인 실패: {e.Message}");
        }
        catch (RequestFailedException e)
        {
            Debug.LogError($"요청 실패: {e.Message}");
        }
    }

    private async Task FetchAndDisplayLeaderboard()
    {
        // ScrollView 초기화 (기존 항목 제거)
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // Cloud Save에서 데이터 가져오기
        var data = await FetchSavedNicknames();
        if (data == null) return;

        // 데이터 리스트 생성
        foreach (var entry in data)
        {
            GameObject item = Instantiate(itemPrefab, contentParent);

            TMP_Text keyText = item.transform.Find("KeyText").GetComponent<TMP_Text>();
            TMP_Text valueText = item.transform.Find("ValueText").GetComponent<TMP_Text>();

            keyText.text = entry.Key;   // Key 값 (예: "code_name")
            valueText.text = entry.Value; // Value 값 (예: 닉네임)
        }
    }

    private async Task<Dictionary<string, string>> FetchSavedNicknames()
    {
        try
        {
            var cloudData = await CloudSaveService.Instance.Data.Player.LoadAllAsync();
            var result = new Dictionary<string, string>();

            foreach (var item in cloudData)
            {
                Debug.Log($"Key: {item.Key}, Value: {item.Value}");
                result[item.Key] = item.Value.ToString();
            }

            Debug.Log("Cloud Save 데이터 가져오기 성공");
            return result;
        }
        catch (CloudSaveException e)
        {
            Debug.LogError($"Cloud Save 데이터 가져오기 실패: {e.Message}");
            return null;
        }
    }
}
