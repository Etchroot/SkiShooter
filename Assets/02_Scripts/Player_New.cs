using UnityEngine;

public class Player_New : MonoBehaviour
{
    public static Player_New Instance { get; private set; }

    [SerializeField] private float baseSpeed = 5f; // 기본 속도
    [SerializeField] private float acceleration = 0.01f; // 매 프레임마다 속도가 증가하는 정도
    [SerializeField] private float maxSpeed = 10f; // 최대 속도
    public float currentSpeed = 0f; // 현재 속도

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentSpeed = baseSpeed;
    }

    private void Update()
    {
        MoveToCheckpoint();
    }

    void MoveToCheckpoint()
    {
        if (CheckPointManager.Instance.AllCheckpointsCompleted)
            return;

        Vector3 targetCheckpointPosition = CheckPointManager.Instance.GetCurrentCheckpointPosition();

        // Y축을 무시하고 X, Z축만 이동하도록 설정
        Vector3 directionToCheckpoint = targetCheckpointPosition - transform.position;
        directionToCheckpoint.y = 0; // Y축을 무시

        // 현재 위치와 목표 위치 간의 거리 계산
        float distanceToCheckpoint = Vector3.Distance(transform.position, targetCheckpointPosition);

        // Y값을 0으로 설정하여 이동 중 Y값 영향을 받지 않도록 함
        targetCheckpointPosition.y = transform.position.y;

        // 이동 속도 계산
        float moveSpeed = Mathf.Min(currentSpeed * Time.deltaTime, distanceToCheckpoint);

        // 이동
        transform.position = Vector3.MoveTowards(transform.position, targetCheckpointPosition, moveSpeed);

        // 체크포인트에 도달했는지 확인
        if (CheckPointManager.Instance.IsPlayerAtCheckpoint(transform.position))
        {
            CheckPointManager.Instance.MoveToNextCheckpoint();
        }
    }


    private void OnDrawGizmos()
    {
        if (CheckPointManager.Instance != null && !CheckPointManager.Instance.AllCheckpointsCompleted)
        {
            Gizmos.color = Color.red;

            // 현재 위치에서 다음 체크포인트까지의 경로를 그립니다.
            Gizmos.DrawLine(transform.position, CheckPointManager.Instance.GetCurrentCheckpointPosition());
        }
    }
}
