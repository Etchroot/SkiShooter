using System;
using System.Collections;
using UnityEngine;

public class TransparentWall : MonoBehaviour, IDamageable
{
    [SerializeField] private bool Destructible = true; // 파괴 가능 여부
    [SerializeField] private Vector3 boxSize = new Vector3(2f, 2f, 2f);

    private void Update()
    {
        Check();
    }

    private void Check()
    {
        // 반경 내의 모든 Colldier 가져오기
        Collider[] hitColliders = Physics.OverlapBox(transform.position, boxSize / 2);

        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("PLAYER"))
            {
                Debug.Log("플레이어 접근 확인.");
                Destroy(gameObject);
                break;
            }
        }
    }
    private void OnDrawGizmos()
    {
        // 디버깅용 박스 표시
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }

    public void TakeDamage()
    {
        if (Destructible) //파괴 가능한 것만 파괴가능
        {
            //아무것도 하지 않음
        }
    }


}
