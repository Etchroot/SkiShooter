using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunShooting : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    public GameObject BulletPrefab; // 총알 프리팹
    public Transform FirePoint; // 총구 위치
    public GameObject handGun; // 총 프리펩

    public bool isLeft = true; // true: 왼쪽, false: 오른쪽

    [SerializeField] private float fireRate = 0.03f; // 발사 딜레이
    private bool canFire = true; // 발사 중복 방지

    public AudioSource source; // 오디오
    public AudioClip fireSound; // 발사 소리
    public AudioClip reloadingSound; // 재장전 중 소리
    public AudioClip reloadSound; // 재장전 완료 소리
    public AudioClip emptyGunSound; // 총알 없음 소리

    [SerializeField] private int maxBullet = 100; // 최대 총알 수
    public int currentBullet = 100; // 현재 총알 수

    private float previousGrips = 0; // Grips 0 > 1 > 0 확인용 변수

    [SerializeField] private float reloadTime = 2f; // 재장전 시간
    public bool isReloading = false; // 재장전 중인지 확인

    private Haptic haptics;
    public float hapticsAmplitude = 0.2f;
    public float hapticsDuration = 0.05f;

    // 오브젝트 풀링 관련 변수
    [SerializeField] private int poolSize = 30; // 풀 크기
    private Queue<GameObject> bulletPool; // 총알 풀

    //private int totalBulletsCreated = 0; //생성된 총알수 (각각 27발씩 생성됨)

    void Awake()
    {
        haptics = GetComponent<Haptic>();

        // 오브젝트 풀 초기화
        bulletPool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(BulletPrefab);
            bullet.SetActive(false); // 초기에는 비활성화
            bullet.GetComponent<Bullet>().Initialize(this); // GunShooting 참조 전달
            bulletPool.Enqueue(bullet);

            //totalBulletsCreated++; // 초기 생성 카운트 증가
        }
        //Debug.Log($"초기 총알 풀 생성 완료. 생성된 총알 수: {totalBulletsCreated}");
    }

    void Update()
    {
        float triggerPressed = 0;
        float gripPressed = 0;

        // 각각의 컨트롤러에 따라 트리거와 그립 버튼 상태 가져오기
        if (isLeft)
        {
            // 왼쪽
            triggerPressed = inputActions.actionMaps[2].actions[2].ReadValue<float>();
            gripPressed = inputActions.actionMaps[2].actions[0].ReadValue<float>();
        }
        else
        {
            // 오른쪽
            triggerPressed = inputActions.actionMaps[5].actions[2].ReadValue<float>();
            gripPressed = inputActions.actionMaps[5].actions[0].ReadValue<float>();
        }

        // 총기 발사
        if (triggerPressed == 1 && canFire && !isReloading)
        {
            FireBullet();
        }

        // 그립 버튼 상태가 0 -> 1로 변할 때 장전 시작
        if (previousGrips == 0 && gripPressed == 1 && currentBullet < maxBullet && !isReloading)
        {
            StartCoroutine(Reload());
            //StartCoroutine(GunSpin());
        }

        // 이전 그립 버튼 상태 업데이트
        previousGrips = gripPressed;
    }

    void FireBullet()
    {
        if (currentBullet > 0)
        {
            // 총알 발사
            GameObject bullet = GetBulletFromPool();
            bullet.transform.position = FirePoint.position; // 총알 위치 설정
            bullet.transform.rotation = FirePoint.rotation; // 총알 방향 설정
            bullet.SetActive(true);

            currentBullet--; // 발사 후 총알 감소
            canFire = false;
            StartCoroutine(FireSound()); // 발사 소리 재생

            // Haptics Trigger (진동 발생)
            haptics?.TriggerHapticFeedback(isLeft, hapticsAmplitude, hapticsDuration);

            //Debug.Log($"{(isLeft ? "왼쪽" : "오른쪽")} 총알 개수: {currentBullet}");
        }
        else
        {
            //Debug.Log($"{(isLeft ? "왼쪽" : "오른쪽")} 총알이 없습니다!");
            canFire = false;
            StartCoroutine(FireSound(true)); // 총알 없음 소리
        }
    }

    IEnumerator GunSpin()
    {
        float duration = 1f; // 회전 시간
        float angle = 720f; // 회전 각도
        float elapsedTime = 0f;

        Quaternion initalRotataion = handGun.transform.rotation; // 시작 회전상태
        Quaternion targetRotataion = initalRotataion * Quaternion.Euler(Vector3.right * angle);

        while (elapsedTime < duration)
        {
            // 시간의 경과에 따라 회전 보간
            float t = elapsedTime / duration; // 0에서 1까지
            handGun.transform.rotation = Quaternion.Lerp(initalRotataion, targetRotataion, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 회전 정확도 봏정
        handGun.transform.rotation = targetRotataion;
    }

    IEnumerator FireSound(bool isEmpty = false)
    {
        if (!isEmpty)
        {
            float volume = Random.Range(0.4f, 1f); // 발사 소리의 랜덤 볼륨
            source.PlayOneShot(fireSound, volume);
        }
        else
        {
            source.PlayOneShot(emptyGunSound); // 총알이 없을 때 소리
        }

        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }

    IEnumerator Reload()
    {
        // 장전 중
        isReloading = true;
        source.PlayOneShot(reloadingSound);
        yield return new WaitForSeconds(reloadTime);

        // 장전 완료
        currentBullet = maxBullet;
        source.PlayOneShot(reloadSound);
        Debug.Log($"{(isLeft ? "왼쪽" : "오른쪽")} 총알 재장전 완료");


        isReloading = false;
    }

    GameObject GetBulletFromPool()
    {
        if (bulletPool.Count > 0)
        {
            return bulletPool.Dequeue();
        }
        else
        {
            // 풀에 남은 총알이 없을 경우 새로운 총알 생성
            GameObject bullet = Instantiate(BulletPrefab);
            bullet.GetComponent<Bullet>().Initialize(this); // GunShooting 참조 전달

            //totalBulletsCreated++; // 생성된 총알 카운트 증가
            //Debug.Log($"새로운 총알 생성. 총 생성된 총알 수: {totalBulletsCreated}");

            return bullet;
        }
    }

    public void ReturnBulletToPool(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
    }
}
