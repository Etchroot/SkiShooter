using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float BulletSpeed = 50f;
    [SerializeField] private float BulletTime = 3f;

    private float lifetime;
    private MeshRenderer meshRenderer;

    private int layerMask;

    private bool hashit =false;

    private int enemyLayer; // ENEMY Layer 저장 변수


    void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        layerMask = LayerMask.GetMask("ENEMY", "OBSTACLE", "BARREL", "ENEMYDRONE", "LAND");
        enemyLayer = LayerMask.NameToLayer("ENEMY");  // ENEMY Layer의 숫자를 가져오기
    }


    void OnEnable()
    {
        lifetime = BulletTime;
        hashit = false;
    }

    void OnDisable()
    {
        if (meshRenderer != null)
            meshRenderer.enabled = true;
    }

    void Update()
    {
        if (hashit) return;

        lifetime -= Time.deltaTime;
        if (lifetime <= 0 && !hashit)
        {
            ObjectPoolManager.ReturnObject(this.gameObject, EPoolObjectType.Bullet);
        }

        transform.position += transform.forward * BulletSpeed * Time.deltaTime;

        if (Physics.Raycast(transform.position, transform.forward, out var hit, BulletSpeed * Time.deltaTime, layerMask))
        {
            hashit = true;

            IDamageable damageable = hit.collider.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage();

                if (meshRenderer != null)
                    meshRenderer.enabled = false;

                EPoolObjectType re_Type = hit.collider.gameObject.layer == enemyLayer ?
                          EPoolObjectType.HitBloodEffect : EPoolObjectType.HitEffect;
                GameObject hitEffect = ObjectPoolManager.GetObject(re_Type);

                if (hitEffect != null)
                {
                    hitEffect.transform.position = hit.point;
                    hitEffect.transform.rotation = Quaternion.LookRotation(hit.normal);

                    ParticleSystem ps = hitEffect.GetComponent<ParticleSystem>();
                    if (ps != null)
                    {
                        ps.Clear();
                        ps.Play();

                        StartCoroutine(ReturnHitEffect(ps.main.duration, hitEffect, re_Type));
                    }
                    else
                    {
                        ObjectPoolManager.ReturnObject(hitEffect, re_Type);
                    }
                }
            }
            //ObjectPoolManager.ReturnObject(this.gameObject, EPoolObjectType.Bullet);
        }
    }

    private IEnumerator ReturnHitEffect(float delay,GameObject effect, EPoolObjectType re_Type)
    {
        //Debug.Log(re_Type);
        yield return new WaitForSeconds(delay);
        ObjectPoolManager.ReturnObject(effect, re_Type);
        ObjectPoolManager.ReturnObject(this.gameObject, EPoolObjectType.Bullet);
    }



}
