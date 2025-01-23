using System.Collections;
using UnityEngine;

public class EnemyDrone : MonoBehaviour, IDamageable
{
    public Transform player; // 플레이어의 Transform
    public Transform target; // 목적지 Transform

    public float moveSpeed = 17f; // 이동 속도
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

        // 플레이어 위치를 계속 추적하며 이동
        MoveTowardsTarget();

        // 공격 범위 내에서만 회전과 공격 수행
        if (hasReachedTarget)
        {
            FacePlayerAndAttack();
        }
    }

    void MoveTowardsTarget()
    {
        // 타겟 갱신: 타겟을 항상 플레이어의 현재 위치로 설정
        target.position = player.position;

        // 타겟을 향해 이동
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        // 타겟 지점에 도착했는지 확인
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        hasReachedTarget = distanceToTarget <= attackRange; // 도착 여부 판정
    }

    void FacePlayerAndAttack()
    {
        // 플레이어와의 거리 확인
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > attackRange) return; // 공격 범위 밖이면 함수 종료

        // 플레이어를 바라보도록 회전
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        // 공격 실행
        if (!isOnCooldown)
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
