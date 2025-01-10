using UnityEngine;

public class Drone : MonoBehaviour
{
    public float followDistance = 2f; // 플레이어 앞 유지 거리
    public float followHeight = 1.5f; // 드론의 높이
    public float moveSpeed = 5f; // 드론의 이동 속도

    private Transform playerTransform;
    private int currentCheckpointIndex = 0;

    void Start()
    {
        // 플레이어를 찾거나, 이미 지정된 플레이어 변수를 사용할 수 있습니다.
        //playerTransform = Player_New.Instance.transform;
    }

    void Update()
    {
        // 드론이 체크포인트를 따라가도록
        MoveToCheckpoint();

        // 플레이어 앞에 위치하도록
        //PositionInFrontOfPlayer();
    }

    // 드론이 체크포인트를 따라가는 로직
    void MoveToCheckpoint()
    {
        if (CheckPointManager.Instance.AllCheckpointsCompleted)
            return;

        Vector3 targetCheckpointPosition = CheckPointManager.Instance.GetCurrentCheckpointPosition();

        // 체크포인트까지의 방향 계산
        Vector3 directionToCheckpoint = targetCheckpointPosition - transform.position;
        directionToCheckpoint.y = 0; // Y축은 제외하고 이동 방향만 계산
        Vector3 moveDirection = directionToCheckpoint.normalized * moveSpeed;

        // 드론 이동
        transform.position = Vector3.MoveTowards(transform.position, targetCheckpointPosition, moveSpeed * Time.deltaTime);

        // 체크포인트에 도달한 경우
        if (CheckPointManager.Instance.IsPlayerAtCheckpoint(transform.position))
        {
            CheckPointManager.Instance.MoveToNextCheckpoint();
        }
    }

    // 드론을 항상 플레이어 앞에 배치
    void PositionInFrontOfPlayer()
    {
        if (playerTransform == null)
            return;

        Vector3 targetPosition = playerTransform.position + playerTransform.forward * followDistance;
        targetPosition.y = playerTransform.position.y + followHeight;

        transform.position = targetPosition;
        //transform.LookAt(playerTransform.position); // 플레이어를 바라봄
    }

    // 적군 위치 브리핑
    public void BriefEnemyPosition(Transform enemyTransform)
    {
        Vector3 directionToEnemy = enemyTransform.position - playerTransform.position;
        float angle = Vector3.SignedAngle(playerTransform.forward, directionToEnemy, Vector3.up);

        if (angle > -45 && angle <= 45)
        {
            Debug.Log("적군 위치: 앞");
        }
        else if (angle > 45 && angle <= 135)
        {
            Debug.Log("적군 위치: 오른쪽");
        }
        else if (angle > -135 && angle <= -45)
        {
            Debug.Log("적군 위치: 왼쪽");
        }
        else
        {
            Debug.Log("적군 위치: 뒤");
        }
    }
}
