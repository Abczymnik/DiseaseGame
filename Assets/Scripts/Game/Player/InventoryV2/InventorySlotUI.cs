using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image itemSprite; //Assigned in editor
    [SerializeField] private TextMeshProUGUI itemCount; //Assigned in editor
    [field: SerializeField] public InventorySlot InventorySlot { get; private set; }

    private void Awake()
    {
        ClearSlot();
    }

    public void Init(InventorySlot slot)
    {
        this.InventorySlot = slot;
        UpdateUISlot(slot);
    }

    public void UpdateUISlot(InventorySlot slot)
    {
        if (slot.ItemData != null)
        {
            itemSprite.sprite = slot.ItemData.Icon;
            itemSprite.color = Color.white;

            if (slot.StackSize > 1) itemCount.text = slot.StackSize.ToString();
            else itemCount.text = "";
        }
        else ClearSlot();
    }

    public void UpdateUISlot()
    {
        if (this.InventorySlot != null) UpdateUISlot(this.InventorySlot);
    }

    public void ClearSlot()
    {
        if (this.InventorySlot == null) this.InventorySlot.ClearSlot();

        itemSprite.sprite = null;
        itemSprite.color = Color.clear;
        itemCount.text = "";
    }
}
