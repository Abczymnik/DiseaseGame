using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public abstract class InventoryDisplay : MonoBehaviour
{
    [SerializeField] private MouseItemData mouseInventoryItem;

    public InventorySystem InventorySystem { get; protected set; }
    public Dictionary<InventorySlotUI, InventorySlot> SlotDictionary { get; protected set; }

    protected virtual void OnValidate()
    {
        mouseInventoryItem = FindAnyObjectByType<MouseItemData>();
    }

    protected virtual void Start()
    {

    }

    public abstract void AssignSlots(InventorySystem inventoryToDisplay);

    protected virtual void UpdateSlot(InventorySlot updatedSlot)
    {
        foreach(var slot in SlotDictionary)
        {
            if (slot.Value == updatedSlot) slot.Key.UpdateUISlot(updatedSlot);
        }
    }

    public void SlotClicked(InventorySlotUI clickedSlotUI)
    {
        bool isLeftShiftPressed = Keyboard.current.leftShiftKey.isPressed;

        if(clickedSlotUI.InventorySlot.ItemData != null && mouseInventoryItem.InventorySlot.ItemData == null)
        {
            if(isLeftShiftPressed && clickedSlotUI.InventorySlot.SplitStack(out InventorySlot halfStackSlot))
            {
                mouseInventoryItem.UpdateMouseSlot(halfStackSlot);
                clickedSlotUI.UpdateUISlot();
            }

            else
            {
                mouseInventoryItem.UpdateMouseSlot(clickedSlotUI.InventorySlot);
                clickedSlotUI.ClearSlot();
            }
        }

        else if (clickedSlotUI.InventorySlot.ItemData == null && mouseInventoryItem.InventorySlot.ItemData != null)
        {
            clickedSlotUI.InventorySlot.AssignItem(mouseInventoryItem.InventorySlot);
            clickedSlotUI.UpdateUISlot();

            mouseInventoryItem.ClearSlot();
        }

        else if (clickedSlotUI.InventorySlot.ItemData != null && mouseInventoryItem.InventorySlot.ItemData != null)
        {
            bool itemsAreEqual = clickedSlotUI.InventorySlot.ItemData == mouseInventoryItem.InventorySlot.ItemData;
            bool enoughSpaceToMerge = clickedSlotUI.InventorySlot.IsEnoughRoomAvailable(mouseInventoryItem.InventorySlot.StackSize, out int roomLeft);

            if (itemsAreEqual && enoughSpaceToMerge)
            {
                clickedSlotUI.InventorySlot.AddToStack(mouseInventoryItem.InventorySlot.StackSize);
                clickedSlotUI.UpdateUISlot();

                mouseInventoryItem.ClearSlot();
            }

            else if(itemsAreEqual && !enoughSpaceToMerge)
            {
                if (roomLeft == 0) SwapSlots(clickedSlotUI);
                else
                {
                    int leftOnMouse = mouseInventoryItem.InventorySlot.StackSize - roomLeft;
                    clickedSlotUI.InventorySlot.AddToStack(roomLeft);
                    clickedSlotUI.UpdateUISlot();

                    InventorySlot newItemOnMouse = new InventorySlot(mouseInventoryItem.InventorySlot.ItemData, leftOnMouse);
                    mouseInventoryItem.ClearSlot();
                    mouseInventoryItem.UpdateMouseSlot(newItemOnMouse);
                }
            }

            else SwapSlots(clickedSlotUI);
        }
    }

    public void SwapSlots(InventorySlotUI clickeSlotUI)
    {
        InventorySlot clonedSlot = new InventorySlot(mouseInventoryItem.InventorySlot.ItemData, mouseInventoryItem.InventorySlot.StackSize);
        mouseInventoryItem.ClearSlot();

        mouseInventoryItem.UpdateMouseSlot(clickeSlotUI.InventorySlot);

        clickeSlotUI.ClearSlot();
        clickeSlotUI.InventorySlot.AssignItem(clonedSlot);
        clickeSlotUI.UpdateUISlot();
    }
}
