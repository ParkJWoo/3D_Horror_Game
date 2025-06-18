using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySceneManager : MonoBehaviour
{
    public static PlaySceneManager instance;
    
    public Enemy enemy;

    private void Awake()
    {
        instance = this;
    }

    public PlayScnenUIManager uiManager;
    public ItemManager itemManager;
    public CharacterManager characterManager;
    public GuideManager guideManager;

    public void Init() 
    {
        itemManager ??= GetComponentInChildren<ItemManager>();
        itemManager?.Init();

        characterManager = CharacterManager.Instance;

        characterManager.Player.Init();

        uiManager ??= FindObjectOfType<PlayScnenUIManager>();
        uiManager?.Init();

        if (GameManager.Instance.isNewGame)
        {
            StartCoroutine(SenarioSequenceText(Constants.introText));
        }
    }
    
    private IEnumerator SenarioSequenceText(string[] scenario)
    {
        Time.timeScale = 0.0f;
        yield return null;

        for (int i = 0; i < scenario.Length; i++)
        {
            uiManager.sequenceTextManager.SetSequenceText(scenario[i]);
            Time.timeScale = 0.0f;
            CharacterManager.Instance.Player.transform.rotation = Quaternion.Euler(0, 194.207f, 0);
            yield return new WaitForSecondsRealtime(1.5f);
        }

        yield return null;

        Time.timeScale = 1.0f;

    }

    private void OnDestroy()
    {
        instance = null;
    }
}
