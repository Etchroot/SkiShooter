using System.Collections;
using UnityEngine;

public class Obstacle : MonoBehaviour, IDamageable
{
    [SerializeField] private bool Destructible = false; // 파괴 가능 여부
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private AudioSource audioSource;

    private ParticleSystem ps;

    void Awake()
    {

        ps = GetComponent<ParticleSystem>();
    }

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
            MeshRenderer meshRendererP = GetComponent<MeshRenderer>();

            if (meshRendererP != null && meshRendererP.enabled)
            {
                meshRendererP.enabled = false;
            }
            GetComponent<Collider>().enabled = false;

            // 하위 객체의 MeshRenderer 비활성화
            foreach (Transform child in transform)
            {
                MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    meshRenderer.enabled = false;
                }
            }

            StartCoroutine(iceBreake());

        }
        else
        {
            return;
        }
    }

    IEnumerator iceBreake()
    {
        GameObject iceEffect = ObjectPoolManager.GetObject(EPoolObjectType.ICE_BRAKE);
        iceEffect.transform.position = gameObject.transform.position;

        if (iceEffect != null)
        {
            ParticleSystem ps = iceEffect.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Clear();
                ps.Play();
                yield return new WaitForSeconds(ps.main.duration); // 파티클이 끝날 때까지 대기
                // 오브젝트풀 리턴
                ObjectPoolManager.ReturnObject(iceEffect, EPoolObjectType.ICE_BRAKE);
            }
        }
        Destroy(gameObject); // 장애물 삭제
    }
}
