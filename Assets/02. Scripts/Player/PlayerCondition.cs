using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;
    private PlayerController controller;

    private float staminaRecoveryDelay = 1.5f;
    private float lastRunTime = 0f;

    private Condition stamina { get { return uiCondition.stamina; } }

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }

    void Update()
    {
        //stamina.Add(stamina.passiveValue * Time.deltaTime);

        //if (controller.isActuallyRunning)
        //{
        //    UseStamina(Time.fixedDeltaTime * 5);
        //}

        if (controller.isRunningInput)
        {
            if (!UseStamina(Time.deltaTime * 25f))
            {
                controller.isRunningInput = false;
            }
        }
        else
        {
            stamina.Add((stamina.passiveValue + stamina.addPassiveValue) * Time.deltaTime);
        }
    }

    public void Die()
    {
        // 죽는 함수
    }

    public bool UseStamina(float amount)
    {
        if (stamina.curValue - amount < 0f)
        {
            return false;
        }

        stamina.Subtract(amount);
        return true;
    }

    public void RecoverStamina(float amount , float duration)
    {
        stamina.RecoverItemValue(amount, duration);
    }
}
