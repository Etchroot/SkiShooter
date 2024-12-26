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
    private GunShooting leftgunShooting;
    private GunShooting rightgunShooting;
    private Player player;
    private string leaderboardID = "Ranking"; // LeaderBoard ID 설정
    private bool isLeftReloadAcive = false; // 현재 리로드 텍스트 상태
    private bool isRightReloadAcive = false; // 현재 리로드 텍스트 상태
    private bool isGameRunning = true; // 게임 진행 상태
    private float palytime = 0f; // 플레이타임
    private float palytimetoscore = 0f; // 점수로 표현되는 플레이타임
    private float finalSpeed = 0f; // 최종 속력
    void Start()
    {
        // 게임 시작 시 초기화
        palytime = 0f;
        palytimetoscore = 0f;
        isGameRunning = true;
        //GameObject LeftController = GameObject.FindWithTag("LCONT");

        if (LeftController != null)
        {
            Debug.Log("LCONT 태그를 가진 오브젝트르 찾았습니다.");
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


        Timer();
        LeftReloading();
        RightReloading();
        LeftReload();
        RightReload();
    }



    // 리더보드에 점수 제출
    private async Task SubmitScoreToLeaderboard(string nickname, int score, string Time)
    {
        try
        {
            // Leaderboard에 점수 제출
            await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardID, score);
            Debug.Log("리더보드에 닉네임과 점수 저장 완료");
        }
        catch (RequestFailedException e)
        {
            Debug.LogError($"리더보드 점수 저장 실패: {e.Message}");
        }
    }

    // Cloud Save에 부가 데이터 저장
    private async Task SaveAdditionalDataToCloud(string nickname, string time, int score)
    {
        var data = new Dictionary<string, object>
        {
            {"codename", nickname},
            {"playtime", time},
            {"final_speed", score}
        };
        try
        {
            await CloudSaveService.Instance.Data.Player.SaveAsync(data);
            Debug.Log("Cloud Save에 추가 데이터 저장 완료");
        }
        catch (CloudSaveException e)
        {
            Debug.LogError($"Cloud Save 저장 실패: {e.Message}");
        }
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
        palytime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(palytime / 60); // 분 계산
        int seconds = Mathf.FloorToInt(palytime % 60); // 초 계산

        TimerText.text = $"{minutes:D1}:{seconds:D2}"; // 자릿수 포맷
    }

    // 게임 종료시 처리
    public async void EndGame()
    {
        isGameRunning = false;

        palytimetoscore += Time.deltaTime;
        int minutes = Mathf.FloorToInt(palytimetoscore / 60); // 분 계산
        int seconds = Mathf.FloorToInt(palytimetoscore % 60); // 초 계산
        Debug.Log($"최종 플레이타임: {palytimetoscore}");

        finalSpeed = Player.Instance.moveSpeed;

        // 플레이 타임과 최종 속력을 정수로 변환
        string finalTime = string.Format("{0:00}:{1:00}", minutes, seconds);
        int finalScore = Mathf.FloorToInt(finalSpeed); // 점수는 최종 속력으로

        string nickname = PlayerPrefs.GetString("PlayerNickname", "UnknownPlayer");

        // Leaderboard와 Cloud Save에 데이터 전송
        await SubmitScoreToLeaderboard(nickname, finalScore, finalTime);
        await SaveAdditionalDataToCloud(nickname, finalTime, finalScore);
    }
}
