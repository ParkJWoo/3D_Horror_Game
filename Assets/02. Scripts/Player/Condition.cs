using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue;
    public float startValue;
    public float maxValue;
    public float passiveValue;
    public float addPassiveValue;
    private float useValue = 3f;
    public Image uiBar;

    private Coroutine applyPassiveValue;
    private Coroutine applyItemValue;

    private void Start()
    {
        curValue = startValue;
    }

    private void Update()
    {
        uiBar.fillAmount = GetPercentage();
    }

    private float GetPercentage()
    {
        return curValue / maxValue;
    }

    public void Add(float value)
    {
        curValue = Mathf.Min(curValue + value, maxValue);
    }

    public void Subtract(float value)
    {
        curValue = Mathf.Max(curValue - value, 0);
    }

    public void RecoverItemValue(float amount, float duration)
    {
        if (applyPassiveValue != null)
        {
            StopCoroutine(applyPassiveValue);
        }

        applyItemValue = StartCoroutine(ApplyItemValue(amount, duration));
    }

    private IEnumerator ApplyItemValue(float amount, float duration)
    {
        float durationCount = duration / 0.5f == 0 ? 1f : duration / 0.5f;
        float tickAmount = amount / durationCount;

        yield return null;

        while (durationCount > 0)
        {
            Add(tickAmount);
            durationCount--;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void GetAddPassiveValue(float amount, float duration)
    {
        if(applyPassiveValue != null)
        {
            StopCoroutine(applyPassiveValue);
        }

        applyPassiveValue = StartCoroutine(ApplyPassiveValue(amount, duration));
    }

    private IEnumerator ApplyPassiveValue(float amount, float duration)
    {
        addPassiveValue = amount;

        yield return new WaitForSeconds(duration);

        addPassiveValue = 0;
    }
}
