using System;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    private Player player => CharacterManager.Instance.Player;
    private PlayScnenUIManager playScnenUIManager;
    public event Action<IInteractable> OnInteracted;

    public ItemData keyData;

    public bool IsOpen => isOpen;
    [SerializeField] private bool isOpen;
    private Rigidbody rb;

    public bool clearDoor;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        rb ??= GetComponent<Rigidbody>();
        rb.isKinematic = !isOpen; //문이 열렸으면 키네마틱 false 처리를 통해 문이 움직이게 해준다.
        playScnenUIManager = (UIManager.Instance as PlayScnenUIManager);
    }

    public void OnInteraction()
    {
        if (!this.enabled)
        {
            return;
        }

        int slotNum = player.Inventory.FindItem(keyData).Item1;
        ItemInstance slotItem = player.Inventory.FindItem(keyData).Item2;

        if (slotItem == null)
        {
            playScnenUIManager.sequenceTextManager.SetSequenceText(Constants.CloseDoorText);
        }
        else
        {
            playScnenUIManager.sequenceTextManager.SetSequenceText(Constants.OpenDoorText);
            OpenDoor();
            player.Inventory.UseItem(slotNum);
            this.enabled = false;
            if (clearDoor)
            {
                GameManager.Instance.sceneLoader.MoveScene("OuttroScene");
            }
        }
    }

    public void SetInterface(bool active)
    {
        
    }

    public void OpenDoor()
    {
        isOpen = true;
        rb.isKinematic = false;
    }

    public void CloseDoor()
    {
        isOpen = false;
        rb.isKinematic = true;
    }
    
}
