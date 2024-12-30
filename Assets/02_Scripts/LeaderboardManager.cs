using UnityEngine;
using Unity.Services.CloudSave;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using Newtonsoft.Json;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] private GameObject scrollViewContent; // ScrollView의 Content
    [SerializeField] private GameObject playerDataPrefab; // UI에 사용할 Player Data 항목 프리팹
    private bool logincheck = false;

    public class PlayerDataUI
    {
        [SerializeField] private TextMeshProUGUI codenameText;
        [SerializeField] private TextMeshProUGUI speedText;
        [SerializeField] private TextMeshProUGUI playtimeText;

        public void SetData(string codename, int speed, string playtime)
        {
            codenameText.text = codename;
            speedText.text = $"{speed} 점";
            playtimeText.text = playtime;
        }
    }

    [System.Serializable]
    public class PlayerData
    {
        public string codename = "Unknown";    // 플레이어 코드네임
        public int final_speed = 0;    // 플레이어 최종 점수
        public string playtime = "00:00";    // 플레이어 플레이타임
    }

    void Awake()
    {
        login();
    }

    async void login()
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
            }
            Debug.Log("로그인 성공: " + AuthenticationService.Instance.PlayerId);

            if (logincheck == true)
            {
                await FetchAndSortPlayerData();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("로그인 실패: " + e.Message);
        }
    }


    private async Task FetchAndSortPlayerData()
    {
        Debug.Log("데이터를 가져오는 중입니다.");
        try
        {
            // 모든 리더보드 데이터를 가져옵니다
            var leaderboardEntries = await LeaderboardsService.Instance.GetScoresAsync("Ranking");
            Debug.Log(JsonConvert.SerializeObject(leaderboardEntries));
            // Debug.Log($"총 {ldeaderboardID.Count}개의 데이터 항목을 가져왔습니다.");  // 데이터 개수 확인

            // 데이터 리스트를 정리합니다
            //List<PlayerData> playerDataList = new List<PlayerData>();

            foreach (var entry in leaderboardEntries.Results)
            {
                // 플레이어의 점수와 ID 가져오기
                string playerId = entry.PlayerId;
                int score = (int)entry.Score;
                Debug.Log($"플레이어 ID :{playerId}, 점수 :{score}");

                // Cloud Save에서 추가 데이터를 가져옵니다 (nickname, playtime)
                var playerData = await CloudSaveService.Instance.Data.Player.LoadAllAsync();

                // ScrollView에 데이터 표시
                // DisplayPlayerData(playerData.codename, score, playerData.playtime);
            }


            // // final_speed 기준으로 내림차순 정렬
            // var sortedList = playerDataList.OrderByDescending(player => player.final_speed).ToList();
            // Debug.Log($"정렬된 데이터 항목 수: {sortedList.Count}");  // 정렬 후 데이터 개수 확인

            // // ScrollView에 데이터 표시
            // DisplayDataInScrollView(sortedList);
        }
        catch (Exception e)
        {
            Debug.LogError($"데이터 가져오기 실패: {e.Message}");
        }
    }

    // private async Task<PlayerData> LoadPlayerData(string playerId)
    // {
    //     try
    //     {
    //         var options = new Unity.Services.CloudSave.Models.Data.Player.LoadAllOptions();
    //         // playerId에 해당하는 데이터를 비동기적으로 불러옴.
    //         var playerDataJson = await CloudSaveService.Instance.Data.Player.LoadAllAsync(options);

    //         // playerId에 해당하는 데이터를 추출
    //         if (playerDataJson.ContainsKey(plyaerId))
    //         {
    //             var jsonData = playerDataJson[playerId].Value; // SaveItem의 Value가 실제 Json 데이터
    //         // Json 데이터를 playerData 객체로 변환하여 반환
    //         return JsonUtility.FromJson<PlayerData>(jsonData);

    //         }
    //     }
    //     catch (Exception e)
    //     {
    //         Debug.LogError($"Cloud save에서 데이터를 가져오는데 실패했습니다: {e.Message}");
    //         return null;
    //     }
    // }

    private void DisplayPlayerData(string nickname, int score, string playtime)
    {
        var newItem = Instantiate(playerDataPrefab, scrollViewContent.transform);
        newItem.GetComponent<PlayerDataUI>().SetData(nickname, score, playtime);
        // Debug.Log($"Displaying data: {sortedData.Count} items");  // 데이터 수 확인
        // foreach (Transform child in scrollViewContent.transform)
        // {
        //     Destroy(child.gameObject); // 이전 데이터 삭제
        // }

        // foreach (var player in sortedData)
        // {
        //     Debug.Log($"Displaying player: {player.codename}, {player.final_speed}, {player.playtime}");
        //     var newItem = Instantiate(playerDataPrefab, scrollViewContent.transform);
        //     newItem.GetComponent<PlayerDataUI>().SetData(player.codename, player.final_speed, player.playtime);
        // }
    }

}
