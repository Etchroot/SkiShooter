using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    public IObjectPool<Bullet> Pool { get; set; }

    [SerializeField] private float BulletSpeed = 50f;
    [SerializeField] private float BulletTime = 3f;

    private float lifetime;
    private Vector3 moveDirection;

    public void SetDirection(Vector3 direction)  // 🔥 외부에서 방향을 설정할 수 있도록 추가
    {
        moveDirection = direction.normalized;  // 방향을 정규화
    }

    void OnEnable()
    {
        lifetime = BulletTime;
    }

    void Update()
    {
        // 수명 감소 및 반환
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            //ReturnToPool();
            ObjectPoolManager.Instance.ReturnToPool("Bullet",this.gameObject);
        }
    }

    void FixedUpdate()
    {
        RaycastHit hit;

        transform.position += moveDirection * BulletSpeed * Time.fixedDeltaTime;

        // 디버깅용 Ray 그리기
        Debug.DrawRay(transform.position, moveDirection * (BulletSpeed * Time.fixedDeltaTime), Color.red, 0.1f);

        // 총알 이동 거리 내에서 충돌 감지
        if (Physics.Raycast(transform.position, moveDirection, out hit, BulletSpeed * Time.fixedDeltaTime))
        {
            if (hit.collider.CompareTag("BARREL") || hit.collider.CompareTag("ENEMY") || hit.collider.CompareTag("OBSTACLE"))
            {
                IDamageable damageable = hit.collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage();
                    //ReturnToPool();
                    ObjectPoolManager.Instance.ReturnToPool("Bullet", this.gameObject);

                }
            }
        }
        
    }

}
