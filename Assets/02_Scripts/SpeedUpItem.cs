using UnityEngine;

public class SpeedUpItem : Item
{
    [SerializeField]
    private float speedup = 2f; // 이동 속도 증가량
    public override void ActivateEffect()
    {
        Debug.Log("가속 아이템 사용");

        Player player = Player.Instance;

        if (player != null)
        {
            Debug.Log("속도 증가");
            player.SetMoveSpeed(player.moveSpeed + speedup);
        }

    }
}
