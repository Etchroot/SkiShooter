using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float BulletSpeed = 5f;
    [SerializeField] private float BulletTime = 3f;
    private GunShooting gunShooting; // GunShooting 참조

    public void Initialize(GunShooting gunShootingInstance)
    {
        gunShooting = gunShootingInstance;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * BulletSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ENEMY"))
        {
            // 적 처리 로직 (예: 데미지)
        }

        // 풀에 반환
        gunShooting.ReturnBulletToPool(this.gameObject);
    }

    private void OnEnable()
    {
        // 일정 시간이 지나면 풀로 반환
        Invoke(nameof(ReturnToPool), BulletTime);
    }

    private void OnDisable()
    {
        // 오브젝트가 비활성화될 때 Invoke 취소
        CancelInvoke();
    }

    private void ReturnToPool()
    {
        if (gunShooting != null)
        {
            gunShooting.ReturnBulletToPool(this.gameObject);
        }
    }
}
