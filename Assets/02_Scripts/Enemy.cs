using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    /*
     * 적군의 행동
     * 플레이어와 일정거리를 두며 따라다님
     * 5초에 한번씩 적군 공격
     * 
     * 
     */

    public enum State
    {
        IDLE, MOVE, ATTACK, HIT, DIE
    }

    public Transform target;    // 목표
    public float speed = 4f;    // 이동속도
    public float attackDist = 5f; // 공격사정거리
    public State state = State.IDLE; // 몬스터 현재 상태
    public bool isDie = false;  // 몬스터 사망 여부
    int hp = 3;

    private NavMeshAgent agent;
    //private Animator anim;

    void Start()
    {
        // NavMeshAgent 컴포넌트 할당
        agent = GetComponent<NavMeshAgent>();

        // Animator 컴포넌트 할당
        //anim = GetComponentInChildren<Animator>();

        // 몬스터의 상태를 체크하는 코루틴 함수 호출
        StartCoroutine(CheckMonsterState());

        // 상태에 따라 몬스터의 행동을 수행하는 코루틴 함수 호출
        StartCoroutine(MonsterAction());

    }

    void Update()
    {

    }

    IEnumerator CheckMonsterState()
    {
        while (!isDie)
        {
            //대기
            yield return new WaitForSeconds(0.3f);

            //거리측정
            float distance = Vector3.Distance(target.position, this.transform.position);


            if (distance > attackDist)
            {
                // 목표를 향해 이동
                state = State.MOVE;
            }
            else
            {
                // 목표에 도달 시 공격
                state = State.ATTACK;
            }


        }
    }

    IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            switch (state)
            {
                //  대기
                case State.IDLE:
                    agent.isStopped = true;
                    //anim.SetTrigger("IDLE");
                    break;

                // 이동
                case State.MOVE:
                    agent.SetDestination(target.position);
                    agent.isStopped = false;
                    //anim.SetTrigger("MOVE");
                    break;

                //공격
                case State.ATTACK:
                    //anim.SetTrigger("ATTACK");
                    break;

                //피격
                case State.HIT:
                    agent.isStopped = true;
                    //anim.SetTrigger("HIT");
                    break;

                //사망
                case State.DIE:
                    //anim.SetTrigger("DIE");
                    break;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("BULLET"))
        {
            Destroy(collision.gameObject);

            //anim.SetTrigger("HIT");
        }
    }



    void Attack()
    {

    }
}
