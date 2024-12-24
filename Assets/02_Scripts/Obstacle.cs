using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        player = GetComponent<Player>();
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PLAYERHEAD"))
        {
            player.TakeDamage(0);
            Debug.Log("머리랑 충돌 일어남");
            //TakeDamage(); // 데미지 입는 함수 호출
        }
    }
}
