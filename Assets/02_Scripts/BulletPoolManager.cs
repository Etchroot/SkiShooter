using System.Collections.Generic;
using UnityEngine;

public class BulletPoolManager : MonoBehaviour
{
    public static BulletPoolManager Instance { get; private set; }

    public Bullet bulletPrefab;
    [SerializeField] private int poolSize = 20;

    private Queue<Bullet> bulletPool = new Queue<Bullet>();

    void Awake()
    {
        // 싱글턴 패턴 구현
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);  // 이미 인스턴스가 있으면 삭제
        }
        else
        {
            Instance = this;
        }

        // 풀 초기화
        for (int i = 0; i < poolSize; i++)
        {
            Bullet bullet = Instantiate(bulletPrefab);
            bullet.gameObject.SetActive(false); // 총알 비활성화
            bulletPool.Enqueue(bullet);
        }
    }
    
    // Pool에서 총알 가져오기
    public Bullet GetBullet(Vector3 position, Quaternion rotation)
    {
        if (bulletPool.Count > 0)
        {
            Bullet bullet = bulletPool.Dequeue();
            bullet.transform.position = position;
            bullet.transform.rotation = rotation;
            return bullet;
        }
        else
        {
            // 풀에 남은 총알이 없으면 새로 생성
            Bullet bullet = Instantiate(bulletPrefab, position, rotation);
            bullet.gameObject.SetActive(false);
            return bullet;
        }
    }

    // Pool로 총알 반환
    public void ReturnBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        bullet.transform.position = Vector3.zero; // 위치 초기화 (필요한 경우 다른 속성도 초기화)
        bulletPool.Enqueue(bullet);
    }

}
