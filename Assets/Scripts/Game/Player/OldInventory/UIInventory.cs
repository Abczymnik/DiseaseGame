using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    private List<UIItem> uIItems = new List<UIItem>();
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotPanel;

    public int NumberOfSlots { get; private set; } = 16;

    private void Awake()
    {
        for(int i = 0; i < NumberOfSlots; i++)
        {
            GameObject instance = Instantiate(slotPrefab);
            instance.transform.SetParent(slotPanel);
            uIItems.Add(instance.GetComponentInChildren<UIItem>());
        }
    }

    public void UpdateSlot(int slot, Item item)
    {
        uIItems[slot].UpdateItem(item);
    }

    public void AddNewItem(Item item)
    {
        UpdateSlot(uIItems.FindIndex(i => i.Item == null), item);
    }

    public void RemoveItem(Item item)
    {
        UpdateSlot(uIItems.FindIndex(i => i.Item == item), null);
    }
}
