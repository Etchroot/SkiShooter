using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private bool Destructible = false; // 파괴 가능 여부
    [SerializeField] private int hp = 1; // 장애물 체력 (0이 되면 파괴)
    [SerializeField] private float playerSpeedReduction = 0.5f; // 충돌 시 플레이어 속도 감소 비율 (0.5 = 50% 감소)

    private void Start()
    {
        // 파괴 가능 여부 초기화 로그
        Debug.Log($"{this.name} - 파괴 가능 여부: {Destructible}");
    }


    // 총알이나 공격을 받아 체력이 감소하는 로직
    public void TakeDamage(int damage)
    {
        if (Destructible) // 파괴 가능 오브젝트만 데미지 처리
        {
            hp -= damage;
            Debug.Log($"{this.name} 체력 감소: {hp}");
            if (hp <= 0)
            {
                destruction();
            }
        }
    }

    // 오브젝트 파괴 처리
    private void destruction()
    {
        Debug.Log($"{this.name} 파괴됨");
        Destroy(gameObject); // 장애물 삭제
    }

    // 플레이어와 충돌 처리
    private void OnTriggerEnter(Collider other)
    {
        // 플레이어와 충돌 확인
        if (other.CompareTag("PlayerHead"))
        {
            Debug.Log("플레이어와 충돌 발생!");

            // 플레이어의 속도를 감소시키는 로직
            //PlayerMovement playerMovement = other.GetComponentInParent<PlayerMovement>(); // 부모 객체에서 속도 관리 스크립트 찾기
            //if (playerMovement != null)
            //{
            //    playerMovement.ReduceSpeed(playerSpeedReduction); // 속도 감소
            //}
        }

        // 총알과의 충돌 확인
        if (other.CompareTag("BULLET"))
        {
            Debug.Log("장애물 총알 맞음");
            TakeDamage(1);
        }
    }
}
