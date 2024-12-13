using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunShooting : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    public GameObject BulletPrefab; // �Ѿ�������
    public Transform FirePointL; // ���� �ѱ�
    public Transform FirePointR; // ������ �ѱ�

    public float fireRate = 0.03f; //�߻� ����
    private bool LBullet = false; //Trigger ��ư �� �������� ���� �ߺ� ���� Left
    private bool RBullet = false; //Trigger ��ư �� �������� ���� �ߺ� ���� Right

    public AudioSource source; // ����� �ҽ� ������Ʈ��
    public AudioClip fireSound; //����� Ŭ�� ������Ʈ

    void Update()
    {
        var leftContTrigger = inputActions.actionMaps[2].actions[2].ReadValue<float>();
        var RightContTrigger = inputActions.actionMaps[5].actions[2].ReadValue<float>();

        if (leftContTrigger == 1 && !LBullet)
        {
            //Debug.Log("���� Ʈ���� ����");
            LBullet = true;

            FireBullet(FirePointL);
            StartCoroutine(Sound(true));
        }

        if (RightContTrigger == 1 && !RBullet)
        {
            //Debug.Log("������ Ʈ���� ����");
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
