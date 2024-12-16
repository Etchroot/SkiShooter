using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunShooting : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    public GameObject BulletPrefab; //총알 프리팹
    public Transform FirePointLeft; //왼쪽 총구
    public Transform FirePointRight; //오른쪽 총구

    [SerializeField] private float fireRate = 0.03f; //발사 딜레이
    private bool LBullet = false; //왼쪽 총알 중복방지
    private bool RBullet = false; //오른쪽 총알 중복방지

    public AudioSource source; //오디오
    public AudioClip fireSound; //발사
    public AudioClip reloadSound; //재장전
    public AudioClip emptyGunSound; //총알 없음

    [SerializeField] private int maxBullet = 100; //최대 총알수
    public int leftCurrentBullet = 100; //왼쪽 현재 총알 수
    public int rightCurrentBullet = 100; //오른쪽 현재 총알 수

    private float previousLeftGrips = 0; // Grips 0 > 1 > 0 확인용 변수
    private float previousRightGrips = 0;

    void Update()
    {
        // Trigger(0: 기본 , 1: 눌림)
        var leftContTrigger = inputActions.actionMaps[2].actions[2].ReadValue<float>();
        var rightContTrigger = inputActions.actionMaps[5].actions[2].ReadValue<float>();

        //발사
        if (leftContTrigger == 1 && !LBullet)
        {
            LBullet = true;
            FireBullet(FirePointLeft, true);
        }

        if (rightContTrigger == 1 && !RBullet)
        {
            RBullet = true;
            FireBullet(FirePointRight, false);
        }

        // Grips(0: 기본 , 1: 눌림)
        float leftGrips = inputActions.actionMaps[2].actions[0].ReadValue<float>();
        float rightGrips = inputActions.actionMaps[5].actions[0].ReadValue<float>();

        // 왼쪽 Grips: 1 -> 0으로 전환될 때 장전
        if (previousLeftGrips == 1 && leftGrips == 0)
        {
            Reload(true);
        }

        // 오른쪽 Grips: 1 -> 0으로 전환될 때 장전
        if (previousRightGrips == 1 && rightGrips == 0)
        {
            Reload(false);
        }

        // Grips 상태 업데이트
        previousLeftGrips = leftGrips;
        previousRightGrips = rightGrips;
    }

    void FireBullet(Transform firePoint, bool isLeft)
    {

        if (isLeft && leftCurrentBullet > 0)
        {
            Instantiate(BulletPrefab, firePoint.position, firePoint.rotation);
            leftCurrentBullet--;
            StartCoroutine(FireSound(isLeft));
            Debug.Log($"왼쪽 총알 개수: {leftCurrentBullet}");
        }
        else if (!isLeft && rightCurrentBullet > 0)
        {
            Instantiate(BulletPrefab, firePoint.position, firePoint.rotation);
            rightCurrentBullet--;
            StartCoroutine(FireSound(isLeft));
            Debug.Log($"오른쪽 총알 개수: {rightCurrentBullet}");
        }
        else
        {
            Debug.Log("총알이 없습니다!");
            StartCoroutine(FireSound(isLeft, true));
        }
    }

    IEnumerator FireSound(bool isLeft)
    {
        float ran = Random.Range(0.4f, 1f);
        source.PlayOneShot(fireSound, ran);
        yield return new WaitForSeconds(fireRate);

        if (isLeft) LBullet = false;
        else RBullet = false;
    }

    IEnumerator FireSound(bool isLeft, bool isEempty)
    {
        if (isEempty)
        {
            source.PlayOneShot(emptyGunSound);
            yield return new WaitForSeconds(fireRate);

            if (isLeft) LBullet = false;
            else RBullet = false;
        }
        else
        {
            yield return null;
        }
    }

    void Reload(bool isLeft)
    {
        if (isLeft && leftCurrentBullet < maxBullet)
        {
            leftCurrentBullet = maxBullet;
            source.PlayOneShot(reloadSound);
            Debug.Log("왼쪽 총알 재장전");
        }
        else if (!isLeft && rightCurrentBullet < maxBullet)
        {
            rightCurrentBullet = maxBullet;
            source.PlayOneShot(reloadSound);
            Debug.Log("오른쪽 총알 재장전");
        }
    }
}
