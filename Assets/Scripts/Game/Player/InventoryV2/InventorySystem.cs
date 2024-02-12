using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySystem
{
    [field: SerializeField] public List<InventorySlot> InventorySlots { get; private set; }

    public InventorySystem(int size)
    {
        InventorySlots = new List<InventorySlot>(size);

        for (int i=0; i < size; i++)
        {
            InventorySlots.Add(new InventorySlot());
        }
    }

    public void AddToInventory(InventoryTestItem itemToAdd, int amoutToAdd)
    {
        InventorySlots[0] = new InventorySlot(itemToAdd, amoutToAdd);
    }
}
