using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform
    //public GameObject projectilePrefab; // 발사체 프리팹
    public Transform firePoint; // 발사 위치
    public float attackRange = 10f; // 공격 범위
    public float attackCooldown = 2f; // 공격 대기 시간
    public float rotationSpeed = 10f; // 회전 시간
    private bool isOnCooldown = false; // 공격 쿨다운 여부
    public bool isDead = false; // 적이 죽는 중인지 체크

    private Animator anim;


    private void Start()
    {
        // Animator 컴포넌트 할당
        anim = GetComponentInChildren<Animator>();

        anim.SetTrigger("IDLE");
    }


    void Update()
    {
        // 플레이어와 적 사이의 거리 계산
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 플레이어가 공격 범위 안에 있는지 확인
        if (distanceToPlayer <= attackRange)
        {
            LookAtPlayer(); // 플레이어를 바라봄

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
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }


    IEnumerator Attack()
    {
        isOnCooldown = true; // 쿨다운 활성화

        // 대기
        yield return new WaitForSeconds(attackCooldown);

        Player_New.Instance.TakeDamage();

        anim.SetTrigger("SHOOT");

        // 공격 행동 (발사체 생성)
        //GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        isOnCooldown = false; // 쿨다운 해제
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (!isDead && collision.collider.CompareTag("BULLET"))
    //    {
    //        StartCoroutine(Die());
    //    }
    //}

    public void TakeDamage()
    {
        StartCoroutine(Die());
    }


    public IEnumerator Die()
    {
        isDead = true; // 죽는 중으로 설정
        anim.SetTrigger("DIE");
        this.gameObject.tag = "Untagged";
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }



}
