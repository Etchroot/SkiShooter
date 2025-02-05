using UnityEngine;
using UnityEngine.Pool;
using System.Collections;

public class EnemyDrone : MonoBehaviour, IDamageable
{
    public Transform player; // 플레이어 Transform
    public Transform target; // 목적지 Transform

    public float moveSpeed = 17f; // 이동 속도
    public float attackRange = 2f; // 공격 범위
    public float attackCooldown = 2f; // 공격 쿨다운
    public float rotationSpeed = 5f; // 회전 속도

    private bool isOnCooldown = false;
    private bool isDead = false;
    private bool hasReachedTarget = false;

    [SerializeField] private GameObject AttackEffect;
    [SerializeField] private AudioClip AttackSound;
    [SerializeField] private GameObject DieEffect;
    [SerializeField] private AudioClip DieSound;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject firePoint;

    private ParticleSystem ps;

    public IObjectPool<GameObject> pool; // 오브젝트 풀

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    public void SetPool(IObjectPool<GameObject> objectPool)
    {
        pool = objectPool;
    }

    void Start()
    {
        
        ObjectPoolManager.Instance.CreatePool("EnemyDroneAttackEffect", AttackEffect, 1, 3);
        ObjectPoolManager.Instance.CreatePool("EnemyDroneDieEffect", DieEffect, 1, 3);
    }

    void OnEnable()
    {
        isDead = false;
        hasReachedTarget = false;
        gameObject.SetActive(true);
    }

    void Update()
    {
        if (isDead) return;

        MoveTowardsTarget();

        if (hasReachedTarget)
        {
            FacePlayerAndAttack();
        }
    }

    void MoveTowardsTarget()
    {
        if (!hasReachedTarget)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            hasReachedTarget = distanceToTarget <= attackRange;
        }
        else
        {
            transform.position = target.position;
        }
    }

    void FacePlayerAndAttack()
    {
        if (!isOnCooldown)
        {
            StartCoroutine(Attack());
        }

        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    IEnumerator Attack()
    {
        if (isDead) yield break;

        isOnCooldown = true;
        yield return new WaitForSeconds(attackCooldown);

        if (isDead) yield break;

        // 효과 객체를 풀에서 가져옵니다.
        GameObject attack = ObjectPoolManager.Instance.GetFromPool("EnemyDroneAttackEffect", firePoint.transform.position, Quaternion.identity);
        StartCoroutine(UpdateAttackEffectPosition(attack));
        audioSource.PlayOneShot(AttackSound);

        yield return new WaitForSeconds(0.2f);
        // 효과가 끝난 후 반환합니다.
        ObjectPoolManager.Instance.ReturnToPool("EnemyDroneAttackEffect", attack);

        Player_New.Instance.TakeDamage();

        isOnCooldown = false;
    }

    IEnumerator UpdateAttackEffectPosition(GameObject effect)
    {
        while (effect != null)
        {
            effect.transform.position = player.position; // 플레이어를 향하게 설정
            yield return null;
        }
    }

    IEnumerator Die()
    {
        isDead = true;
        audioSource.PlayOneShot(DieSound);

        GameObject dieEffect = ObjectPoolManager.Instance.GetFromPool("EnemyDroneDieEffect", transform.position, Quaternion.identity);
        if (dieEffect != null)
        {
            ParticleSystem ps = dieEffect.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Clear();
                ps.Play();
                yield return new WaitForSeconds(ps.main.duration); // 파티클이 끝날 때까지 대기
                ObjectPoolManager.Instance.ReturnToPool("EnemyDroneDieEffect", dieEffect);
            }
        }

        if (pool != null)
        {
            ObjectPoolManager.Instance.ReturnToPool("drone", this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void TakeDamage()
    {
        if (!isDead)
        {
            StartCoroutine(Die());
        }
    }
}
