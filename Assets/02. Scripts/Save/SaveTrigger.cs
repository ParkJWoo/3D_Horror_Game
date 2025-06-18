using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTrigger : MonoBehaviour
{
    private SaveManager saveManager;
    private ItemManager itemManager;

    private void Start()
    {
        saveManager = SaveManager.Instance;
        itemManager = PlaySceneManager.instance.itemManager;
    }

    public void SaveData()
    {
        Player player = FindObjectOfType<Player>();

        saveManager.UpdatePlayerData(player);
        itemManager.Save();
        saveManager.SaveGame();
    }
}