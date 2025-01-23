using System.Collections;
using UnityEngine;

public class EnemyDrone : MonoBehaviour, IDamageable
{
    public Transform player; // 플레이어의 Transform
    public Transform target; // 목적지 Transform

    public float moveSpeed = 5f; // 이동 속도
    public float attackRange = 2f; // 공격 범위
    public float attackCooldown = 2f; // 공격 쿨다운
    public float rotationSpeed = 5f; // 회전 속도

    private bool isOnCooldown = false; // 쿨다운 여부
    private bool isDead = false; // 드론 사망 여부
    private bool hasReachedTarget = false; // 타겟 도착 여부

    [SerializeField] private GameObject AttackEffect; // 공격 효과
    [SerializeField] private AudioClip AttackSound; // 공격 소리
    [SerializeField] private GameObject DieEffect; // 죽었을 때 효과
    [SerializeField] private AudioClip DieSound; // 폭발 소리
    [SerializeField] private AudioSource audioSource; // 오디오 소스

    void Update()
    {
        if (isDead) return;

        if (!hasReachedTarget)
        {
            // 타겟 지점으로 이동
            MoveTowardsTarget();
        }
        else
        {
            // 타겟 도착 후 플레이어를 바라보고 공격
            FacePlayerAndAttack();
        }
    }

    void MoveTowardsTarget()
    {
        // 타겟을 향해 이동
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        // 타겟 지점에 도착했는지 확인
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget <= 0.5f) // 도착 판정 거리
        {
            hasReachedTarget = true; // 타겟 도착
        }
    }

    void FacePlayerAndAttack()
    {
        // 플레이어를 바라보도록 회전
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        // 공격 범위 확인 및 공격
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange && !isOnCooldown)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        if (isDead) yield break;

        isOnCooldown = true; // 쿨다운 활성화

        // 공격 효과 실행
        GameObject attack = Instantiate(AttackEffect, transform.position, Quaternion.identity);
        audioSource.PlayOneShot(AttackSound);
        Destroy(attack, 2f);

        Player_New.Instance.TakeDamage();

        yield return new WaitForSeconds(attackCooldown);
        isOnCooldown = false; // 쿨다운 해제
    }

    public IEnumerator Die()
    {
        isDead = true;

        GameObject dieEffect = Instantiate(DieEffect, transform.position, Quaternion.identity);
        audioSource.PlayOneShot(DieSound);
        Destroy(dieEffect, 2f);

        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    public void TakeDamage()
    {
        if (!isDead)
        {
            StartCoroutine(Die());
        }
    }
}
