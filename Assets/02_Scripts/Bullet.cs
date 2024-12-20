using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class Bullet : MonoBehaviour
{
    // �Ѿ� ��ũ��Ʈ
    // ������ ������ ������ ���ư��� ��ü�� �΋H���ų� 5���� Destroy
    
    [SerializeField] private float BulletSpeed = 5f;
        
    void Start()
    {
        //�����ǰ� 5���� ����
        StartCoroutine(Destroy());
    }

    void Update()
    {
        //�̵�
        transform.Translate(Vector3.forward * BulletSpeed * Time.deltaTime);
    }

    //�ٸ� ���� ������Ʈ�� ������ ����
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ENEMY"))
        {
            Destroy(other.gameObject);
        }
        else
        {
            //�ٴ��̳� �ٸ��� �ε�������
            Destroy(this.gameObject);
        }
        
    }

    IEnumerator Destroy()
    {
        //�����ǰ� 5�� ������ ����
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }

}
