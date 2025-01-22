using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DroneSpawner : MonoBehaviour
{
    [SerializeField] private GameObject dronePrefab; // 드론 프리팹
    [SerializeField] private Transform[] spawnPoints; // 스폰 위치 배열
    [SerializeField] private float spawnInterval = 5f; // 스폰 간격
    [SerializeField] private Transform player; // 플레이어의 Transform
    [SerializeField] private int maxDrones = 2; // 최대 드론 개수 제한

    private List<GameObject> activeDrones = new List<GameObject>(); // 활성화된 드론 리스트

    void Start()
    {
        StartCoroutine(SpawnDrones());
    }

    private IEnumerator SpawnDrones()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // 활성화된 드론의 개수를 확인
            if (activeDrones.Count >= maxDrones)
            {
                continue; // 최대 개수 도달 시 스폰 중단
            }

            // 랜덤한 위치에서 드론 스폰
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject drone = Instantiate(dronePrefab, spawnPoint.position, spawnPoint.rotation);

            // 플레이어 설정
            EnemyDrone droneScript = drone.GetComponent<EnemyDrone>();
            if (droneScript != null)
            {
                droneScript.player = player;
            }

            // 활성 드론 리스트에 추가
            activeDrones.Add(drone);

            // 드론이 파괴될 때 리스트에서 제거하도록 설정
            drone.GetComponent<EnemyDrone>().onDestroyed += () =>
            {
                activeDrones.Remove(drone);
            };
        }
    }
}
