using System.Collections.Generic;
using UnityEngine;

public class StaticInventoryDisplay : InventoryDisplay
{
    [SerializeField] private PlayerInventoryHolder playerInventoryHolder;
    [SerializeField] private InventorySlotUI[] slots;

    protected override void OnValidate()
    {
        base.OnValidate();

        playerInventoryHolder = FindAnyObjectByType<PlayerInventoryHolder>();
    }

    private void Start()
    {
        this.InventorySystem = playerInventoryHolder.PlayerInventorySystem;
        this.InventorySystem.onInventorySlotChanged += UpdateSlot;
        AssignSlots(this.InventorySystem);
    }

    protected override void AssignSlots(InventorySystem inventoryToDisplay)
    {
        SlotDictionary = new Dictionary<InventorySlotUI, InventorySlot>();

        for(int i=0; i < this.InventorySystem.InventorySlots.Count; i++)
        {
            SlotDictionary.Add(slots[i], this.InventorySystem.InventorySlots[i]);
            slots[i].Init(this.InventorySystem.InventorySlots[i]);
        }
    }
}
