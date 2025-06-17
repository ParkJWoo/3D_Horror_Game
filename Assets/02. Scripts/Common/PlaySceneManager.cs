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

    public void Init() 
    {
        characterManager = CharacterManager.Instance;
        characterManager.Player.Init();
        uiManager ??= FindObjectOfType<PlayScnenUIManager>();
        uiManager?.Init();
        itemManager ??= GetComponentInChildren<ItemManager>();
        itemManager?.Init();
    }
    

    private void OnDestroy()
    {
        instance = null;
    }
}
