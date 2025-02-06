using UnityEngine;
using System.Collections;

public class EnemyDrone : MonoBehaviour, IDamageable
{
    public Transform player; // 플레이어 Transform
    public Transform target; // 목적지 Transform

    [SerializeField] private float moveSpeed = 17f; // 이동 속도
    [SerializeField] private float attackRange = 2f; // 공격 범위
    [SerializeField] private float attackCooldown = 2f; // 공격 쿨다운
    [SerializeField] private float rotationSpeed = 5f; // 회전 속도

    private bool isOnCooldown = false;
    private bool isDead = false;
    private bool hasReachedTarget = false;

    //[SerializeField] private GameObject AttackEffect;
    //[SerializeField] private GameObject DieEffect;
    [SerializeField] private AudioClip AttackSound;
    [SerializeField] private AudioClip DieSound;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Transform firePoint;

    private ParticleSystem ps;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
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

        // 공격이펙트 풀
        GameObject attack = ObjectPoolManager.GetObject(EPoolObjectType.EnemyDrone_Attack);
        attack.transform.position = this.firePoint.position;
        attack.transform.rotation = this.firePoint.rotation;
        audioSource.PlayOneShot(AttackSound);

        yield return new WaitForSeconds(0.2f);

        //오브젝트풀 리턴
        ObjectPoolManager.ReturnObject(attack, EPoolObjectType.EnemyDrone_Attack);

        Player_New.Instance.TakeDamage();

        isOnCooldown = false;
    }

    IEnumerator Die()
    {
        isDead = true;
        audioSource.PlayOneShot(DieSound);

        GameObject dieEffect = ObjectPoolManager.GetObject(EPoolObjectType.EnemyDrone_Die);
        dieEffect.transform.position = gameObject.transform.position;
        
        if (dieEffect != null)
        {
            ParticleSystem ps = dieEffect.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Clear();
                ps.Play();
                yield return new WaitForSeconds(ps.main.duration); // 파티클이 끝날 때까지 대기
                // 오브젝트풀 리턴
                ObjectPoolManager.ReturnObject(dieEffect, EPoolObjectType.EnemyDrone_Die);
            }
        }
        // 오브젝트풀 리턴
        ObjectPoolManager.ReturnObject(this.gameObject, EPoolObjectType.EnemyDrone);
    }


    public void TakeDamage()
    {
        if (!isDead)
        {
            StartCoroutine(Die());
        }
    }
}
