using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyStaminaEffect : ApplyItemEffect
{
    public ApplyStaminaEffect(Player player) : base(player) { }

    public override void ApplyItem(ItemEffect itemEffect)
    {
        player.condition.RecoverStamina(itemEffect.amount , itemEffect.duration);
    }
}
