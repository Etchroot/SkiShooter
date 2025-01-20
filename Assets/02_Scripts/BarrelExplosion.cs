using UnityEngine;
using System;
using System.Collections;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class BarrelExplosion : MonoBehaviour
{
    [SerializeField] private float explosionForce = 300f; // 폭발력
    [SerializeField] private float explosionRadius = 10f; // 폭발 반경
    [SerializeField] private float upwardModifier = 1f; // 위쪽으로 밀어내는 효과 (0이면 순수한 방향)

    private bool hasExploded = false; // 폭발한 상태인지 확인하는 플래그
    public GameObject explosionEffect = null; // 폭발 이펙트 프리펩

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BULLET"))
        {
            Explode();
        }
    }

    public void Explode()
    {
        if (hasExploded) return; // 이미 폭발했다면 중복 실행 방지
        hasExploded = true;

        // 폭발 이펙트 생성
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        else
        {
            Debug.LogError("폭발 프리펩이 없음");
        }

        // 폭발 반경 내의 모든 Collider를 탐색
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();

            if (rb != null) // Rigidbody가 있는 오브젝트만 폭발 영향 받음
            {
                // Rigidbody에 폭발력 적용
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardModifier, ForceMode.Impulse);
            }

            // 적에게 추가적인 데미지 부여
            Enemy enemy = nearbyObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (!enemy.isDead)
                {
                    StartCoroutine(enemy.Die()); // Die() 코루틴 실행
                }
            }

        }

        // 가스통의 시각적/물리적 요소 비활성화
        HideBarrel();

        // x초 후 가스통 파괴
        StartCoroutine(DestroyAfterDelay(10f));
    }

    private void HideBarrel()
    {
        // 메쉬 랜더러와 콜라이더 비활성화
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.enabled = false;
        }

        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject); // delay후 가스통 삭제
    }


    // 폭발 반경 시각적으로 확인하기 위한 기즈모 설정
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(BarrelExplosion))]
    public class GasCanisterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // 기본 Inspector UI 표시
            DrawDefaultInspector();

            // 타겟 스크립트 참조
            BarrelExplosion barrelExplosion = (BarrelExplosion)target;

            // 버튼 추가
            if (GUILayout.Button("폭발"))
            {
                barrelExplosion.Explode(); // 버튼 클릭 시 Explode() 실행
            }
        }
    }
#endif
}
