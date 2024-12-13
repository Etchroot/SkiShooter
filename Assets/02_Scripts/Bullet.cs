using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class Bullet : MonoBehaviour
{
    // 총알 스크립트
    // 생성된 방향의 앞으로 날아가며 물체와 부딫히거나 5초후 Destroy
    
    [SerializeField] private float BulletSpeed = 5f;
        
    void Start()
    {
        //생성되고 5초후 삭제
        StartCoroutine(Destroy());
    }

    void Update()
    {
        //이동
        transform.Translate(Vector3.forward * BulletSpeed * Time.deltaTime);
    }

    //다른 게임 오브젝트와 닿으면 삭제
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ENEMY"))
        {
            Destroy(other.gameObject);
        }
        else
        {
            //바닥이나 다른곳 부딪혔을떄
            Destroy(this.gameObject);
        }
        
    }

    IEnumerator Destroy()
    {
        //생성되고 5초 지나면 제거
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }

}
