using UnityEngine;

public class Player_New : MonoBehaviour
{
    public delegate void DamageEvent(ref float damage);
    public event DamageEvent OnDamageTaken;

    public static Player_New Instance { get; private set; } // 싱글턴 인스턴스

    [Header("Player Stats")]
    public float moveSpeed = 5f;
    public float gravity = 9.8f;
    public float groundOffset = 0.2f; // 지면 높이 오프셋
    //public float maxHp = 100f;
    //private float currentHp;

    [Header("References")]
    private CharacterController cc; // 캐릭터 컨트롤러
    private MainUI mainUI; // UI 컨트롤 스크립트 연결
    private CheckpointManager checkpointManager; // CheckpointManager와 연동

    private Vector3 velocity; // 이동 속도
    private bool isGrounded; // 지면에 닿았는지 여부

    private void Awake()
    {
        // 싱글턴 설정
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //currentHp = maxHp; // 초기 체력 설정
    }

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        mainUI = GetComponentInChildren<MainUI>();

        // CheckpointManager 가져오기
        checkpointManager = CheckpointManager.Instance;
        if (checkpointManager == null)
        {
            Debug.LogError("CheckpointManager가 씬에 존재하지 않습니다!");
        }
    }

    private void Update()
    {
        if (!checkpointManager.AllCheckpointsCompleted)
        {
            MoveToCheckpoint(); // 체크포인트로 이동
        }
        ApplyGravity();     // 중력 처리
    }

    private void MoveToCheckpoint()
    {
        if (checkpointManager == null || checkpointManager.AllCheckpointsCompleted) return;

        Vector3 targetPosition = checkpointManager.GetCurrentCheckpointPosition();
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0; // y축 이동 제거
        Vector3 moveVector = direction.normalized * moveSpeed;

        // 이동 처리
        cc.Move(moveVector * Time.deltaTime);

        // 플레이어가 현재 체크포인트에 도달했는지 확인
        bool reached = checkpointManager.IsPlayerAtCheckpoint(transform.position);
        //Debug.Log($"Is Player at checkpoint: {reached}");

        if (reached)
        {
            checkpointManager.MoveToNextCheckpoint();
        }
    }

    private void ApplyGravity()
    {
        // 지면 감지
        isGrounded = cc.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // 지면에 닿았을 때 Y축 속도 초기화
        }

        // Gravity 적용
        velocity.y -= gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
    }

    public void TakeDamage(float damage)
    {
        if (OnDamageTaken != null)
        {
            // 데미지를 받을 때 이벤트 호출
            OnDamageTaken.Invoke(ref damage);
        }

        if (damage > 0)
        {
            //currentHp -= damage;
            //currentHp = Mathf.Clamp(currentHp, 0, maxHp); // 체력 0 이하로 감소 방지
            //Debug.Log($"현재 체력: {currentHp}");
        }
        else
        {
            Debug.Log("데미지가 보호막에 의해 차단되었습니다.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("END"))
        {
            Debug.Log("END 태그와 충돌했습니다!");
            mainUI.EndGame();
        }
    }

    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }
}
