using System.Collections;
using UnityEngine;

public class Obstacle : MonoBehaviour, IDamageable
{
    [SerializeField] private bool Destructible = false; // 파괴 가능 여부
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private AudioSource audioSource;

    // 오브젝트 파괴 처리
    public void TakeDamage()
    {
        if (Destructible) //파괴 가능한 것만 파괴가능
        {
            if (audioSource != null && audioClip != null)
            {
                audioSource.PlayOneShot(audioClip); // 효과음 재생
            }
            // if (audioSource == null)
            // {
            //     Debug.LogError("AudioSource 가 null 입니다");
            // }
            // if (audioClip == null)
            // {
            //     Debug.LogError("AudioClip 이 null 입니다.");
            // }
            // else
            // {
            //     Debug.LogError("알 수 없는 오류");
            // }

            // MeshRenderer와 Collider 비활성화
            // 하위 객체의 MeshRenderer 비활성화
            foreach (Transform child in transform)
            {
                MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    meshRenderer.enabled = false;
                }
            }

            GetComponent<Collider>().enabled = false;


            StartCoroutine(Delay());

        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject); // 장애물 삭제
    }

    // 플레이어와 충돌 처리
    //private void OnTriggerEnter(Collider other)
    //{
    //    //총알에 맞으면
    //    if (other.CompareTag("BULLET"))
    //    {
    //        destruction();
    //    }
    //}
}
