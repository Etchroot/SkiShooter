using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System.Threading.Tasks;
using UnityEngine.UI;
using TMPro;


public class AnonymousLogin : MonoBehaviour
{
    public TMP_InputField nicknameInputField;
    [SerializeField] Button saveButton;


    private async void Start()
    {
        // Unity Services 초기화
        await UnityServices.InitializeAsync();
        Debug.Log("데이터 초기화 중");

        // 항상 새로 로그인 (기존 세션 제거)
        ClearSavedLoginData();

        // 익명 로그인 시도
        await SignInAnonymously();

        // 버튼 클릭 이벤트 연결
        saveButton.onClick.AddListener(OnSaveButtonClicked);
    }

    // 로그인 데이터 초기화
    private void ClearSavedLoginData()
    {
        // Unity가 저장한 익명 사용자 데이터를 내 디바이스에서 초기화
        PlayerPrefs.DeleteKey("com.unity.services.authentication.instance.playerid");
        PlayerPrefs.DeleteKey("com.unity.services.authentication.instance.sessiontoken");

        // 혹시 모를 캐싱 데이터도 초기화
        AuthenticationService.Instance.ClearSessionToken();
    }

    private async Task SignInAnonymously()
    {
        try
        {
            // 로그인 상태 확인
            if (AuthenticationService.Instance.IsSignedIn)
            {
                Debug.Log("이미 로그인 된 상태입니다. 세션을 초기화 합니다.");
                ClearSavedLoginData();
            }
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
    private void OnSaveButtonClicked()
    {
        string nickname = nicknameInputField.text;

        if (string.IsNullOrEmpty(nickname))
        {
            Debug.LogWarning("닉네임이 입력되지 않았습니다. 기본 닉네임으로 설정합니다.");
            // 닉네임 입력 없이 버튼 클릭하면 닉네임 자동 지정
            nickname = "JamesBond";
        }

        // PlayerPrefs에 닉네임 저장
        PlayerPrefs.SetString("PlayerNickname", nickname);
        PlayerPrefs.Save();
        Debug.Log($"닉네임 : {nickname}");
    }
}
