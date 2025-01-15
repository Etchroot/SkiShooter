using UnityEngine;
using System.Collections;

public class Drone : MonoBehaviour
{
    [SerializeField] private bool tutorial; //튜토리얼
    private bool tutorialInProgress = false;

    public Transform playerTransform;

    private float targetHeight; // 목표 높이
    [SerializeField] private float Height = 5.0f;
    [SerializeField] private float heightSmoothSpeed = 100f; // 높이 변화 속도
    [SerializeField] private float rotationSpeed = 20f;  // 회전 속도
    
    private float raycastDistance = 20f; // Raycast 거리

    private Vector3 lastCheckpointPosition; //마지막 체크포인트
    private bool isRotating = false;    //회전중

    void Start()
    {
        lastCheckpointPosition = Vector3.zero;

        //튜토리얼 씬일 때
        if (tutorial)
        {
            //플레이어를 바라봄
            this.transform.LookAt(playerTransform);
        }
    }

    void Update()
    {
        if (CheckPointManager.Instance == null || CheckPointManager.Instance.PlayerAllCheckpointsCompleted)
        {
            // 플레이어 체크포인트 도달시 멈춤
            return;
        }

        //튜토리얼 씬일 때
        if (tutorial)
        {
            //플레이어 대기 확인
            tutorialInProgress = Player_New.Instance.tutorialInProgress;

            // 플레이어 코루틴 완료 후
            if (!tutorialInProgress)
            {
                // 체크포인트 이동
                MoveToCheckpoint();
            }
        }
        else // 메인 씬일 때
        {
            // 체크포인트 이동
            MoveToCheckpoint();
        }
    }

    private void UpdateHeightWithRaycast()
    {
        RaycastHit hit;

        // 드론 아래로 Raycast를 발사
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance))
        {
            float groundHeight = hit.point.y; // 바닥의 Y 좌표
            targetHeight = groundHeight + Height; // 바닥에서 5 유닛 위로 목표 높이 설정
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
        Vector3 horizontalPosition = new Vector3(targetCheckpointPosition.x, transform.position.y, targetCheckpointPosition.z);

        //체크포인트 변경시에 회전 
        if (targetCheckpointPosition != lastCheckpointPosition)
        {
            isRotating = true;

            lastCheckpointPosition = targetCheckpointPosition;
        }

        if (isRotating)
        {
            Rotate(horizontalPosition);
        }

        // 이동 거리 계산
        float moveDistance = Player_New.Instance.currentSpeed * Time.deltaTime;

        // 드론 이동
        transform.position = Vector3.MoveTowards(transform.position, horizontalPosition, moveDistance);

        // 높이
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

    void Rotate(Vector3 targetCheckpointPosition)
    {
        Vector3 directionToCheckpoint = (targetCheckpointPosition - transform.position).normalized;

        if (directionToCheckpoint == Vector3.zero)
        {
            Debug.LogWarning("회전 방향이 없습니다. (목표와 위치가 동일)");
            return;
        }

        // 목표 회전 계산
        Quaternion targetRotation = Quaternion.LookRotation(directionToCheckpoint);

        // 점진적 회전 적용
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // 회전 완료 체크
        if (Quaternion.Angle(transform.rotation, targetRotation) < 1.0f)
        {
            transform.rotation = targetRotation;
            isRotating = false;
            //Debug.Log("회전 완료!");
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
