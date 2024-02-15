using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticInventoryDisplay : InventoryDisplay
{
    [SerializeField] private InventoryHolder inventoryHolder;
    [SerializeField] private InventorySlotUI[] slots;

    protected override void Start()
    {
        base.Start();

        if (inventoryHolder != null)
        {
            this.InventorySystem = inventoryHolder.InventorySystem;
            this.InventorySystem.onInventorySlotChanged += UpdateSlot;
        }
        else Debug.Log("No inventory");

        AssignSlot(this.InventorySystem);
    }

    public override void AssignSlot(InventorySystem inventoryToDisplay)
    {
        SlotDictionary = new Dictionary<InventorySlotUI, InventorySlot>();

        for(int i=0; i < this.InventorySystem.InventorySlots.Count; i++)
        {
            SlotDictionary.Add(slots[i], this.InventorySystem.InventorySlots[i]);
            slots[i].Init(this.InventorySystem.InventorySlots[i]);
        }
    }

}
