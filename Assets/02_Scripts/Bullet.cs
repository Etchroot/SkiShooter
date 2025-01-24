using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float BulletSpeed = 50f;  // 속도 증가
    [SerializeField] private float BulletTime = 3f;
    private GunShooting gunShooting; // GunShooting 참조

    private float lifetime;

    public void Initialize(GunShooting gunShootingInstance)
    {
        gunShooting = gunShootingInstance;
    }

    void Start()
    {
        lifetime = BulletTime;
    }

    void Update()
    {
        RaycastHit hit;

        // 총알 이동과 동시에 레이캐스트 발사
        if (Physics.Raycast(transform.position, transform.forward, out hit, BulletSpeed * Time.deltaTime))
        {
            // 태그로 먼저 필터링
            if (hit.collider.CompareTag("BARREL") || hit.collider.CompareTag("ENEMY")|| hit.collider.CompareTag("OBSTACLE"))
            {
                // 해당 오브젝트가 IDamageable을 구현했는지 확인
                IDamageable damageable = hit.collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(); // 데미지 처리
                    gunShooting.ReturnBulletToPool(this.gameObject); // 총알 풀로 반환
                    return;
                }
            }
        }


        // 실제 총알 이동
        transform.Translate(Vector3.forward * BulletSpeed * Time.deltaTime);

        // 일정 시간이 지나면 풀로 반환
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            gunShooting.ReturnBulletToPool(this.gameObject);
        }
    }

    private void OnEnable()
    {
        lifetime = BulletTime;
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void ReturnToPool()
    {
        if (gunShooting != null)
        {
            gunShooting.ReturnBulletToPool(this.gameObject);
        }
    }
}
