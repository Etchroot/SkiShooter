using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // 총알 스크립트
    // 생성된 방향의 앞으로 날아가며 물체와 부딪히거나 5초 후 Destroy

    [SerializeField] private float BulletSpeed = 5f;

    void Start()
    {
        // 생성되고 5초 후 삭제
        StartCoroutine(DestroyAfterTime());
    }

    void Update()
    {
        // 이동
        transform.Translate(Vector3.forward * BulletSpeed * Time.deltaTime);
    }

    // 다른 게임 오브젝트와 닿으면 삭제
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ENEMY"))
        {
            // 적군: 체력 감소 또는 제거
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                //enemy.TakeDamage(1); // 적군에 데미지 1 적용
            }
            Destroy(this.gameObject); // 총알 제거
        }
        else if (other.CompareTag("PLAYER"))
        {
            // 플레이어: 속도 감소 또는 체력 감소
            //PlayerMovement player = other.GetComponent<PlayerMovement>();
            //if (player != null)
            //{
            //    player.ReduceSpeed(0.5f); // 플레이어 속도를 50% 감소
            //}
            //Destroy(this.gameObject); // 총알 제거
        }
        else
        {
            // 바닥이나 다른 곳과 부딪혔을 때
            Destroy(this.gameObject);
        }
    }

    IEnumerator DestroyAfterTime()
    {
        // 생성되고 5초 지나면 제거
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }
}
