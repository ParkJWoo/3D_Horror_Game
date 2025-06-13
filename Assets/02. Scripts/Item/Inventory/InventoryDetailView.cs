using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDetailView : MonoBehaviour
{
    public Image detailItemImage;
    public Button leftArrow;
    public Button rightArrow;
    public TextMeshProUGUI detailDescription;
    public Button useButton;

    public Sprite emptyImage;

    private InvenSlot currentSlot;
    private Inventory inventory;
    private InventoryUI inventoryUI;

    public void Init()
    {
        inventory = CharacterManager.Instance.Player.Inventory;
        inventoryUI = PlaySceneManager.instance.uiManager.inventoryUI;
        detailItemImage.sprite = emptyImage;
        leftArrow.onClick.AddListener(LeftArrow);
        rightArrow.onClick.AddListener(RightArrow);
        detailDescription.text = "";
        useButton.onClick.AddListener(UseButton);
        useButton.gameObject.SetActive(false);
        CloseUI();
    }

    public void OpenUI()
    {
        gameObject.SetActive(true);
        inventoryUI.HideUI();
    }

    public void CloseUI()
    {
        gameObject.SetActive(false);
        detailItemImage.sprite = emptyImage;
        detailDescription.text = "";
        //inventoryUI.OpenUI();
    }

    public void SetDetailInfo(InvenSlot invenSlot)
    {
        OpenUI();
        currentSlot = invenSlot;
        if (currentSlot.slotItem != null)
        {
            ItemData itemData = currentSlot.slotItem.itemData;
            detailItemImage.sprite = itemData.itemImage;
            detailDescription.text = itemData.itemDetailDescription;

            useButton.gameObject.SetActive(itemData.itemType == ItemType.consumable);
        }
        else
        {
            CloseUI();
        }
    }

    public void LeftArrow()
    {
        Debug.Log("레프트에로우");
        //currentSlot = inventoryUI.FindPreviousSlot();

        if (currentSlot != null)
        {
            SetDetailInfo(currentSlot);
        }
    }

    public void RightArrow()
    {
        Debug.Log("라이트에로우");
        //currentSlot = inventoryUI.FindNextSlot();

        if (currentSlot != null)
        {
            SetDetailInfo(currentSlot);
        }
    }

    public void UseButton()
    {
        inventory.UseItem();
        if (currentSlot == null || currentSlot.slotItem == null)
        {
            CloseUI();
        }
    }
}
