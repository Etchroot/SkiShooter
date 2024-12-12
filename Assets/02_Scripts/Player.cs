using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public float moveSpeed = 5f; // 기본 이동속도
    private float originalSpeed; // 원래 이동속도 (SpeedUp 후 복구용)

    // void Start()
    // {
    //     // 원래 이동속도 저장
    //     originalSpeed = moveSpeed;
    // }
    void Awake()
    {
        // 싱글턴 인스턴스 설정
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 변경 시에도 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 있다면 중복 생성 방지
        }
    }

    void Update()
    {
        // 플레이어 이동 처리
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    // 이동속도를 설정하는 메서드
    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

    // // 원래 속도로 복구하는 메서드
    // public void ResetMoveSpeed()
    // {
    //     moveSpeed = originalSpeed;
    // }

    // 게임 뷰에서 실시간으로 속도를 표시
    void OnGUI()
    {
        // 스타일 설정
        GUIStyle style = new GUIStyle();
        style.fontSize = 50; // 글자 크기 설정
        style.normal.textColor = Color.red; // 글자 색상 설정

        // 화면에 속도 표시 (왼쪽 상단)
        GUI.Label(new Rect(10, 10, 300, 80), "Move Speed: " + moveSpeed.ToString("F2"), style);
    }
}
