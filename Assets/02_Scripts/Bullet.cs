using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float BulletSpeed = 50f;
    [SerializeField] private float BulletTime = 3f;

    private float lifetime;
    
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
            //오브젝트풀 반환
            ObjectPoolManager.ReturnObject(this.gameObject, EPoolObjectType.Bullet);
        }

        transform.position += transform.forward * BulletSpeed * Time.deltaTime;
        //transform.Translate(transform.forward * BulletSpeed * Time.deltaTime);

        // 디버깅용 Ray 그리기
        //Debug.DrawRay(transform.position, moveDirection * (BulletSpeed * Time.DeltaTime), Color.red, 0.1f);

        // 총알 이동 거리 내에서 충돌 감지
        if (Physics.Raycast(transform.position, transform.forward, out var hit, 1.0f))
        {
            if (hit.collider.CompareTag("BARREL") || hit.collider.CompareTag("ENEMY") || hit.collider.CompareTag("OBSTACLE"))
            {
                IDamageable damageable = hit.collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage();

                    //오브젝트풀 반환
                    ObjectPoolManager.ReturnObject(this.gameObject, EPoolObjectType.Bullet);

                }
            }
        }

    }

}
