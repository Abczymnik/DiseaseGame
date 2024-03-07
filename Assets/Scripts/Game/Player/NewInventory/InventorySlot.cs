using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    [field:SerializeField] public BaseItem ItemData { get; private set; }
    [field:SerializeField] public int StackSize { get; private set; }

    public InventorySlot(BaseItem item, int amount)
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

    public void AssignItem(InventorySlot inventorySlot)
    {
        if (ItemData == inventorySlot.ItemData) AddToStack(inventorySlot.StackSize);
        else
        {
            ItemData = inventorySlot.ItemData;
            StackSize = inventorySlot.StackSize;
        }
    }

    public void AddToStack(int amount)
    {
        StackSize += amount;
    }

    public void RemoveFromStack(int amount)
    {
        StackSize -= amount;
    }

    public bool IsEnoughRoomAvailable(int amountToAdd, out int amountRemaining)
    {
        amountRemaining = ItemData.MaxStackSize - StackSize;

        return IsEnoughRoomAvailable(amountToAdd);
    }

    public bool IsEnoughRoomAvailable(int amountToAdd)
    {
        if (StackSize + amountToAdd > ItemData.MaxStackSize) return false;
        return true;
    }

    public bool SplitStack(out InventorySlot newStack)
    {
        if (StackSize <= 1)
        {
            newStack = null;
            return false;
        }

        int halfStack = Mathf.RoundToInt(StackSize / 2);
        RemoveFromStack(halfStack);
        newStack = new InventorySlot(ItemData, halfStack);
        return true;
    }

    public void UpdateInventorySlot(BaseItem itemData, int amount)
    {
        ItemData = itemData;
        StackSize = amount;
    }
}
