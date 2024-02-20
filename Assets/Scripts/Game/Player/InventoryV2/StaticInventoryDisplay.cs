using System.Collections;
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

    protected override void Start()
    {
        if (playerInventoryHolder != null)
        {
            this.InventorySystem = playerInventoryHolder.PlayerInventorySystem;
            this.InventorySystem.onInventorySlotChanged += UpdateSlot;
        }
        else Debug.Log("No inventory");

        AssignSlots(this.InventorySystem);
    }

    public override void AssignSlots(InventorySystem inventoryToDisplay)
    {
        SlotDictionary = new Dictionary<InventorySlotUI, InventorySlot>();

        for(int i=0; i < this.InventorySystem.InventorySlots.Count; i++)
        {
            SlotDictionary.Add(slots[i], this.InventorySystem.InventorySlots[i]);
            slots[i].Init(this.InventorySystem.InventorySlots[i]);
        }
    }
}
