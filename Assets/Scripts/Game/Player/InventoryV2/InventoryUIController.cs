using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private DynamicInventoryDisplay chestInventoryPanel;
    [SerializeField] private StaticInventoryDisplay playerInventoryPanel;

    private void OnValidate()
    {
        playerInventoryPanel = transform.GetComponentInChildren<StaticInventoryDisplay>();
        chestInventoryPanel = transform.GetComponentInChildren<DynamicInventoryDisplay>();
    }

    private void OnEnable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested += DisplayInventory;
    }

    private void Awake()
    {
        chestInventoryPanel.gameObject.SetActive(false);
        playerInventoryPanel.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (chestInventoryPanel.gameObject.activeInHierarchy && Keyboard.current.escapeKey.wasPressedThisFrame) chestInventoryPanel.gameObject.SetActive(false);
        if (Keyboard.current.iKey.wasPressedThisFrame) playerInventoryPanel.gameObject.SetActive(!playerInventoryPanel.gameObject.activeInHierarchy);
    }

    private void DisplayInventory(InventorySystem inventoryToDisplay)
    {
        chestInventoryPanel.gameObject.SetActive(true);
        chestInventoryPanel.RefreshDynamicInventory(inventoryToDisplay);
    }

    private void OnDisable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested -= DisplayInventory;
    }
}
