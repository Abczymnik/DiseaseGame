using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InventoryDisplay : MonoBehaviour
{
    [field: SerializeField] public MouseItemData MouseInventoryItem { get; private set; }

    public InventorySystem InventorySystem { get; protected set; }
    public Dictionary<InventorySlotUI, InventorySlot> SlotDictionary { get; protected set; }

    protected virtual void OnValidate()
    {
        MouseInventoryItem = FindAnyObjectByType<MouseItemData>();
    }

    protected abstract void AssignSlots(InventorySystem inventoryToDisplay);

    protected virtual void UpdateSlot(InventorySlot updatedSlot)
    {
        foreach(var slot in SlotDictionary)
        {
            if (slot.Value == updatedSlot) slot.Key.UpdateUISlot(updatedSlot);
        }
    }

    public void SlotClicked(InventorySlotUI clickedSlotUI)
    {
        if(clickedSlotUI.InventorySlot.ItemData != null && MouseInventoryItem.InventorySlot.ItemData == null)
        {
            bool isLeftShiftPressed = Keyboard.current.leftShiftKey.isPressed;

            if (isLeftShiftPressed && clickedSlotUI.InventorySlot.SplitStack(out InventorySlot halfStackSlot))
            {
                MouseInventoryItem.UpdateMouseSlot(halfStackSlot);
                clickedSlotUI.UpdateUISlot();
            }

            else
            {
                MouseInventoryItem.UpdateMouseSlot(clickedSlotUI.InventorySlot);
                clickedSlotUI.ClearSlot();
            }
        }

        else if (clickedSlotUI.InventorySlot.ItemData == null && MouseInventoryItem.InventorySlot.ItemData != null)
        {
            clickedSlotUI.InventorySlot.AssignItem(MouseInventoryItem.InventorySlot);
            clickedSlotUI.UpdateUISlot();

            MouseInventoryItem.ClearSlot();
        }

        else if (clickedSlotUI.InventorySlot.ItemData != null && MouseInventoryItem.InventorySlot.ItemData != null)
        {
            bool itemsAreEqual = clickedSlotUI.InventorySlot.ItemData == MouseInventoryItem.InventorySlot.ItemData;
            bool enoughSpaceToMerge = clickedSlotUI.InventorySlot.IsEnoughRoomAvailable(MouseInventoryItem.InventorySlot.StackSize, out int roomLeft);

            if (itemsAreEqual && enoughSpaceToMerge)
            {
                clickedSlotUI.InventorySlot.AddToStack(MouseInventoryItem.InventorySlot.StackSize);
                clickedSlotUI.UpdateUISlot();

                MouseInventoryItem.ClearSlot();
            }

            else if(itemsAreEqual && !enoughSpaceToMerge)
            {
                if (roomLeft == 0) SwapSlots(clickedSlotUI);
                else
                {
                    int leftOnMouse = MouseInventoryItem.InventorySlot.StackSize - roomLeft;
                    clickedSlotUI.InventorySlot.AddToStack(roomLeft);
                    clickedSlotUI.UpdateUISlot();

                    InventorySlot newItemOnMouse = new InventorySlot(MouseInventoryItem.InventorySlot.ItemData, leftOnMouse);
                    MouseInventoryItem.ClearSlot();
                    MouseInventoryItem.UpdateMouseSlot(newItemOnMouse);
                }
            }

            else SwapSlots(clickedSlotUI);
        }
    }

    private void SwapSlots(InventorySlotUI clickeSlotUI)
    {
        InventorySlot clonedSlot = new InventorySlot(MouseInventoryItem.InventorySlot.ItemData, MouseInventoryItem.InventorySlot.StackSize);
        MouseInventoryItem.ClearSlot();

        MouseInventoryItem.UpdateMouseSlot(clickeSlotUI.InventorySlot);

        clickeSlotUI.ClearSlot();
        clickeSlotUI.InventorySlot.AssignItem(clonedSlot);
        clickeSlotUI.UpdateUISlot();
    }
}
