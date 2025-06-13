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

    public Inputs PlayerInput => playerInput;
    private Inputs playerInput;

    public Transform equipPos;

    void Awake()
    {
        CharacterManager.Instance.Player = this;
        playerInput = GetComponent<Inputs>();
        controller = GetComponent<PlayerController>();
    }

    public void Init()
    {
        inventory = new Inventory(this);
        equipment = GetComponent<Equipment>();
        equipment.Init(this);
    }
}
