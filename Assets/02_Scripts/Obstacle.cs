using UnityEngine;

public class Obstacle : MonoBehaviour, IDamageable
{
    [SerializeField] private bool Destructible = false; // 파괴 가능 여부
    
    // 오브젝트 파괴 처리
    public void TakeDamage()
    {
        if (Destructible) //파괴 가능한 것만 파괴가능
        {
            Destroy(gameObject); // 장애물 삭제
        }
    }

    // 플레이어와 충돌 처리
    //private void OnTriggerEnter(Collider other)
    //{
    //    //총알에 맞으면
    //    if (other.CompareTag("BULLET"))
    //    {
    //        destruction();
    //    }
    //}
}
