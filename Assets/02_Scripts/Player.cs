using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public delegate void DamageEvent(ref float damage);
    public event DamageEvent OnDamageTaken;
    public static Player Instance { get; private set; }
    public float moveSpeed = 5f; // 기본 이동속도
    public float gravity = 9.8f;
    public float groundOffset = 0.2f; //지면 오프셋
    public float maxHp = 100f;
    private float currentHp;
    private CharacterController cc;
    private Vector3 velocity; //속도 변수


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
        currentHp = maxHp;
    }
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }


    void Update()
    {
        // 플레이어 이동 처리
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        //direction.y -= 10f * Time.deltaTime;
        //transform.Translate(direction * moveSpeed * Time.deltaTime);

        #region  강사님 이동로직
        // Debug.DrawRay(transform.position, Vector3.down * 0.2f, Color.green);

        // if (Physics.Raycast(transform.position + Vector3.down * 1f, Vector3.down, out var hit, 0.2f))
        // {
        //     if (hit.collider.CompareTag("Road"))
        //     {
        //         direction.y = 0.0f;
        //     }
        // }
        // else
        // {
        //     direction.y -= 100.0f * Time.deltaTime;
        // }

        // cc.Move(direction * moveSpeed * Time.deltaTime);
        // Debug.Log(cc.isGrounded);
        #endregion
        #region  GPT 이동로직
        // //이동 처리
        // if (cc.isGrounded)
        // {
        velocity = direction * moveSpeed;
        //}
        // else
        // {
        //velocity.y -= gravity * Time.deltaTime;
        velocity.y = 0; //y축 이동 막기
        //}
        cc.Move(velocity * Time.deltaTime);
        Debug.Log(cc.isGrounded);

        // Terrain 높이 보정
        float terrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);

        //캐릭터를 Terrain 위로 보정
        Vector3 newPosition = transform.position;
        newPosition.y = terrainHeight + groundOffset;
        transform.position = newPosition;
        // if (transform.position.y < terrainHeight + groundOffset)
        // {
        //     transform.position = new Vector3(transform.position.x, terrainHeight + groundOffset, transform.position.z);
        // }
        #endregion
        // 데미지 받는 임시 메소드
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(10f);
        }
    }

    public void TakeDamage(float damage)
    {
        // 데미지를 받을 때 이벤트 발생
        OnDamageTaken?.Invoke(ref damage);

        // 이벤트 구독자가 데미지를 막았는지 확인
        if (damage > 0)
        {
            Debug.Log(damage);
            currentHp -= damage;
            currentHp = Mathf.Clamp(currentHp, 0, maxHp); // 체력 0이하로 감소하지 않게 고정
            Debug.Log($"현재 Hp: {currentHp}");
        }
        else
        {
            Debug.Log("데미지가 보호막에 의해 차단되었습니다.");
        }



    }

    // 이동속도를 설정하는 메서드
    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

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
