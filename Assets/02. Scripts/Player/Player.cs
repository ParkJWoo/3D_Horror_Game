using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;

    public PlayerCondition condition;

    public Inventory Inventory => inventory;
    private Inventory inventory;

    public Equipment Equipment => equipment;
    private Equipment equipment;

    public Inputs PlayerInput => playerInput;
    private Inputs playerInput;

    public Transform equipPos;

    private Dictionary<ItemEffectType, ApplyItemEffect> applyItemeffectDictionary = new Dictionary<ItemEffectType, ApplyItemEffect>();

    void Awake()
    {
        CharacterManager.Instance.Player = this;
    }

    public void Init()
    {
        transform.position = SaveManager.Instance.saveData.playerPosition;

        playerInput = GetComponent<Inputs>();
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();

        inventory = new Inventory(this);
        equipment = GetComponent<Equipment>();
        equipment.Init(this);
        RegistedApplyItemEffect();

        equipment.OnEquipHandler += controller.ApplyEquipItem;
        equipment.OnUnequipHandler += controller.RemoveEquipItem;

        equipment.OnEquipHandler += condition.uiCondition.stamina.ApplyEquipItem;
        equipment.OnUnequipHandler += condition.uiCondition.stamina.RemoveEquipItem;
    }

    public void ApplyUseItem(ItemEffect itemEffect)
    {
        if (applyItemeffectDictionary.TryGetValue(itemEffect.itemEffectType, out ApplyItemEffect applyItemEffect)) 
        {
            applyItemEffect.ApplyItem(itemEffect);
        }
    }

    private void RegistedApplyItemEffect()
    {
        applyItemeffectDictionary[ItemEffectType.moveSpeed] = new ApplyMoveSpeedEffect(this);
        applyItemeffectDictionary[ItemEffectType.stamina] = new ApplyStaminaEffect(this);
        applyItemeffectDictionary[ItemEffectType.staminaRegen] = new ApplyStaminaRegenEffect(this);
    }

    private void OnDestroy()
    {
        equipment.OnEquipHandler -= controller.ApplyEquipItem;
        equipment.OnUnequipHandler -= controller.RemoveEquipItem;

        equipment.OnEquipHandler -= condition.uiCondition.stamina.ApplyEquipItem;
        equipment.OnUnequipHandler -= condition.uiCondition.stamina.RemoveEquipItem;
    }
}
