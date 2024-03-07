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

    public void SlotSelected(InventorySlotUI selectedSlotUI)
    {
        bool mouseSlotFree = MouseInventoryItem.InventorySlot.ItemData == null;
        bool selectedSlotFree = selectedSlotUI.InventorySlot.ItemData == null;

        if (!selectedSlotFree && mouseSlotFree)
        {
            bool isLeftShiftPressed = Keyboard.current.leftShiftKey.isPressed;

            if (isLeftShiftPressed && selectedSlotUI.InventorySlot.SplitStack(out InventorySlot halfStackSlot))
            {
                MouseInventoryItem.UpdateMouseSlot(halfStackSlot);
                selectedSlotUI.UpdateUISlot();
            }

            else
            {
                MouseInventoryItem.UpdateMouseSlot(selectedSlotUI.InventorySlot);
                selectedSlotUI.ClearSlot();
            }
        }

        else if (selectedSlotFree && !mouseSlotFree)
        {
            selectedSlotUI.InventorySlot.AssignItem(MouseInventoryItem.InventorySlot);
            selectedSlotUI.UpdateUISlot();

            MouseInventoryItem.ClearSlot();
        }

        else if (!selectedSlotFree && !mouseSlotFree)
        {
            bool itemsAreEqual = selectedSlotUI.InventorySlot.ItemData == MouseInventoryItem.InventorySlot.ItemData;
            bool enoughSpaceToMerge = selectedSlotUI.InventorySlot.IsEnoughRoomAvailable(MouseInventoryItem.InventorySlot.StackSize, out int roomLeft);

            if (itemsAreEqual && enoughSpaceToMerge)
            {
                selectedSlotUI.InventorySlot.AddToStack(MouseInventoryItem.InventorySlot.StackSize);
                selectedSlotUI.UpdateUISlot();

                MouseInventoryItem.ClearSlot();
            }

            else if(itemsAreEqual && !enoughSpaceToMerge)
            {
                if (roomLeft == 0) SwapSlots(selectedSlotUI);
                else
                {
                    int leftOnMouse = MouseInventoryItem.InventorySlot.StackSize - roomLeft;
                    selectedSlotUI.InventorySlot.AddToStack(roomLeft);
                    selectedSlotUI.UpdateUISlot();

                    InventorySlot newItemOnMouse = new InventorySlot(MouseInventoryItem.InventorySlot.ItemData, leftOnMouse);
                    MouseInventoryItem.ClearSlot();
                    MouseInventoryItem.UpdateMouseSlot(newItemOnMouse);
                }
            }

            else SwapSlots(selectedSlotUI);
        }
    }

    public void SwitchTooltip(InventorySlotUI clickedSlotUI)
    {
        if (clickedSlotUI.InventorySlot.ItemData is NoteItem noteItem)
        {
            MouseInventoryItem.Tooltip.SwitchTooltip(noteItem);
        }
    }

    public void ClearTooltip()
    {
        MouseInventoryItem.Tooltip.ClearTooltip();
    }

    public abstract void UseItem(InventorySlotUI selectedSlotUI);

    private void SwapSlots(InventorySlotUI clickedSlotUI)
    {
        InventorySlot clonedSlot = new InventorySlot(MouseInventoryItem.InventorySlot.ItemData, MouseInventoryItem.InventorySlot.StackSize);
        MouseInventoryItem.ClearSlot();

        MouseInventoryItem.UpdateMouseSlot(clickedSlotUI.InventorySlot);

        clickedSlotUI.ClearSlot();
        clickedSlotUI.InventorySlot.AssignItem(clonedSlot);
        clickedSlotUI.UpdateUISlot();
    }
}
