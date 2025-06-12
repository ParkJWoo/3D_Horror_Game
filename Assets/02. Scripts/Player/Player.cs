using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;

    public Inventory Inventory => inventory;
    private Inventory inventory;

    public Equipment Equipment => equipment;
    private Equipment equipment;

    void Awake()
    {
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();

        inventory = new Inventory(this);
        equipment = GetComponent<Equipment>();
    }
}
