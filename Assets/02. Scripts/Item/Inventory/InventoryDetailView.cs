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

    private bool isOpen;

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
        isOpen = true;
        if (gameObject.activeSelf != isOpen)
        {
            gameObject.SetActive(true);
            inventoryUI.HideUI();
        }
    }

    public void CloseUI()
    {
        isOpen = false;
        if (gameObject.activeSelf != isOpen)
        {
            gameObject.SetActive(false);
            ResetView();
        }
    }

    public void ReturnInvenUI()
    {
        inventoryUI.Show();
        currentSlot.DeselectSlot();
    }

    public void ResetView()
    {
        currentSlot = null;
        detailItemImage.sprite = emptyImage;
        detailDescription.text = "";
    }

    public void SetDetailInfo(InvenSlot invenSlot)
    {
        currentSlot = invenSlot;
        if (currentSlot.slotItem != null)
        {
            OpenUI();
            ItemData itemData = currentSlot.slotItem.itemData;
            detailItemImage.sprite = itemData.itemImage;
            detailDescription.text = itemData.itemDetailDescription;

            useButton.gameObject.SetActive(ActiveUseButton(itemData));
        }
        else
        {
            ResetView();
        }
    }

    public void LeftArrow()
    {
        currentSlot = inventoryUI.FindPreviousSlot();

        if (currentSlot != null || currentSlot == this)
        {
            SetDetailInfo(currentSlot);
        }
    }

    public void RightArrow()
    {
        currentSlot = inventoryUI.FindNextSlot();

        if (currentSlot != null || currentSlot == this)
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

    public bool ActiveUseButton(ItemData itemData)
    {
        if (itemData.itemType == ItemType.consumable) return true;
        if (itemData.itemType == ItemType.battery) return true;
        return false;
    }
}
