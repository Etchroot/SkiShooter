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

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        layerMask = LayerMask.GetMask("ENEMY", "OBSTACLE", "BARREL", "ENEMYDRONE", "LAND");
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

                EPoolObjectType re_Type = hit.collider.CompareTag("ENEMY") ? EPoolObjectType.HitBloodEffect : EPoolObjectType.HitEffect;
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

                        ReturnHitEffect(ps.main.duration);

                        ObjectPoolManager.ReturnObject(hitEffect, re_Type);
                    }
                    else
                    {
                        ObjectPoolManager.ReturnObject(hitEffect, re_Type);
                    }
                }
            }
            ObjectPoolManager.ReturnObject(this.gameObject, EPoolObjectType.Bullet);
        }
    }

    private IEnumerator ReturnHitEffect(float delay)
    {
        yield return new WaitForSeconds(delay);
    }



}
