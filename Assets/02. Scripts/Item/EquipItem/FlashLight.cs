using System.Collections;
using UnityEngine;

public class FlashLight : EquipItemHandler
{
    private bool isOnFlash; 

    private WaitForSeconds timeToConsumption;
                  
    private float flashOnConsumption;
    private float consumptionPerTick;

    private DurabilityType durabilityType;

    private Coroutine batteryConsumption;
    [SerializeField] private Light flashLight;

    float range;
    float spotAngle;
    int rayCount = 10;
    float angleStep;
    float startAngle;
    private LayerMask targetMask;

    public override void Init(Player player, ItemInstance item)
    {
        base.Init(player, item);
        timeToConsumption = new WaitForSeconds(0.5f);
        flashOnConsumption = -5f;
        consumptionPerTick = -0.1f;
        durabilityType = DurabilityType.flashlight;

        range = flashLight.range;
        spotAngle = flashLight.innerSpotAngle;
        angleStep = spotAngle / rayCount;
        startAngle = -spotAngle / 2f;

        if (GameManager.Instance.isNewGame)
        {
            PlaySceneManager.instance.uiManager.sequenceTextManager.SetSequenceText(Constants.getFlashlight);
        }

        isOnFlash = false;
        flashLight.enabled = false;
    }

    public override bool RecoverDurability(DurabilityData durabilityData)
    {
        if(durabilityType == durabilityData.durabiliyType) 
        {
            item.ChangeDurability(durabilityData.amount);
            return true;
        }

        return false;
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
        SoundManager.Instance.PlaySound("LightToggle");
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
            FindEnemy();
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
        SoundManager.Instance.PlaySound("LightToggle");
        flashLight.enabled = false;
    }

    private bool CheckDurability()
    {
        return item.GetDurability();
    }

    private void FindEnemy()
    {
        for (int i = 0; i <= rayCount; i++)
        {
            float angle = startAngle + (angleStep * i);
            Vector3 rayDir = Quaternion.Euler(0, angle, 0) * flashLight.transform.forward;

            if(Physics.Raycast(flashLight.transform.position, rayDir, out RaycastHit hit, range, targetMask))
            {
                if(hit.transform.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    Debug.Log("에너미 찾음");
                }
            }
        }
    }
}
