using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System.Threading.Tasks;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.Services.CloudSave;
using TMPro;
using Unity.Services.Leaderboards;

public class AnonymousLogin : MonoBehaviour
{
    public TMP_InputField nicknameInputField;
    [SerializeField] Button saveButton;

    //private string leaderboardID = "Ranking"; // LeaderBoard ID 설정
    private int score = 0; // 초기 점수 
    private async void Start()
    {
        // Unity Services 초기화
        await UnityServices.InitializeAsync();
        Debug.Log("Unity Services Initialized");



        Debug.Log("데이터 초기화 중");
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
            Debug.LogWarning("닉네임이 입력되지 않았습니다.");
            // 닉네임 입력 없이 버튼 클릭하면 닉네임 자동 지정
            nickname = "JamesBond";
        }

        // PlayerPrefs에 닉네임 저장
        PlayerPrefs.SetString("PlayerNickname", nickname);
        PlayerPrefs.Save();

        //await SaveNicknameToCloud(nickname);
        //await SubmitScoreToLeaderboard(nickname, score);
    }

    // // 닉네임을 Cloud Save에 저장
    // private async Task SaveNicknameToCloud(string nickname)
    // {
    //     // 저장할 데이터
    //     var data = new Dictionary<string, object>
    //     {
    //         {"code_name", nickname}
    //     };

    //     //데이터 저장
    //     try
    //     {
    //         await CloudSaveService.Instance.Data.Player.SaveAsync(data);
    //         Debug.Log("코드네임을 성공적으로 저장했습니다.");
    //     }
    //     catch (CloudSaveException e)
    //     {
    //         Debug.LogError($"Cloud Save Exception: {e.Message}");
    //     }

    // }

    // private async Task SubmitScoreToLeaderboard(string nickname, int score)
    // {
    //     try
    //     {
    //         // LeaderBoard에 스코어와 닉네임 제출
    //         await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardID, score);
    //         Debug.Log($"Leaderboar에 {nickname}의 점수 {score} 저장 완료");
    //     }
    //     catch (RequestFailedException e)
    //     {
    //         Debug.LogError($"Leaderboard에 저장 실패 : {e.Message}");
    //     }
    // }
}
