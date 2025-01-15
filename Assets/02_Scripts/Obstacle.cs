using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private bool Destructible = false; // 파괴 가능 여부
    [SerializeField] private float playerSpeedReduction = 0.5f; // 충돌 시 플레이어 속도 감소 비율 (0.5 = 50% 감소)
    
    // 오브젝트 파괴 처리
    private void destruction()
    {
        if (Destructible) //파괴 가능한 것만 파괴가능
        {
            Destroy(gameObject); // 장애물 삭제
        }
        else
        {
            return;
        }
    }

    // 플레이어와 충돌 처리
    private void OnTriggerEnter(Collider other)
    {
        //총알에 맞으면
        if (other.CompareTag("BULLET"))
        {
            destruction();
        }
    }
}
