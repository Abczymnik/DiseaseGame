using System.Collections.Generic;
using UnityEngine;

public class DynamicInventoryDisplay : InventoryDisplay
{
    [SerializeField] private InventorySlotUI slotPrefab;

    public void RefreshDynamicInventory(InventorySystem inventoryToDisplay)
    {
        ClearSlots();
        this.InventorySystem = inventoryToDisplay;
        AssignSlots(inventoryToDisplay);
    }

    protected override void AssignSlots(InventorySystem inventoryToDisplay)
    {
        SlotDictionary = new Dictionary<InventorySlotUI, InventorySlot>();

        if (inventoryToDisplay == null) return;

        for (int i = 0; i < inventoryToDisplay.InventorySlots.Count; i++)
        {
            var slotUI = Instantiate(slotPrefab, transform);
            SlotDictionary.Add(slotUI, inventoryToDisplay.InventorySlots[i]);
            slotUI.Init(inventoryToDisplay.InventorySlots[i]);
            slotUI.UpdateUISlot();
        }
    }

    public override void UseItem(InventorySlotUI selectedSlotUI)
    {
        MouseInventoryItem.UpdateMouseSlot(selectedSlotUI.InventorySlot);
        selectedSlotUI.ClearSlot();
    }

    private void ClearSlots()
    {
        for (int i = transform.childCount-1; i>=0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        if (SlotDictionary != null) SlotDictionary.Clear();
    }
}
