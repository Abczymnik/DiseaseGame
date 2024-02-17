using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MouseItemData : MonoBehaviour
{
    [field: SerializeField] public Image ItemSprite { get; private set; }
    [field: SerializeField] public TextMeshProUGUI ItemCount { get; private set; }
    [field: SerializeField] public InventorySlot InventorySlot { get; private set; }

    private void OnValidate()
    {
        ItemSprite = GetComponentInChildren<Image>();
        ItemCount = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Awake()
    {
        ItemSprite.color = Color.clear;
        ItemCount.text = "";
    }

    public void UpdateMouseSlot(InventorySlot inventorySlot)
    {
        this.InventorySlot.AssignItem(inventorySlot);
        ItemSprite.material = inventorySlot.ItemData.Icon;
        ItemCount.text = inventorySlot.StackSize.ToString();
        ItemSprite.color = Color.white;
    }

    public void ClearSlot()
    {
        this.InventorySlot.ClearSlot();
        ItemCount.text = "";
        ItemSprite.color = Color.clear;
        ItemSprite.material = null;
    }

    private void LateUpdate()
    {
        if(InventorySlot.ItemData != null)
        {
            transform.localPosition = Mouse.current.position.ReadValue();

            if(Mouse.current.leftButton.wasPressedThisFrame && !UIHelper.IsPointerOverUI("InventoryUI"))
            {
                ClearSlot();
            }
        }
    }

}


