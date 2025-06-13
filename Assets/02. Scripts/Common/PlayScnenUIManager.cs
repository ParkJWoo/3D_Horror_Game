using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayScnenUIManager : UIManager
{
    public InventoryUI inventoryUI;
    public InventoryDetailView inventoryDetailView;
    public EquipmentUI equipUI;

    public override void Init()
    {
        Player player = CharacterManager.Instance.Player;
        inventoryUI.Init(player);
        inventoryDetailView.Init();
        equipUI.Init(player);
    }
}
