using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System.Threading.Tasks;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.Services.CloudSave;
using TMPro;

public class AnonymousLogin : MonoBehaviour
{
    public TMP_InputField nicknameInputField;
    [SerializeField] Button saveButton;
    private async void Start()
    {
        // Unity Services 초기화
        await UnityServices.InitializeAsync();
        Debug.Log("Unity Services Initialized");

        // 익명 로그인 시도
        await SignInAnonymously();

        // 버튼 클릭 이벤트 연결
        saveButton.onClick.AddListener(OnSaveButtonClicked);
    }

    private async Task SignInAnonymously()
    {
        try
        {
            // 익명 로그인
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

    // 저장 버튼 클릭 처리
    private async void OnSaveButtonClicked()
    {
        string nickname = nicknameInputField.text;

        if (string.IsNullOrEmpty(nickname))
        {
            Debug.LogWarning("닉네임이 입력되지 않았습니다.");
            // 닉네임 입력 없이 버튼 클릭하면 닉네임 자동 지정
            nickname = "JamesBond";
        }

        await SaveNicknameToCloud(nickname);
    }

    // 닉네임을 Cloud Save에 저장
    private async Task SaveNicknameToCloud(string nickname)
    {
        // 저장할 데이터
        var data = new Dictionary<string, object>
        {
            {"player_name", nickname}
        };

        //데이터 저장
        try
        {
            await CloudSaveService.Instance.Data.Player.SaveAsync(data);
            Debug.Log("Data를 성공적으로 저장했습니다.");
        }
        catch (CloudSaveException e)
        {
            Debug.LogError($"Cloud Save Exception: {e.Message}");
        }
    }
}
