using UnityEngine;

public class Drone : MonoBehaviour
{
    public Transform playerTransform;

    private float targetHeight; // 목표 높이
    private float heightSmoothSpeed = 100f; // 높이 변화 속도
    private float raycastDistance = 20f; // Raycast 거리

    void Start()
    {
        
    }

    void Update()
    {
        if (CheckPointManager.Instance == null || CheckPointManager.Instance.PlayerAllCheckpointsCompleted)
        {
            // 플레이어 체크포인트 도달시 멈춤
            return;
        }

        // 드론 이동 로직
        MoveToCheckpoint();
    }

    private void UpdateHeightWithRaycast()
    {
        RaycastHit hit;

        // 드론 아래로 Raycast를 발사
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance))
        {
            float groundHeight = hit.point.y; // 바닥의 Y 좌표
            targetHeight = groundHeight + 5.0f; // 바닥에서 5 유닛 위로 목표 높이 설정
        }
        else
        {
            Debug.LogWarning("Raycast가 바닥을 감지하지 못했습니다.");
        }
    }

    void MoveToCheckpoint()
    {
        if (CheckPointManager.Instance == null)
            return;

        // 체크포인트 위치 가져오기
        Vector3 targetCheckpointPosition = CheckPointManager.Instance.GetCheckpointPosition(false);

        // 수평 이동만 처리
        Vector3 horizontalPosition = new Vector3(targetCheckpointPosition.x, transform.position.y, targetCheckpointPosition.z);

        // 이동 거리 계산
        float moveDistance = Player_New.Instance.currentSpeed * Time.deltaTime;

        // 드론 수평 이동
        transform.position = Vector3.MoveTowards(transform.position, horizontalPosition, moveDistance);

        // 높이 업데이트 (Raycast 사용)
        UpdateHeightWithRaycast();

        // 드론 높이 보정 적용
        Vector3 correctedPosition = new Vector3(transform.position.x, targetHeight, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, correctedPosition, Time.deltaTime * heightSmoothSpeed);

        // 체크포인트 도달 확인
        if (CheckPointManager.Instance.IsAtCheckpoint(transform.position, false))
        {
            CheckPointManager.Instance.MoveToNextCheckpoint(false);
        }
    }


    private void OnDrawGizmos()
    {
        // Raycast 시각화 (디버깅용)
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, Vector3.down * raycastDistance);
    }


    // 적군 위치 브리핑
    public void BriefEnemyPosition(Transform enemyTransform)
    {
        if (playerTransform == null)
            return;

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
