using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyStaminaRegenEffect : ApplyItemEffect
{
    public ApplyStaminaRegenEffect(Player player) : base(player) { }
  
    public override void ApplyItem(ItemEffect itemEffect)
    {
        player.condition.uiCondition.stamina.GetAddPassiveValue(itemEffect.amount, itemEffect.duration);
    }
}
