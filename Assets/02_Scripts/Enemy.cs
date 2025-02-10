using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable
{
    public Transform player; // 플레이어의 Transform
    //public GameObject projectilePrefab; // 발사체 프리팹
    public Transform firePoint; // 발사 위치
    public float attackRange = 10f; // 공격 범위
    public float attackCooldown = 2f; // 공격 대기 시간
    public float rotationSpeed = 10f; // 회전 시간
    private bool isOnCooldown = false; // 공격 쿨다운 여부
    public bool isDead = false; // 적이 죽는 중인지 체크

    [SerializeField] private AudioClip gunShotAudio; // 총 발사 소리 클립
    [SerializeField] private AudioClip[] getShotAudio; // 총 맞는 소리 클립
    [SerializeField] private AudioSource audioSource; // 오디오 소스
    [SerializeField] private AudioSource getShotAudioSource; // 총 맞는 소리 오디오 소스

    private GameObject[] effects;
    [SerializeField] GameObject gunShotEffectPF1; // 총 발사시 나오는 이펙트 프리펩
    [SerializeField] GameObject gunShotEffectPF2;
    [SerializeField] GameObject gunShotEffectPF3;
    private float effectDuration = 0.1f; // 각 이펙트 지속 시간
    private float interval = 0.1f; // 각 이펙트 사이 간격

    private Animator anim;

    private void Start()
    {
        // Animator 컴포넌트 할당
        anim = GetComponentInChildren<Animator>();

        anim.SetTrigger("IDLE");

        //초기화
        effects = new GameObject[] { gunShotEffectPF1, gunShotEffectPF2, gunShotEffectPF3 };
    }

    void Update()
    {
        //사망여부
        if (isDead) return;

        // 플레이어와 적 사이의 거리 계산
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        LookAtPlayer(); // 플레이어를 바라봄

        // 플레이어가 공격 범위 안에 있는지 확인
        if (distanceToPlayer <= attackRange)
        {
            //LookAtPlayer(); // 플레이어를 바라봄

            //공격
            if (!isOnCooldown)
            {
                StartCoroutine(Attack());
            }
        }

    }

    void LookAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // y축 회전을 고정
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        // Local 기준 Y축에 추가 각도 적용
        Quaternion adjustedRotation = targetRotation * Quaternion.Euler(0, 36, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, adjustedRotation, Time.deltaTime * rotationSpeed);
    }

    IEnumerator Attack()
    {
        // 죽었다면 공격 중단
        if (isDead) yield break;

        isOnCooldown = true; // 쿨다운 활성화

        // 대기
        yield return new WaitForSeconds(attackCooldown);

        // 죽었다면 공격 중단
        if (isDead) yield break;

        StartCoroutine(PlayGunEffects());
        audioSource.PlayOneShot(gunShotAudio); // 총 소리

        Player_New.Instance.TakeDamage();

        anim.SetTrigger("SHOOT");

        // 공격 행동 (발사체 생성)
        //GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        isOnCooldown = false; // 쿨다운 해제
    }

    private IEnumerator PlayGunEffects()
    {
        foreach (GameObject effectPrefab in effects)
        {
            // 적이 죽었는지 확인하여 중단
            if (isDead) yield break;

            // 이펙트 생성
            GameObject effect = Instantiate(effectPrefab, firePoint.position, firePoint.rotation);

            // 일정 시간 후 이펙트 제거
            Destroy(effect, effectDuration);

            // 다음 이펙트를 위해 대기
            yield return new WaitForSeconds(interval);
        }
    }

    public void TakeDamage()
    {
        if (isDead) return;
        StartCoroutine(Die());
    }

    public IEnumerator Die()
    {
        isDead = true; // 죽는 중으로 설정

        // 랜덤으로 죽는 소리 클립 재생
        int randomIndex = Random.Range(0, getShotAudio.Length - 1);
        AudioClip selectedClip = getShotAudio[randomIndex];
        getShotAudioSource.PlayOneShot(selectedClip, 1.5f); // 볼륨 증가

        anim.SetTrigger("DIE");
        this.gameObject.tag = "Untagged";
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }

    public IEnumerator DiebyExplosion()
    {
        isDead = true; // 죽는 중으로 설정

        // 애니메이션 제거
        // Animator animator = GetComponentInChildren<Animator>();
        // if (animator != null)
        // {
        //     Debug.Log("체크");

        //     animator.enabled = false;
        // }

        // 빌헬름의 비명 재생
        if (getShotAudio.Length > 0)
        {
            AudioClip lastClip = getShotAudio[getShotAudio.Length - 1];
            getShotAudioSource.PlayOneShot(lastClip, 0.8f); // 볼륨 감소
        }

        this.gameObject.tag = "Untagged";
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, attackRange);
    //}

}
