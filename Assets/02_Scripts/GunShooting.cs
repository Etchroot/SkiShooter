using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunShooting : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    public GameObject BulletPrefab; //�Ѿ� ������
    public Transform FirePointLeft; //���� �ѱ�
    public Transform FirePointRight; //������ �ѱ�

    [SerializeField] private float fireRate = 0.03f; //�߻� ������
    private bool LBullet = false; //���� �Ѿ� �ߺ�����
    private bool RBullet = false; //������ �Ѿ� �ߺ�����

    public AudioSource source; //�����
    public AudioClip fireSound; //�߻�
    public AudioClip reloadSound; //������
    public AudioClip emptyGunSound; //�Ѿ� ����

    [SerializeField] private int maxBullet = 100; //�ִ� �Ѿ˼�
    public int leftCurrentBullet = 100; //���� ���� �Ѿ� ��
    public int rightCurrentBullet = 100; //������ ���� �Ѿ� ��

    private float previousLeftGrips = 0; // Grips 0 > 1 > 0 Ȯ�ο� ����
    private float previousRightGrips = 0;

    void Update()
    {
        // Trigger(0: �⺻ , 1: ����)
        var leftContTrigger = inputActions.actionMaps[2].actions[2].ReadValue<float>();
        var rightContTrigger = inputActions.actionMaps[5].actions[2].ReadValue<float>();

        //�߻�
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

        // Grips(0: �⺻ , 1: ����)
        float leftGrips = inputActions.actionMaps[2].actions[0].ReadValue<float>();
        float rightGrips = inputActions.actionMaps[5].actions[0].ReadValue<float>();

        // ���� Grips: 1 -> 0���� ��ȯ�� �� ����
        if (previousLeftGrips == 1 && leftGrips == 0)
        {
            Reload(true);
        }

        // ������ Grips: 1 -> 0���� ��ȯ�� �� ����
        if (previousRightGrips == 1 && rightGrips == 0)
        {
            Reload(false);
        }

        // Grips ���� ������Ʈ
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
            Debug.Log($"���� �Ѿ� ����: {leftCurrentBullet}");
        }
        else if (!isLeft && rightCurrentBullet > 0)
        {
            Instantiate(BulletPrefab, firePoint.position, firePoint.rotation);
            rightCurrentBullet--;
            StartCoroutine(FireSound(isLeft));
            Debug.Log($"������ �Ѿ� ����: {rightCurrentBullet}");
        }
        else
        {
            Debug.Log("�Ѿ��� �����ϴ�!");
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
            Debug.Log("���� �Ѿ� ������");
        }
        else if (!isLeft && rightCurrentBullet < maxBullet)
        {
            rightCurrentBullet = maxBullet;
            source.PlayOneShot(reloadSound);
            Debug.Log("������ �Ѿ� ������");
        }
    }
}
