using UnityEngine;

public class Player_New : MonoBehaviour
{
    public static Player_New Instance { get; private set; }

    [SerializeField] private float baseSpeed = 5f;  // 기본 속도
    [SerializeField] private float acceleration = 0.01f; // 증가 속도
    [SerializeField] private float maxSpeed = 10f;  // 최대 속도
    [SerializeField] private float rotationSpeed = 5f;  //회전 속도
    [SerializeField] private float gravity = -9.81f;  // 중력 설정
    [SerializeField] private float damage = 1f; // 적에게 피격시 받는 속도 감소량
    public MainUI mainUI; // EndGame() 함수를 호출할 스크립트 연결

    public float currentSpeed = 0f; //현재 속도


    private Vector3 lastCheckpointPosition; //마지막 체크포인트
    private bool isRotating = false;    //회전중

    private CharacterController characterController; // CharacterController 변수

    /* 추가해야할 기능
     * 피격 (감속,피격보호)
     * 
     * 
     */

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
        characterController = GetComponent<CharacterController>(); // CharacterController 컴포넌트 참조
        currentSpeed = baseSpeed;
        lastCheckpointPosition = Vector3.zero;
    }

    private void Update()
    {
        if (CheckPointManager.Instance.AllCheckpointsCompleted)
        {
            // 모든 체크포인트 완료 시 멈춤
            return;
        }
        else
        {
            // 속도 증가 처리
            IncreaseSpeed();
        }

        // 체크포인트 이동
        MoveToCheckpoint();
    }

    void IncreaseSpeed()
    {
        //속도 증가
        currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, maxSpeed);
    }

    public void TakeDamage()
    {
        currentSpeed -= damage;
    }

    void MoveToCheckpoint()
    {
        if (CheckPointManager.Instance.AllCheckpointsCompleted)
            return;

        Vector3 targetCheckpointPosition = CheckPointManager.Instance.GetCurrentCheckpointPosition();
        Vector3 directionToCheckpoint = targetCheckpointPosition - transform.position;
        directionToCheckpoint.y = 0; // y축 무시

        // 체크포인트가 변경되었는지 확인
        if (targetCheckpointPosition != lastCheckpointPosition)
        {
            isRotating = true;

            lastCheckpointPosition = targetCheckpointPosition;
        }

        // 회전
        if (isRotating)
        {
            Rotate(directionToCheckpoint);
        }

        float distanceToCheckpoint = Vector3.Distance(transform.position, targetCheckpointPosition);
        targetCheckpointPosition.y = transform.position.y; // y축 무시

        float moveSpeed = Mathf.Min(currentSpeed * Time.deltaTime, distanceToCheckpoint);

        Vector3 movement = directionToCheckpoint.normalized * moveSpeed;
        movement.y = gravity * Time.deltaTime;  // 중력 적용

        characterController.Move(movement);  // 이동

        // 체크포인트에 도달했는지 확인
        if (CheckPointManager.Instance.IsPlayerAtCheckpoint(transform.position))
        {
            CheckPointManager.Instance.MoveToNextCheckpoint();
        }
    }

    void Rotate(Vector3 directionToCheckpoint)
    {
        if (directionToCheckpoint == Vector3.zero)
            return;

        // 회전
        Quaternion targetRotation = Quaternion.LookRotation(directionToCheckpoint);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // 회전 완료 체크
        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
        {
            transform.rotation = targetRotation;
            isRotating = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (CheckPointManager.Instance != null && !CheckPointManager.Instance.AllCheckpointsCompleted)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, CheckPointManager.Instance.GetCurrentCheckpointPosition());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트의 태그가 "END"인지 확인
        if (other.CompareTag("END"))
        {
            Debug.Log("END 태그 오브젝트와 충돌!");
            mainUI.EndGame();
        }
    }
}
