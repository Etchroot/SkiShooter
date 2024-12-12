using UnityEngine;

public class BombItem : Item
{
    [Header("폭탄 세팅")]

    [SerializeField]
    private float explosionRadius = 5f; // 폭발 반경

    // private void OnDestroy()
    // {
    //     ActivateEffect();
    // }
    // 이미 해당 메소드 item 스크립트에 있음

    public override void ActivateEffect()
    {
        Debug.Log("폭탄 아이템 사용");

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("ENEMY"))
            {
                Destroy(collider.gameObject);
            }
            else if (collider.CompareTag("OBSTACLE"))
            {
                Destroy(collider.gameObject);
            }
        }
    }

    // 디버깅용 시각화
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
