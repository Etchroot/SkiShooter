using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunShooting : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    public GameObject BulletPrefab; //총알 프리팹
    public Transform FirePoint; // 총구 위치

    public bool isLeft = true; // true: 왼쪽, false : 오른쪽 (오른족 컨트롤러는 유니티에서 꼭 체크 해지 되어있어야함)

    [SerializeField] private float fireRate = 0.03f; //발사 딜레이
    private bool canFire = true; // 발사 중복 방지

    public AudioSource source; // 오디오
    public AudioClip fireSound; // 발사 소리
    public AudioClip reloadingSound; // 재장전 중 소리
    public AudioClip reloadSound; // 재장전 완료 소리
    public AudioClip emptyGunSound; // 총알 없음 소리

    [SerializeField] private int maxBullet = 100; //최대 총알수
    public int currentBullet = 100; //현재 총알 수 (각 컨트롤러에서 별도 관리)

    private float previousGrips = 0; // Grips 0 > 1 > 0 확인용 변수

    [SerializeField] private float reloadTime = 2f; // 재장전 시간
    public bool isReloading = false; // 재장전 중인지 확인

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
        }

        // 이전 그립 버튼 상태 업데이트
        previousGrips = gripPressed;
    }

    void FireBullet()
    {
        if (currentBullet > 0)
        {
            // 총알 발사
            Instantiate(BulletPrefab, FirePoint.position, FirePoint.rotation);
            currentBullet--; // 발사 후 총알 감소
            canFire = false;
            StartCoroutine(FireSound()); // 발사 소리 재생
            Debug.Log($"{(isLeft ? "왼쪽" : "오른쪽")} 총알 개수: {currentBullet}");
        }
        else
        {
            Debug.Log($"{(isLeft ? "왼쪽" : "오른쪽")} 총알이 없습니다!");
            canFire = false;
            StartCoroutine(FireSound(true)); // 총알 없음 소리
        }
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
        //장전중
        isReloading = true;
        source.PlayOneShot(reloadingSound);
        yield return new WaitForSeconds(reloadTime);

        //장전완료
        currentBullet = maxBullet;
        source.PlayOneShot(reloadSound);
        Debug.Log($"{(isLeft ? "왼쪽" : "오른쪽")} 총알 재장전 완료");

        isReloading = false;
    }
}
