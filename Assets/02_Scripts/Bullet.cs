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
            //적군
            if (hit.collider.CompareTag("ENEMY"))
            {
                //데미지 주는 로직
                hit.collider.GetComponent<Enemy>().TakeDamage(); //데미지 받는 함수 TakeDamage() 통일

                // 적과 충돌 시 처리
                gunShooting.ReturnBulletToPool(this.gameObject);
                
            }

            //장애물
            if (hit.collider.CompareTag("OBSTACLE"))
            {
                //데미지 주는 로직
                hit.collider.GetComponent<Obstacle>().TakeDamage(); //데미지 받는 함수 TakeDamage() 통일

                // 적과 충돌 시 처리
                gunShooting.ReturnBulletToPool(this.gameObject);
                
            }

            //가스통
            if (hit.collider.CompareTag("BARREL"))
            {
                //데미지 주는 로직
                hit.collider.GetComponent<BarrelExplosion>().TakeDamage(); //데미지 받는 함수 TakeDamage() 통일

                // 적과 충돌 시 처리
                gunShooting.ReturnBulletToPool(this.gameObject);

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
