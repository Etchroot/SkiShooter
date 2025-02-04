using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.InputSystem;

public class GunShooting : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    public GameObject BulletPrefab; // 총알 프리팹
    public Transform FirePoint; // 총구 위치

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

    // 탄창 재장전 모션 관련 변수
    [SerializeField] private string gunclipPrefabPath = "GunClip"; // Resources 내 경로
    [SerializeField] private Transform gunclipPosition; // 탄창이 결합될 위치
    [SerializeField] private float fallSpeed = 10f; // 탄창 떨어지는 속도

    private GameObject gunclipPrefab; // 새로운 탄창 프리펩
    public GameObject currentGunclip; // 현재 결합된 탄창

    // UnityEngine.Pool을 활용한 오브젝트 풀
    private ObjectPool<GameObject> bulletPool;

    void Awake()
    {
        haptics = GetComponent<Haptic>();

        // Resources 폴더에서 프리펩 로드
        gunclipPrefab = Resources.Load<GameObject>(gunclipPrefabPath);

    }

    void Start()
    {
        if (ObjectPoolManager.Instance == null)
        {
            Debug.LogError("ObjectPoolManager 인스턴스가 초기화되지 않았습니다!");
            return;
        }

        ObjectPoolManager.Instance.CreatePool("Bullet", BulletPrefab, 60, 100);

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
        Vector3 bulletDirection = FirePoint.TransformDirection(Vector3.forward);
        if (currentBullet <= 0)
        {
            StartCoroutine(FireSound(true)); // 총알 없음 소리
            return;
        }

        canFire = false;
        currentBullet--;

        GameObject bulletObj = ObjectPoolManager.Instance.GetFromPool("Bullet", FirePoint.position, FirePoint.rotation);
        Bullet bullet = bulletObj.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Pool = ObjectPoolManager.Instance.GetPool("Bullet") as IObjectPool<Bullet>;
            bullet.SetDirection(bulletDirection);
        }
        else
        {
            Debug.LogError("FireBullet: Bullet 컴포넌트를 찾을 수 없습니다!");
        }
        
        StartCoroutine(FireSound());
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
            source.PlayOneShot(emptyGunSound, 0.8f); // 총알이 없을 때 소리
        }

        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }

    IEnumerator Reload()
    {
        // 장전 중
        isReloading = true;
        ReloadMotion();
        source.PlayOneShot(reloadingSound);
        yield return new WaitForSeconds(reloadTime);

        // 장전 완료
        currentBullet = maxBullet;
        source.PlayOneShot(reloadSound);
        //Debug.Log($"{(isLeft ? "왼쪽" : "오른쪽")} 총알 재장전 완료");


        isReloading = false;
    }

    #region 탄창 재장전 모션


    public void ReloadMotion()
    {
        // 기존 탄창 배출
        EjectCurrentGunclip();

        // 딜레이 추가
        StartCoroutine(DelayReloadMotion());

        IEnumerator DelayReloadMotion()
        {
            yield return new WaitForSeconds(reloadTime);
            // 새로운 탄창 생성 및 결합
            AttachNewGunclip();
        }

    }

    private void EjectCurrentGunclip()
    {
        if (currentGunclip != null)
        {
            // 탄창 배출 로직
            currentGunclip.transform.SetParent(null); // 결합 해체

            // Rigidbody 추가 및 Y축 방향으로 힘 가하기
            Rigidbody rb = currentGunclip.GetComponent<Rigidbody>();
            if (rb == null) rb = currentGunclip.AddComponent<Rigidbody>();

            rb.AddForce(gunclipPosition.up * -fallSpeed, ForceMode.VelocityChange);


            // 배출된 탄창 파괴
            Destroy(currentGunclip, 2f);

            // 탄창 참조 해제
            currentGunclip = null;
        }
    }

    private void AttachNewGunclip()
    {
        // 새로운 탄창 생성
        GameObject newGunclip = Instantiate(gunclipPrefab, gunclipPosition.position, gunclipPosition.rotation);

        // 결합
        newGunclip.transform.SetParent(gunclipPosition);
        currentGunclip = newGunclip;
    }

    #endregion
}
