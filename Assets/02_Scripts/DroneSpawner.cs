using UnityEngine;
using UnityEngine.Pool;

public class DroneSpawner : MonoBehaviour
{
    [SerializeField] private GameObject dronePrefab; // 드론 프리팹
    [SerializeField] private Transform player; // 플레이어 Transform
    [SerializeField] private Transform leftTarget; // 플레이어 왼쪽 타겟
    [SerializeField] private Transform rightTarget; // 플레이어 오른쪽 타겟
    [SerializeField] private float spawnRadius = 10f; // 감지 범위
    [SerializeField] private float spawnDistanceOutside = 12f; // 스폰 범위 밖 거리

    private bool hasSpawned = false; // 스폰 여부

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 플레이어가 감지 범위 안에 있고 드론이 스폰되지 않았다면 스폰
        if (distanceToPlayer <= spawnRadius && !hasSpawned)
        {
            SpawnDrone();
            hasSpawned = true;
        }
        // 플레이어가 범위를 벗어나면 다시 스폰 가능
        else if (distanceToPlayer > spawnRadius + 5f)
        {
            hasSpawned = false;
        }
    }

    void SpawnDrone()
    {
        Vector3 spawnPosition = GetRandomPositionOutsideSpawnRadius();

        GameObject drone = ObjectPoolManager.GetObject(EPoolObjectType.EnemyDrone);

        drone.transform.position = spawnPosition;
        drone.transform.rotation = Quaternion.identity;

        EnemyDrone droneScript = drone.GetComponent<EnemyDrone>();
        if (droneScript != null)
        {
            droneScript.player = player;

            int ran = Random.Range(0, 2);
            droneScript.target = ran == 1 ? leftTarget : rightTarget;

            if (ran == 1) WarringSignTree.LeftSign();
            else WarringSignTree.RightSign();
        }
    }

    private Vector3 GetRandomPositionOutsideSpawnRadius()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector3 spawnDirection = new Vector3(randomDirection.x, 0, randomDirection.y);
        Vector3 spawnPosition = transform.position + spawnDirection * spawnDistanceOutside;
        spawnPosition.y = transform.position.y + 10;
        return spawnPosition;
    }
}
