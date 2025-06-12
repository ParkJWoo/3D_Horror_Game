using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : EquipItemHandler
{
    private bool isOnFlash; 
    private WaitForSeconds timeToConsumption;
                  
    private float flashOnConsumption;
    private float consumptionPerTick;

    private Coroutine batteryConsumption;
    private Light flashLight;

    public override void UseItem()
    {
        Toggle();
    }

    private void Toggle()
    {
        if (isOnFlash)
        {
            FlashOff();
        }
        else
        {
            FlashOn();
        }
    }

    private void FlashOn()
    {
        if (!CheckDurability())
        {
            FlashOff();
            return;
        }

        isOnFlash = true;
        if (batteryConsumption == null)
        {
            batteryConsumption = StartCoroutine(BatteryConsumptionHandler());
        }
    }

    private IEnumerator BatteryConsumptionHandler()
    {
        item.ChangeDurability(flashOnConsumption);

        if (!CheckDurability())
        {
            FlashOff();
            yield break;
        }

        while (isOnFlash)
        {
            yield return timeToConsumption;
            item.ChangeDurability(consumptionPerTick);
            if (!CheckDurability())
            {
                FlashOff();
                yield break;
            }
        }
    }

    private void FlashOff()
    {
        isOnFlash = false;
        if (batteryConsumption != null)
        {
            StopCoroutine(batteryConsumption);
            batteryConsumption = null;
        }
    }

    private bool CheckDurability()
    {
        return item.GetDurability();
    }
}
