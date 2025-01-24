using System.Collections;
using UnityEngine;

public class EnemyDrone : MonoBehaviour, IDamageable
{
    public Transform player; // 플레이어의 Transform
    public Transform target; // 목적지 Transform

    public float moveSpeed = 17f; // 이동 속도
    public float attackRange = 2f; // 공격 범위 -> 드론이 위치가 고정되는 범위
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
    [SerializeField] private GameObject firePoint; // 이펙트 위치

    void Update()
    {
        if (isDead) return;

        // 플레이어 위치를 계속 추적하며 이동
        MoveTowardsTarget();

        // 공격 범위 내에서만 회전과 공격 수행 -> 도착지점 안착했으면 공격
        if (hasReachedTarget)
        {
            FacePlayerAndAttack();
        }
    }

    void MoveTowardsTarget()
    {
        // 타겟 갱신: 타겟을 항상 플레이어의 현재 위치로 설정
        //target.position = player.position;

        if (!hasReachedTarget)
        {
            // 타겟을 향해 이동
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            // 타겟 지점에 도착했는지 확인
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            hasReachedTarget = distanceToTarget <= attackRange; // 도착 여부 판정
        }
        else
        {
            transform.position = target.position;
        }



    }

    void FacePlayerAndAttack()
    {
        // 플레이어와의 거리 확인 -> 어차피 사거리 안으로 들어와야 공격
        // float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        // if (distanceToPlayer > attackRange) return; // 공격 범위 밖이면 함수 종료

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
        yield return new WaitForSeconds(attackCooldown);

        if (isDead) yield break;
        // 공격 효과 실행
        GameObject attack = Instantiate(AttackEffect, firePoint.transform.position, firePoint.transform.rotation);
        attack.transform.LookAt(Player_New.Instance.transform); // 이펙트가 플레이어 바라보게
        StartCoroutine(AttackEffectPosion(attack));
        audioSource.PlayOneShot(AttackSound);
        Destroy(attack, 0.5f);
        //Debug.Log("드론 공격중");

        Player_New.Instance.TakeDamage();

        isOnCooldown = false; // 쿨다운 해제
    }

    IEnumerator AttackEffectPosion(GameObject effect) // 이펙트만 덩그러니 남아있는것 방지
    {
        while (effect != null) // 이펙트가 파과되지 않는 동안 실행
        {
            effect.transform.position = target.position;
            yield return null; //한프레임 대기 후 반복
        }
    }

    public IEnumerator Die()
    {
        isDead = true;

        audioSource.PlayOneShot(DieSound);
        GameObject dieEffect = Instantiate(DieEffect, transform.position, Quaternion.identity);
        Destroy(dieEffect, 2f);

        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    public void TakeDamage()
    {
        if (!isDead)
        {
            StartCoroutine(Die());
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
