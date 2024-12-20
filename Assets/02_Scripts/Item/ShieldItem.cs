using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShieldItem : Item
{
    private bool isShieldActive = false;

    public override void ActivateEffect()
    {
        if (isShieldActive)
        {
            Debug.Log("이미 보호막이 활성화 되어 있습니다.");
            return;
        }

        isShieldActive = true;
        Debug.Log("보호막 활성화");


        // Player의 데미지 이벤트 구독
        // Player.Instance.OnDamageTaken += BlockDamage;
        Player.Instance.OnDamageTaken += BlockDamage;
    }

    private void BlockDamage(ref float damage)
    {
        if (!isShieldActive)
        {
            return; // 보호막이 비활성화 상태라면 아무 작업도 수행하지 않음
        }

        // 한 번의 타격을 차단
        Debug.Log("보호막이 타격을 막았습니다.");
        damage = 0; // 데미지 0으로 만듦

        // 데미지 차단 후 보호막 비활성화
        DeactivateShield();
    }

    private void DeactivateShield()
    {
        if (!isShieldActive)
        {
            return; // 이미 비활성화 상태라면 아무 작업도 수행하지 않음
        }

        isShieldActive = false;
        Player.Instance.OnDamageTaken -= BlockDamage; // 이벤트 구독 해제
        Debug.Log("보호막 비활성화");
    }

}
