using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private DynamicInventoryDisplay inventoryPanel;

    private void OnValidate()
    {
        inventoryPanel = transform.GetComponentInChildren<DynamicInventoryDisplay>();
    }

    private void OnEnable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested += DisplayInventory;
    }

    private void Awake()
    {
        inventoryPanel.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (inventoryPanel.gameObject.activeInHierarchy && Keyboard.current.escapeKey.wasPressedThisFrame) inventoryPanel.gameObject.SetActive(false);
    }

    private void DisplayInventory(InventorySystem inventoryToDisplay)
    {
        inventoryPanel.gameObject.SetActive(true);
        inventoryPanel.RefreshDynamicInventory(inventoryToDisplay);
    }

    private void OnDisable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested -= DisplayInventory;
    }
}
