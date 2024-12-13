using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunShooting : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    public GameObject BulletPrefab; // 총알프리팹
    public Transform FirePointL; // 왼쪽 총구
    public Transform FirePointR; // 오른쪽 총구

    public float fireRate = 0.03f; //발사 간격
    private bool LBullet = false; //Trigger 버튼 꾹 눌렀을때 실행 중복 방지 Left
    private bool RBullet = false; //Trigger 버튼 꾹 눌렀을때 실행 중복 방지 Right

    public AudioSource source; // 오디오 소스 컴포넌트그
    public AudioClip fireSound; //오디오 클립 컴포넌트

    void Update()
    {
        var leftContTrigger = inputActions.actionMaps[2].actions[2].ReadValue<float>();
        var RightContTrigger = inputActions.actionMaps[5].actions[2].ReadValue<float>();

        if (leftContTrigger == 1 && !LBullet)
        {
            //Debug.Log("왼쪽 트리거 눌림");
            LBullet = true;

            FireBullet(FirePointL);
            StartCoroutine(Sound(true));
        }

        if (RightContTrigger == 1 && !RBullet)
        {
            //Debug.Log("오른쪽 트리거 눌림");
            RBullet = true;
            FireBullet(FirePointR);
            StartCoroutine(Sound(false));
        }
    }

    void FireBullet(Transform firePoint)
    {
        Instantiate(BulletPrefab, firePoint.position, firePoint.rotation);
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

}
