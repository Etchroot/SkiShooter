using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    public IObjectPool<Bullet> Pool { get; set; }

    [SerializeField] private float BulletSpeed = 50f;
    [SerializeField] private float BulletTime = 3f;

    private float lifetime;
    private Vector3 moveDirection;

    void OnEnable()
    {
        lifetime = BulletTime;
        moveDirection = transform.forward; // 초기 방향 설정
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

        transform.Translate(moveDirection * BulletSpeed * Time.fixedDeltaTime);

        // 디버깅용 Ray 그리기
        //Debug.DrawRay(transform.position, moveDirection * (BulletSpeed * Time.fixedDeltaTime), Color.red, 0.1f);

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

    //private void ReturnToPool()
    //{
    //    if (Pool != null)
    //    {
    //        Pool.Release(this);
    //        gameObject.SetActive(false);
    //    }
    //    else
    //    {
    //        Debug.LogError("Bullet의 Pool이 설정되지 않았습니다! 총알을 삭제합니다.");
    //        Destroy(gameObject); // 풀을 찾지 못하면 그냥 삭제
    //    }
    //}

}
