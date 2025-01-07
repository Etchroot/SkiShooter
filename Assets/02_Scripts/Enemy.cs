using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum State
    {
        IDLE, MOVE, ATTACK, HIT, DIE
    }

    
    
    public Transform target;           // 플레이어 목표
    public float speed = 4f;           // 이동 속도
    public float attackDist = 5f;      // 공격 사정거리
    public float attackCooldown = 5f; // 공격 쿨타임
    public State state = State.IDLE;   // 적 상태

    public bool isDie = false;         // 적 사망 여부
    private int hp = 3;                // 적 체력

    public GameObject BulletPrefab;    // 총알 프리팹
    public Transform FirePoint;        // 총구 위치

    //private Animator anim;             // 애니메이터
    private float lastAttackTime = 0f; // 마지막 공격 시간

    public float detectionRange = 10f; // 적군이 플레이어를 탐지할 수 있는 거리 (이 범위 내에서 이동 시작)

    void Start()
    {

        //anim = GetComponentInChildren<Animator>();

        // 적 상태 체크 및 행동 코루틴 시작
        StartCoroutine(CheckEnemyState());
        StartCoroutine(EnemyAction());
    }

    IEnumerator CheckEnemyState()
    {
        while (!isDie)
        {
            yield return new WaitForSeconds(0.3f);

            // 플레이어와의 거리 측정
            float distance = Vector3.Distance(target.position, transform.position);

            if (distance > attackDist && distance < detectionRange)
            {
                // 사정거리 밖이지만 탐지 범위 안: 이동 상태
                state = State.MOVE;
            }
            else if (distance <= attackDist)
            {
                // 공격 사정거리 안: 공격 상태
                state = State.ATTACK;
            }
            else
            {
                // 탐지 범위 밖: 대기 상태
                state = State.IDLE;
            }
        }
    }


    IEnumerator EnemyAction()
    {
        while (!isDie)
        {
            switch (state)
            {
                case State.IDLE:
                    //anim.SetTrigger("IDLE");
                    break;

                case State.MOVE:
                    //anim.SetTrigger("MOVE");
                    break;

                case State.ATTACK:
                    if (Time.time - lastAttackTime >= attackCooldown)
                    {
                        Attack(); // 공격 실행
                        lastAttackTime = Time.time; // 공격 쿨타임 초기화
                    }
                    //anim.SetTrigger("ATTACK");
                    break;

                case State.HIT:
                    //anim.SetTrigger("HIT");
                    break;

                case State.DIE:
                    //anim.SetTrigger("DIE");
                    Destroy(gameObject, 2f); // 2초 후 오브젝트 삭제
                    break;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("BULLET"))
        {
            Destroy(collision.gameObject); // 총알 파괴
            TakeDamage(1); // 데미지 1 추가
        }
    }

    void TakeDamage(int damage)
    {
        if (isDie) return;

        hp -= damage;
        state = State.HIT;

        if (hp <= 0)
        {
            isDie = true;
            state = State.DIE;
        }
    }

    void Attack()
    {
        // 총알 생성
        if (BulletPrefab != null && FirePoint != null)
        {
            Instantiate(BulletPrefab, FirePoint.position, FirePoint.rotation);
        }
    }

}
