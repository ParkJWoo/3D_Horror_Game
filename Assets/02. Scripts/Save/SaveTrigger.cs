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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !PlaySceneManager.instance.enemy.gameObject.activeInHierarchy)
        {
            Player player = other.GetComponent<Player>();
            
            saveManager.UpdatePlayerData(player);
            itemManager.Save();
            saveManager.SaveGame();
        }
        
    }
}