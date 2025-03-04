using UnityEngine;
using System.Collections;

public class Player_New : MonoBehaviour
{
    public static Player_New Instance { get; private set; }

    [SerializeField] private bool tutorial = false; //튜토리얼
    [SerializeField] private float tutorialDelay = 30f; // 튜토리얼 대기 시간
    public bool tutorialInProgress = false;

    [SerializeField] private float baseSpeed = 5f;  // 기본 속도
    [SerializeField] private float acceleration = 0.01f; // 증가 속도
    [SerializeField] private float maxSpeed = 10f;  // 최대 속도
    [SerializeField] private float rotationSpeed = 5f;  // 회전 속도
    [SerializeField] private float gravity = -9.81f;  // 중력 설정
    [SerializeField] private float damage; // 적에게 피격 시 받는 속도 감소량

    public RedScreen redScreen; // 피격시 나오는 스크린
    public MainUI mainUI; // EndGame() 함수를 호출할 스크립트 연결

    public float currentSpeed = 0f; // 현재 속도

    public GameObject Drone; //드론
    private Vector3 DronPositon;

    private CharacterController characterController; // CharacterController 변수

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

        //튜토리얼 씬일때
        if (tutorial)
        {
            StartCoroutine(TutorialSetting());

        }
        else
        {
            currentSpeed = baseSpeed;
        }

    }

    private void FixedUpdate()
    {
        if (CheckPointManager.Instance == null || CheckPointManager.Instance.PlayerAllCheckpointsCompleted)
        {
            // 모든 체크포인트 완료 시 멈춤
            return;
        }

        if (tutorial) //Tutorial 씬 여부 확인
        {
            //튜토 30초 대기 후 진행
            if (!tutorialInProgress)
            {
                // 체크포인트 이동
                MoveToCheckpoint();
                // 회전
                RotateTowardsDrone();
            }
        }
        else // Main씬
        {
            // 속도 증가 처리
            IncreaseSpeed();

            // 체크포인트 이동
            MoveToCheckpoint();

            RotateTowardsDrone();
        }
    }

    //튜토리얼 세팅
    IEnumerator TutorialSetting()
    {
        // 초기 속도 0
        currentSpeed = 0f;
        tutorialInProgress = true;

        //Debug.Log("코루틴 들어감");

        //튜토리얼 대기
        yield return new WaitForSeconds(tutorialDelay);

        //Debug.Log("코루틴 끝");
        // 대기 완료 후 기본속도로 이동
        currentSpeed = baseSpeed;
        tutorialInProgress = false;
    }

    void IncreaseSpeed()
    {
        // 속도 증가
        currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, maxSpeed);
    }

    public void TakeDamage()
    {
        currentSpeed = Mathf.Max(currentSpeed - damage, 0); // 속도 감소, 음수 방지
        redScreen.TriggerRedScreenEffect(); // 피격 스크린 띄우기
        //Debug.Log("적 총알 맞음");
    }

    void RotateTowardsDrone()
    {
        // 드론의 위치를 가져와 y 값을 현재 플레이어의 y 값으로 설정
        Vector3 dronePosition = new Vector3(Drone.transform.position.x, this.transform.position.y, Drone.transform.position.z);

        // 드론을 바라보는 방향을 계산
        Vector3 directionToDrone = dronePosition - transform.position;

        // 회전할 방향이 있을 경우 부드럽게 회전
        if (directionToDrone != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToDrone);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void MoveToCheckpoint()
    {
        Vector3 targetCheckpointPosition = CheckPointManager.Instance.GetCheckpointPosition(true); // 플레이어 체크포인트 위치 가져오기
        Vector3 directionToCheckpoint = targetCheckpointPosition - transform.position;
        directionToCheckpoint.y = 0; // y축 무시

        float distanceToCheckpoint = directionToCheckpoint.magnitude; // 거리 계산
        directionToCheckpoint.Normalize(); // 방향 정규화

        float moveSpeed = Mathf.Min(currentSpeed * Time.deltaTime, distanceToCheckpoint);

        Vector3 movement = directionToCheckpoint * moveSpeed;
        movement.y = gravity * Time.deltaTime;  // 중력 적용

        characterController.Move(movement);  // 이동

        // 체크포인트에 도달했는지 확인
        if (CheckPointManager.Instance.IsAtCheckpoint(transform.position, true))
        {
            CheckPointManager.Instance.MoveToNextCheckpoint(true); // 플레이어 체크포인트 이동
        }
    }

    //private void OnDrawGizmos()
    //{
    //    if (CheckPointManager.Instance != null && !CheckPointManager.Instance.PlayerAllCheckpointsCompleted)
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawLine(transform.position, CheckPointManager.Instance.GetCheckpointPosition(true));
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("END"))
        {
            //Debug.Log("END 태그 오브젝트와 충돌!");
            mainUI.EndGame();
        }
    }
}
