using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    [field:SerializeField] public InventoryTestItem ItemData { get; private set; }
    [field:SerializeField] public int StackSize { get; private set; }

    public InventorySlot(InventoryTestItem item, int amount)
    {
        ItemData = item;
        StackSize = amount;
    }

    public InventorySlot()
    {
        ClearSlot();
    }

    public void ClearSlot()
    {
        ItemData = null;
        StackSize = -1;
    }

    public void AddToStack(int amount)
    {
        StackSize += amount;
    }

    public void RemoveFromStack(int amount)
    {
        StackSize -= amount;
    }

    public bool RoomLeftInStack(int amountToAdd, out int amountRemaining)
    {
        amountRemaining = ItemData.MaxStackSize - StackSize;

        return RoomLeftInStack(amountToAdd);
    }

    public bool RoomLeftInStack(int amountToAdd)
    {
        if (StackSize + amountToAdd > ItemData.MaxStackSize) return false;
        return true;
    }

    public void UpdateInventorySlot(InventoryTestItem itemData, int amount)
    {
        ItemData = itemData;
        StackSize = amount;
    }
}
