using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InventorySystem
{
    [field: SerializeField] public List<InventorySlot> InventorySlots { get; private set; }

    public UnityAction<InventorySlot> onInventorySlotChanged;

    public InventorySystem(int size)
    {
        InventorySlots = new List<InventorySlot>(size);

        for (int i=0; i < size; i++)
        {
            InventorySlots.Add(new InventorySlot());
        }
    }

    public void AddToInventory(InventoryTestItem itemToAdd, int amountToAdd)
    {
        if(ContainsItem(itemToAdd, out List<InventorySlot> inventorySlots))
        {
            foreach(InventorySlot inventorySlot in inventorySlots)
            {
                if(inventorySlot.IsEnoughRoomAvailable(amountToAdd))
                {
                    inventorySlot.AddToStack(amountToAdd);
                    onInventorySlotChanged?.Invoke(inventorySlot);
                    return;
                }
            }
        }

        if(HasFreeSlot(out InventorySlot freeSlot))
        {
            freeSlot.UpdateInventorySlot(itemToAdd, amountToAdd);
            onInventorySlotChanged?.Invoke(freeSlot);
        }
    }

    public bool ContainsItem(InventoryTestItem itemToAdd, out List<InventorySlot> inventorySlots)
    {
        inventorySlots = new List<InventorySlot>();
        foreach(InventorySlot inventorySlot in InventorySlots)
        {
            if (inventorySlot.ItemData == itemToAdd) inventorySlots.Add(inventorySlot);
        }

        if (inventorySlots.Count > 0) return true;
        return false;
    }

    public bool HasFreeSlot(out InventorySlot freeSlot)
    {
        foreach(InventorySlot inventorySlot in InventorySlots)
        {
            if (inventorySlot.ItemData == null)
            {
                freeSlot = inventorySlot;
                return true;
            }
        }
        freeSlot = null;
        return false;
    }
}
