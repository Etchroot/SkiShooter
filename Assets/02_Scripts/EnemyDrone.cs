using System.Collections;
using UnityEngine;

public class EnemyDrone : MonoBehaviour, IDamageable
{
    public Transform player; // 플레이어의 Transform
    public float speed = 5f; // 드론 이동 속도
    public float attackRange = 2f; // 공격 범위
    public float attackCooldown = 2f; // 공격 쿨다운
    public float rotationSpeed = 5f; // 회전 속도

    private bool isOnCooldown = false; // 쿨다운 여부
    private bool isDead = false; // 드론 사망 여부

    [SerializeField] private GameObject explosionEffectPrefab; // 폭발 효과
    [SerializeField] private AudioClip explosionSound; // 폭발 소리
    [SerializeField] private AudioSource audioSource; // 오디오 소스

    // 드론이 파괴될 때 호출되는 이벤트
    public System.Action onDestroyed;

    void Update()
    {
        if (isDead) return;

        // 플레이어를 향해 이동
        MoveTowardsPlayer();

        // 플레이어와의 거리 확인 후 공격
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange && !isOnCooldown)
        {
            StartCoroutine(Attack());
        }
    }

    void MoveTowardsPlayer()
    {
        // 플레이어를 바라보도록 회전
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        // 플레이어를 향해 이동
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    IEnumerator Attack()
    {
        isOnCooldown = true;

        // 공격 실행 (폭발 예제)
        Explode();

        // 쿨다운 대기
        yield return new WaitForSeconds(attackCooldown);
        isOnCooldown = false;
    }

    void Explode()
    {
        // 폭발 효과 생성
        GameObject explosion = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, 2f); // 폭발 효과 제거

        // 폭발 사운드 재생
        if (explosionSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(explosionSound);
        }

        // 플레이어에게 데미지
        Player_New.Instance.TakeDamage();

        // 드론 사망 처리
        StartCoroutine(Die());
    }

    public IEnumerator Die()
    {
        isDead = true; // 사망 상태
        onDestroyed?.Invoke(); // 스폰 매니저에게 파괴 알림
        yield return new WaitForSeconds(0.5f); // 효과 시간 대기
        Destroy(gameObject); // 드론 제거
    }

    public void TakeDamage()
    {
        if (!isDead)
        {
            StartCoroutine(Die());
        }
    }
}
