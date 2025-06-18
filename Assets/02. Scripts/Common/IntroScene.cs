using System.Collections;
using TMPro;
using UnityEngine;

public class IntroScene : MonoBehaviour
{
    private WaitForSeconds nextScenarioDelay;

    private WaitForSeconds typingDelay;

    public TextMeshProUGUI typingTextUI;

    private bool isTag;

    private Coroutine introScenario;

    void Start()
    {
        nextScenarioDelay = new WaitForSeconds(1f);
        typingDelay = new WaitForSeconds(0.07f);
        typingTextUI.text = "";
        IntroScenario();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopCoroutine(introScenario);
            GameManager.Instance.sceneLoader.MoveScene("MainScene");
        }
    }

    public void IntroScenario()
    {
        introScenario = StartCoroutine(ScenarioExcute(Constants.introSceneText));
    }

    private IEnumerator ScenarioExcute(string[] scenario)
    {
        for (int i = 0; i < scenario.Length; i++)
        {
            
            yield return StartCoroutine(TypingScenario(scenario[i]));
        }

        yield return new WaitForSeconds(1f);

        GameManager.Instance.sceneLoader.MoveScene("MainScene");
    }

    private IEnumerator TypingScenario(string scenarioLine)
    {
        typingTextUI.text = "";
        string tempTag = "";
        foreach(char c in scenarioLine)
        {
            if(c == '<')
            {
                isTag = true;
                tempTag += c;
                continue;
            }

            if (isTag)
            {
                tempTag += c;
                if (c == '>') 
                {
                    isTag = false;
                }
                typingTextUI.text = tempTag;
                continue;
            }

            typingTextUI.text += c;
            yield return typingDelay;
        }

        yield return nextScenarioDelay;
    }
}
