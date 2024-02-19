using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicInventoryDisplay : InventoryDisplay
{
    [SerializeField] protected InventorySlotUI slotPrefab;

    protected override void Start()
    {
        base.Start();
    }

    public void RefreshDynamicInventory(InventorySystem inventoryToDisplay)
    {
        ClearSlots();
        this.InventorySystem = inventoryToDisplay;
        AssignSlot(inventoryToDisplay);
    }

    public override void AssignSlot(InventorySystem inventoryToDisplay)
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

    public void ClearSlots()
    {
        for (int i = transform.childCount-1; i>=0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        if (SlotDictionary != null) SlotDictionary.Clear();
    }
}
