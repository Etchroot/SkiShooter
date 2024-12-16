using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunShooting : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    public GameObject BulletPrefab; // 총알프리팹
    public Transform FirePointLeft; // 왼쪽 총구
    public Transform FirePointRight; // 오른쪽 총구

    public float fireRate = 0.03f; //발사 간격
    private bool LBullet = false; //Trigger 버튼 꾹 눌렀을때 실행 중복 방지 Left
    private bool RBullet = false; //Trigger 버튼 꾹 눌렀을때 실행 중복 방지 Right

    public AudioSource source; // 오디오 소스 컴포넌트그
    public AudioClip fireSound; //오디오 클립 컴포넌트

    public int MaxBullet = 100; // 최대 총알 개수
    public int LeftCurruntBullet = 100; // 왼쪽 현재 총알 개수
    public int RightCurruntBullet = 100; // 오른쪽 현재 총알 개수

    void Update()
    {
        var leftContTrigger = inputActions.actionMaps[2].actions[2].ReadValue<float>();
        var RightContTrigger = inputActions.actionMaps[5].actions[2].ReadValue<float>();

        if (LeftCurruntBullet > 0 && leftContTrigger == 1 && !LBullet)
        {
            //Debug.Log("왼쪽 트리거 눌림");
            LBullet = true;

            FireBullet(FirePointLeft, true);
            StartCoroutine(Sound(true));
        }
        else if (LeftCurruntBullet <= 0)
        {
            Debug.Log("왼쪽총알없음");
        }

        if (RightCurruntBullet > 0 && RightContTrigger == 1 && !RBullet)
        {
            //Debug.Log("오른쪽 트리거 눌림");
            RBullet = true;
            FireBullet(FirePointRight, false);
            StartCoroutine(Sound(false));
        }
        else if (RightCurruntBullet <= 0)
        {
            Debug.Log("오른쪽총알없음");
        }

        //재장전 테스트
        if (Input.GetKeyDown(KeyCode.C))
        {
            Reload(true);
            Reload(false);
        }
    }

    void FireBullet(Transform firePoint, bool isLeft)
    {
        Instantiate(BulletPrefab, firePoint.position, firePoint.rotation);

        if (isLeft)
        {
            LeftCurruntBullet--;
            Debug.Log($"왼쪽 총알 개수 : {LeftCurruntBullet}");
        }
        else
        {
            RightCurruntBullet--;
            Debug.Log($"오른쪽 총알 개수 : {RightCurruntBullet}");
        }
    }

    IEnumerator Sound(bool isLeft)
    {
        float ran = Random.Range(0.4f, 1f);
        source.PlayOneShot(fireSound, ran);
        yield return new WaitForSeconds(fireRate);

        if (isLeft)
        {
            LBullet = false;
        }
        else
        {
            RBullet = false;
        }
    }

    void Reload(bool isLeft)
    {
        if (!isLeft)
        {
            // 왼쪽
            LeftCurruntBullet = MaxBullet;
        }
        else
        {
            //오른쪽
            RightCurruntBullet = MaxBullet;
        }
    }

}
