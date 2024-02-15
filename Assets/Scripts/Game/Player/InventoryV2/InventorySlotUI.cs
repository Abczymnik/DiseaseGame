using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image itemSprite;
    [SerializeField] private TextMeshProUGUI itemCount;
    [field: SerializeField] public InventorySlot InventorySlot { get; private set; }
    [field: SerializeField] public InventoryDisplay ParentDisplay { get; private set; }

    private void OnValidate()
    {
        itemCount = GetComponentInChildren<TextMeshProUGUI>();
        itemSprite = transform.GetChild(0).GetComponent<Image>();
    }

    private void Awake()
    {
        ParentDisplay = transform.parent.GetComponent<InventoryDisplay>();
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
            itemSprite.material = slot.ItemData.Icon;
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

        itemSprite.color = Color.clear;
        itemCount.text = "";
    }

    public void OnUISlotClick()
    {
        ParentDisplay.SlotClicked(this);
    }
}
