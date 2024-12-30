using UnityEngine;
using TMPro;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using System;
using Unity.Services.Leaderboards;
using Unity.Services.Core;
using System.Collections.Generic;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using Unity.Services.Authentication;
using UnityEngine.SceneManagement;

public class MainUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI LeftBulletText;
    [SerializeField] private TextMeshProUGUI RightBulletText;
    [SerializeField] private TextMeshProUGUI PlayerSpeedText;
    [SerializeField] private TextMeshProUGUI TimerText;
    [SerializeField] private GameObject LeftReloadText;
    [SerializeField] private GameObject LeftReloadImage;
    [SerializeField] private GameObject RightReloadText;
    [SerializeField] private GameObject RightReloadImage;
    [SerializeField] private GameObject LeftController;
    [SerializeField] private GameObject RightController;
    [SerializeField] private CanvasGroup fadeCanvasGroup; // fadeout 캔버스
    private GunShooting leftgunShooting;
    private GunShooting rightgunShooting;
    private Player player;
    private string leaderboardID = "Ranking"; // LeaderBoard ID 설정
    private bool isLeftReloadAcive = false; // 현재 리로드 텍스트 상태
    private bool isRightReloadAcive = false; // 현재 리로드 텍스트 상태
    [HideInInspector] public bool isGameRunning = true; // 게임 진행 상태
    private float palytime = 0f; // 플레이타임
    private float finalSpeed = 0f; // 최종 속력
    void Start()
    {
        // 게임 시작 시 초기화
        palytime = 0f;
        isGameRunning = true;
        //GameObject LeftController = GameObject.FindWithTag("LCONT");

        if (LeftController != null)
        {
            leftgunShooting = LeftController.GetComponent<GunShooting>();
        }
        if (leftgunShooting == null)
        {
            Debug.Log("LCONT 태그를 가진 오브젝트에서 gunshooting 스크립트를 찾을 수 없음");
        }

        //GameObject rightController = GameObject.FindWithTag("RCONT");

        if (RightController != null)
        {
            rightgunShooting = RightController.GetComponent<GunShooting>();
        }
        if (rightgunShooting == null)
        {
            Debug.Log("RCONT 태그를 가진 오브젝트에서 gunshooting 스크립트를 찾을 수 없음");
        }


    }



    // Update is called once per frame
    void Update()
    {
        LeftBulletText.text = $"{leftgunShooting.currentBullet}";
        RightBulletText.text = $"{rightgunShooting.currentBullet}";

        if (Player.Instance != null)
        {
            PlayerSpeedText.text = $"{Player.Instance.moveSpeed} km/h";

        }
        else
        {
            Debug.LogWarning("Player 인스턴스를 찾을 수 없습니다.");
        }

        palytime += Time.deltaTime;

        Timer();
        LeftReloading();
        RightReloading();
        LeftReload();
        RightReload();
    }



    // 리더보드에 점수 제출
    private async Task SubmitScoreToLeaderboard(string nickname, int score, string Time)
    {
        // Leaderboard에 Score 제출
        try
        {
            var metadata = new Dictionary<string, string>
            {
                {"playtime", Time} // Metadata에 finalTime 저장
            };

            var options = new AddPlayerScoreOptions
            {
                // AddPlayerScoreOptions 객체로 저장해야 하므로 타입을 변경
                Metadata = metadata
            };
            await LeaderboardsService.Instance.AddPlayerScoreAsync("Ranking", score, options);
            Debug.Log($"리더보드에 점수와 플레이타임 저장 완료 :{score}, {options}");
        }
        catch (RequestFailedException e)
        {
            Debug.LogError($"리더보드 점수 저장 실패: {e.Message}");
        }

        // Leaderboard에 Name 제출
        try
        {
            await AuthenticationService.Instance.UpdatePlayerNameAsync(nickname);
            Debug.Log($"리더보드에 코드네임 저장 완료 :{nickname}");
        }
        catch (Exception e)
        {
            Debug.LogError($"플레이어 이름 설정 실패 :{e.Message}");
        }

        // Leaderboard에 Metadata 제출
    }

    // Cloud Save에 부가 데이터 저장
    private async Task SaveAdditionalDataToCloud(string playerId, string nickname, string time, int score)
    {
        var playerData = new PlayerData
        {
            codename = nickname,
            playtime = time,
            final_speed = score
        };
        try
        {
            string jsonData = JsonUtility.ToJson(playerData);

            // Saveitem 객체로 감싸서 전달
            var saveData = new Dictionary<string, SaveItem>
            {
                {playerId, new SaveItem(jsonData,"")} // writeLock을 빈 문자열로 설정
            };
            await CloudSaveService.Instance.Data.Player.SaveAsync(saveData);
            Debug.Log("Cloud Save에 추가 데이터 저장 완료");
        }
        catch (CloudSaveException e)
        {
            Debug.LogError($"Cloud Save 저장 실패: {e.Message}");
        }
    }

    [System.Serializable]
    public class PlayerData
    {
        public string codename;
        public int final_speed;
        public string playtime;
    }

    void LeftReload()
    {
        int leftRemainBullet = leftgunShooting.currentBullet;


        if (leftRemainBullet == 0 && !isLeftReloadAcive)
        {
            Debug.Log("왼손 재장전 필요");
            LeftReloadText.SetActive(true);

            isLeftReloadAcive = true;
        }
        else if (leftRemainBullet != 0 && isLeftReloadAcive)
        {
            LeftReloadText.SetActive(false);
            isLeftReloadAcive = false;
        }
    }

    void RightReload()
    {
        int rightRemainBullet = rightgunShooting.currentBullet;

        if (rightRemainBullet == 0 && !isRightReloadAcive)
        {
            Debug.Log("오른손 재장전 필요");
            RightReloadText.SetActive(true);
            //RightReloadImage.SetActive(true);
            //Invoke("DeacitveRightReloadImage", 2f);
            isRightReloadAcive = true;
        }
        else if (rightRemainBullet != 0 && isRightReloadAcive)
        {
            RightReloadText.SetActive(false);
            isRightReloadAcive = false;
        }

    }

    void LeftReloading()
    {
        if (leftgunShooting.isReloading == true)
        {
            LeftReloadImage.SetActive(true);
            Invoke("DeacitveLeftReloadImage", 2f);
        }
    }
    void RightReloading()
    {
        if (rightgunShooting.isReloading == true)
        {
            RightReloadImage.SetActive(true);
            Invoke("DeacitveRightReloadImage", 2f);
        }
    }
    void DeacitveLeftReloadImage()
    {
        LeftReloadImage.SetActive(false);
    }
    void DeacitveRightReloadImage()
    {
        RightReloadImage.SetActive(false);
    }

    void Timer()
    {


        int minutes = Mathf.FloorToInt(palytime / 60); // 분 계산
        int seconds = Mathf.FloorToInt(palytime % 60); // 초 계산

        TimerText.text = $"{minutes:D1}:{seconds:D2}"; // 자릿수 포맷
    }

    // 게임 종료시 처리
    public async void EndGame()
    {
        int minutes = Mathf.FloorToInt(palytime / 60); // 분 계산
        int seconds = Mathf.FloorToInt(palytime % 60); // 초 계산
        Debug.Log($"최종 플레이타임: {palytime}");

        finalSpeed = Player.Instance.moveSpeed;

        // 플레이 타임과 최종 속력을 정수로 변환
        string finalTime = string.Format("{0}:{1:00}", minutes, seconds);
        int finalScore = Mathf.FloorToInt(finalSpeed); // 점수는 최종 속력으로
        string nickname = PlayerPrefs.GetString("PlayerNickname", "UnknownPlayer");
        string playerId = AuthenticationService.Instance.PlayerId;



        // Leaderboard와 Cloud Save에 데이터 전송
        await SubmitScoreToLeaderboard(nickname, finalScore, finalTime);
        await SaveAdditionalDataToCloud(playerId, nickname, finalTime, finalScore);

        isGameRunning = false;

        ChangeScene();
    }

    // 게임 엔드시 씬 변경
    public void ChangeScene()
    {
        Debug.Log("씬 변경 진입");
        if (!isGameRunning)
        {
            StartCoroutine(LoadSceneFadeOut("04_Leaderboard"));
            Debug.Log("씬을 변경합니다.");
        }
        else
        {
            Debug.LogError("씬이 변경되지 않았습니다.");
        }
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
