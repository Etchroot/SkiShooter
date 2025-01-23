using UnityEngine;

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
        // 플레이어가 스포너 감지 범위에 들어왔는지 확인
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= spawnRadius && !hasSpawned)
        {
            SpawnDrone();
            hasSpawned = true; // 한 번만 스폰
        }
    }

    void SpawnDrone()
    {
        // 스폰 범위 밖에서 드론 생성 위치 계산
        Vector3 spawnPosition = GetRandomPositionOutsideSpawnRadius();

        // 드론 생성
        GameObject drone = Instantiate(dronePrefab, spawnPosition, Quaternion.identity);

        // 랜덤 숫자 생성 (0 또는 1)
        int randomChoice = Random.Range(0, 2);

        // 드론에 타겟 설정
        EnemyDrone droneScript = drone.GetComponent<EnemyDrone>();
        if (droneScript != null)
        {
            droneScript.player = player;

            // 숫자에 따라 타겟 설정
            droneScript.target = randomChoice == 1 ? leftTarget : rightTarget;
        }
    }

    private Vector3 GetRandomPositionOutsideSpawnRadius()
    {
        // 랜덤 방향 계산
        Vector2 randomDirection = Random.insideUnitCircle.normalized; // 원형 방향
        Vector3 spawnDirection = new Vector3(randomDirection.x, 0, randomDirection.y);

        // 스포너 중심에서 spawnDistanceOutside 만큼 떨어진 위치 계산
        Vector3 spawnPosition = transform.position + spawnDirection * spawnDistanceOutside;

        return spawnPosition;
    }

    private void OnDrawGizmosSelected()
    {
        // 탐지 범위 (spawnRadius)를 노란색으로 표시
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);

        // 스폰 범위 바깥 거리 (spawnDistanceOutside)를 초록색으로 표시
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnDistanceOutside);
    }

}
