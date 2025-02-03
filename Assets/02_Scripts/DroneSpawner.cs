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
    private IObjectPool<GameObject> dronePool; // 드론 오브젝트 풀

    void Start()
    {
        // 드론 풀을 먼저 생성
        ObjectPoolManager.Instance.CreatePool("drone", dronePrefab, 10, 20);

        // 풀을 직접 가져오기
        dronePool = ObjectPoolManager.Instance.GetPool("drone");
        if (dronePool == null)
        {
            Debug.LogError("Drone pool is null!");
        }
    }

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

        // ObjectPoolManager에서 드론을 풀에서 가져옵니다.
        GameObject drone = dronePool.Get();

        if (drone == null)
        {
            Debug.LogError("Failed to get drone from pool!");
            return;
        }

        drone.transform.position = spawnPosition;
        drone.transform.rotation = Quaternion.identity;

        EnemyDrone droneScript = drone.GetComponent<EnemyDrone>();
        if (droneScript != null)
        {
            droneScript.player = player;
            droneScript.target = Random.Range(0, 2) == 1 ? leftTarget : rightTarget;
            droneScript.SetPool(dronePool); // 드론에 풀 전달
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
