using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyMoveSpeedEffect : ApplyItemEffect
{
    public ApplyMoveSpeedEffect(Player player) : base(player) { }

    public override void ApplyItem(ItemEffect itemEffect)
    {
        player.controller.GetAddItemValue(itemEffect.amount, itemEffect.duration);
    }
}
