using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;
    private PlayerController controller;

    public bool isExhausted = false;
    private bool isNormalState = true;

    private Color whiteColor = new Color(180 / 255f, 180 / 255f, 180 / 255f, 255 / 255f);
    private Color redColor = new Color(180 / 255f, 50 / 255f, 50 / 255f, 255 / 255f);
    private Color redColorBlink = new Color(180 / 255f, 50 / 255f, 50 / 255f, 20 / 255f);
    private float lerpT = 0f;


    Condition stamina { get { return uiCondition.stamina; } }

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }

    private void Start()
    {
        //stamina.icon.color = whiteColor; // 시작 시 스태미너 아이콘 하얀색
    }

    void Update()
    {
        stamina.uiBar.color = Color.Lerp(whiteColor, redColor, 1-stamina.GetPercentage());
        if (controller.isRunningInput && controller.isMoving)
        {
            if (!UseStamina(Time.deltaTime * 25f))
            {
                controller.isRunningInput = false;
            }
        }
        else
        {
            if (!isExhausted) stamina.Add(stamina.passiveValue * Time.deltaTime);
        }

        if (!isExhausted && stamina.curValue < 0.2f && !isNormalState)
        {
            StartCoroutine(Exhaustion());
        }

        ExhaustionIcon();

    }

    public bool UseStamina(float amount) // 스태미너 사용
    {
        if (stamina.curValue - amount < 0f)
        {
 
            isNormalState = false;
            return false;
        }

        stamina.Subtract(amount);
        return true;
    }

    IEnumerator Exhaustion() // 탈진상태
    {
        isExhausted = true;

        yield return new WaitForSeconds(3f);
        isExhausted = false;
        isNormalState = true;

    }

    public void ExhaustionIcon() // 탈진하면 icon 빨간색으로 변하고 깜빡깜빡
    {        
        if (isExhausted) 
        {
            lerpT += Time.deltaTime*2;
            lerpT = Mathf.Clamp01(lerpT);
            stamina.icon.color = Color.Lerp(whiteColor, redColor, lerpT);
            if(stamina.icon.color == redColor)
            {
                float pingPong = Mathf.PingPong(Time.time * 2f, 1f);
                stamina.icon.color = Color.Lerp(redColor, redColorBlink, pingPong);
            }
        }
        else
        {
            lerpT -= Time.deltaTime*2;
            lerpT = Mathf.Clamp01(lerpT);
            stamina.icon.color = Color.Lerp(redColor, whiteColor, 1-lerpT);
        }
    }
}
