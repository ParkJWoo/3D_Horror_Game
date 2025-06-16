using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ApplyItemEffect
{
    public ApplyItemEffect(Player player)
    {
        this.player = player;
    }

    protected Player player;

    public abstract void ApplyItem(ItemEffect itemEffect);
}
