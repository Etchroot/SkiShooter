using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System;
using Unity.Services.Leaderboards;
using Newtonsoft.Json;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] private GameObject scrollViewContent; // ScrollView의 Content
    [SerializeField] private GameObject myDataViewContent; // 내 기록의 Content
    [SerializeField] Button backtoTitleButton;
    [SerializeField] Button exitGameButton;
    [SerializeField] private CanvasGroup fadeCanvasGroup; // fadeout용 canvas
    private GameObject playerDataPrefab; // UI에 사용할 Player Data 항목 프리팹
    private GameObject myDataPrefab; // UI에 사용할 Player Data 항목 프리팹
    private bool logincheck = false;

    void Awake()
    {
        playerDataPrefab = Resources.Load<GameObject>("Text_Ranklist");
        if (playerDataPrefab == null)
        {
            Debug.Log("playerDataPrefab이 null입니다.");
        }
        myDataPrefab = Resources.Load<GameObject>("Text_MyRanklist");
        if (myDataPrefab == null)
        {
            Debug.Log("myDataPrefab이 null입니다.");
        }
        if (scrollViewContent == null)
        {
            Debug.Log("scrollviewcontent가 null입니다.");
        }
        if (myDataViewContent == null)
        {
            Debug.Log("mydataviewcontent가 null입니다.");
        }

        backtoTitleButton.onClick.AddListener(onBackTitleButtonClick);
        exitGameButton.onClick.AddListener(onExitGameButtononClick);

        Login();
    }

    async void Login()
    {
        try
        {
            // Unity Authentication 서비스 초기화
            await UnityServices.InitializeAsync();

            // 로그인 처리 (자동으로 로그인하는 방식)
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                logincheck = true;
                Debug.Log("로그인 성공: " + AuthenticationService.Instance.PlayerId);
            }
            else
            {
                logincheck = true;
                Debug.Log("이미 로그인되었습니다.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("로그인 실패: " + e.Message);
        }
    }

    async void Start()
    {
        try
        {
            // 로그인 체크 완료 후 실행
            await WaitForLogin();
            await FetchAndSortPlayerData();
            await ViewMyPlayerData();
        }
        catch (Exception e)
        {
            Debug.LogError($"로그인 체크 안됨 :{e.Message}");
            return;
        }
    }

    // 로그인 완료 대기
    async Task WaitForLogin()
    {
        while (!logincheck)
        {
            await Task.Delay(100); // 로그인 완료를 기다리는 동안 대기
        }
    }


    private async Task FetchAndSortPlayerData()
    {
        Debug.Log("데이터를 가져오는 중입니다.");
        try
        {
            // 모든 리더보드 데이터를 가져옵니다
            var leaderboardEntries = await LeaderboardsService.Instance.GetScoresAsync("Ranking", new GetScoresOptions { IncludeMetadata = true });
            Debug.Log(JsonConvert.SerializeObject(leaderboardEntries));

            foreach (var entry in leaderboardEntries.Results)
            {
                // 플레이어의 점수와 ID 가져오기
                string codename = entry.PlayerName;
                double score = entry.Score;
                int rank = entry.Rank + 1;
                string playtime = "기록없음";

                // 메타데이터를 파싱하고 "playtime" 값 추출
                if (!string.IsNullOrEmpty(entry.Metadata))
                {
                    var metadata = JsonConvert.DeserializeObject<Dictionary<string, string>>(entry.Metadata);
                    if (metadata != null && metadata.TryGetValue("playtime", out string extractedPlaytime))
                        playtime = extractedPlaytime; // 0:00으로 표현되는 숫자만 추출
                }

                Debug.Log($"랭킹 :{rank}, 플레이어 ID :{codename}, 점수 :{score}, 플레이타임 : {playtime}");

                TMP_Text playerData = Instantiate(playerDataPrefab, scrollViewContent.transform).GetComponent<TMP_Text>();
                if (playerData == null)
                {
                    Debug.LogError("playerDataPrefab이 null입니다.");
                }
                var textComponent = playerData.GetComponentInChildren<TextMeshProUGUI>();
                if (textComponent == null)
                {
                    Debug.LogError("TextMeshProUGUI 컴포넌트를 찾을 수 없습니다.");
                }
                else
                {
                    textComponent.text = $"{rank}등/ 코드네임: {codename}/  최종속도: {score}/  클리어시간: {playtime}";
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"데이터 가져오기 실패: {e.Message}");

        }
    }

    // 리더보드에서 내 데이터 받아오기
    private async Task ViewMyPlayerData()
    {
        try
        {
            var myScore = await LeaderboardsService.Instance.GetPlayerScoreAsync("Ranking", new GetPlayerScoreOptions { IncludeMetadata = true });

            string _codename = myScore.PlayerName;
            double _score = myScore.Score;
            int _rank = myScore.Rank + 1;
            var _metadata = JsonConvert.DeserializeObject<Dictionary<string, string>>(myScore.Metadata);
            string _playtime = _metadata?["playtime"] ?? "기록없음";

            // 캔버스에 내 데이터 표시
            TMP_Text myData = Instantiate(myDataPrefab, myDataViewContent.transform).GetComponent<TMP_Text>();
            myData.text = $"[나의 기록]  순위 : {_rank}/  코드네임 : {_codename}/  최종속도 : {_score}/  클리어시간 : {_playtime}";
        }
        catch (Exception e)
        {
            TMP_Text myData = Instantiate(myDataPrefab, myDataViewContent.transform).GetComponent<TMP_Text>();
            myData.text = "불러올 수 있는 데이터가 존재하지 않습니다.";
            Debug.LogError($"내 데이터 가져오기 실패: {e.Message}");
        }
    }

    void onBackTitleButtonClick()
    {
        StartCoroutine(LoadSceneFadeOut("01_Title"));
    }

    void onExitGameButtononClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private IEnumerator LoadSceneFadeOut(String scenename)
    {
        // Fade out 시작
        yield return StartCoroutine(FadeOut());

        // 씬 비동기 로드
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scenename);
        asyncLoad.allowSceneActivation = false;

        // 로딩이 완료될 떄까지 대기
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f) // 로딩이 완료되면
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Clamp01(elapsedTime / 1f); // 서서히 어두워짐
            yield return null;
        }
        fadeCanvasGroup.alpha = 1f; // 완전히 어두워짐
    }
}
