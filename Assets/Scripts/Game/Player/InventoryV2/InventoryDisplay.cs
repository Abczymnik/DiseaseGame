using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public abstract class InventoryDisplay : MonoBehaviour
{
    [SerializeField] private MouseItemData mouseInventoryItem;

    public InventorySystem InventorySystem { get; protected set; }
    public Dictionary<InventorySlotUI, InventorySlot> SlotDictionary { get; protected set; }

    protected virtual void Start()
    {

    }

    public abstract void AssignSlot(InventorySystem inventoryToDisplay);

    protected virtual void UpdateSlot(InventorySlot updatedSlot)
    {
        foreach(var slot in SlotDictionary)
        {
            if (slot.Value == updatedSlot) slot.Key.UpdateUISlot(updatedSlot);
        }
    }

    public void SlotClicked(InventorySlotUI clickedSlotUI)
    {
        if(clickedSlotUI.InventorySlot.ItemData != null && mouseInventoryItem.InventorySlot.ItemData == null)
        {
            mouseInventoryItem.UpdateMouseSlot(clickedSlotUI.InventorySlot);
            clickedSlotUI.ClearSlot();
            return;
        }
    }
}
