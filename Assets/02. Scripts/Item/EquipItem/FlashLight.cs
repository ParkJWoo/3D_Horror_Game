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
    [SerializeField] private Light flashLight;

    private void Awake()
    {
        timeToConsumption = new WaitForSeconds(0.5f);
        flashOnConsumption = -5f;
        consumptionPerTick = -0.1f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) 
        {
            UseItem();
        }
    }

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
        flashLight.enabled = true;
    }

    private IEnumerator BatteryConsumptionHandler()
    {
        item.ChangeDurability(flashOnConsumption);

        yield return null;

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
        flashLight.enabled = false;
    }

    private bool CheckDurability()
    {
        return item.GetDurability();
    }
}
