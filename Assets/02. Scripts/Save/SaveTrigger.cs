using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTrigger : MonoBehaviour
{
    private SaveManager saveManager;

    private void Start()
    {
        saveManager = SaveManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !PlaySceneManager.instance.enemy.gameObject.activeInHierarchy)
        {
            saveManager.UpdatePlayerPosition(other.transform);
            
            saveManager.SaveGame();
        }
    }
}