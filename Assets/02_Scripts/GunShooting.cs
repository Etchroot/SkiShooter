using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunShooting : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    public GameObject BulletPrefab; // �Ѿ�������
    public Transform FirePointLeft; // ���� �ѱ�
    public Transform FirePointRight; // ������ �ѱ�

    public float fireRate = 0.03f; //�߻� ����
    private bool LBullet = false; //Trigger ��ư �� �������� ���� �ߺ� ���� Left
    private bool RBullet = false; //Trigger ��ư �� �������� ���� �ߺ� ���� Right

    public AudioSource source; // ����� �ҽ� ������Ʈ��
    public AudioClip fireSound; //����� Ŭ�� ������Ʈ

    public int MaxBullet = 100; // �ִ� �Ѿ� ����
    public int LeftCurruntBullet = 100; // ���� ���� �Ѿ� ����
    public int RightCurruntBullet = 100; // ������ ���� �Ѿ� ����

    void Update()
    {
        var leftContTrigger = inputActions.actionMaps[2].actions[2].ReadValue<float>();
        var RightContTrigger = inputActions.actionMaps[5].actions[2].ReadValue<float>();

        if (LeftCurruntBullet > 0 && leftContTrigger == 1 && !LBullet)
        {
            //Debug.Log("���� Ʈ���� ����");
            LBullet = true;

            FireBullet(FirePointLeft, true);
            StartCoroutine(Sound(true));
        }
        else if (LeftCurruntBullet <= 0)
        {
            Debug.Log("�����Ѿ˾���");
        }

        if (RightCurruntBullet > 0 && RightContTrigger == 1 && !RBullet)
        {
            //Debug.Log("������ Ʈ���� ����");
            RBullet = true;
            FireBullet(FirePointRight, false);
            StartCoroutine(Sound(false));
        }
        else if (RightCurruntBullet <= 0)
        {
            Debug.Log("�������Ѿ˾���");
        }

        //������ �׽�Ʈ
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
            Debug.Log($"���� �Ѿ� ���� : {LeftCurruntBullet}");
        }
        else
        {
            RightCurruntBullet--;
            Debug.Log($"������ �Ѿ� ���� : {RightCurruntBullet}");
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
            // ����
            LeftCurruntBullet = MaxBullet;
        }
        else
        {
            //������
            RightCurruntBullet = MaxBullet;
        }
    }

}
