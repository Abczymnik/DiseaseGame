using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private DynamicInventoryDisplay chestInventoryPanel;
    [SerializeField] private StaticInventoryDisplay playerInventoryPanel;

    private InputAction inventoryToggleInput;
    private InputAction escapeFromInventoriesInput;

    private void OnValidate()
    {
        playerInventoryPanel = transform.GetComponentInChildren<StaticInventoryDisplay>();
        chestInventoryPanel = transform.GetComponentInChildren<DynamicInventoryDisplay>();
    }

    private void OnEnable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested += DisplayInventory;
        inventoryToggleInput = PlayerUI.Instance.InputActions.Gameplay.Inventory;
        inventoryToggleInput.performed += OnToggleInventoryInput;
        escapeFromInventoriesInput = PlayerUI.Instance.InputActions.Gameplay.Back;
        escapeFromInventoriesInput.performed += OnEscapeFromInventoriesInput;
    }

    private void Awake()
    {
        chestInventoryPanel.gameObject.SetActive(false);
        playerInventoryPanel.gameObject.SetActive(false);
    }

    private void OnEscapeFromInventoriesInput(InputAction.CallbackContext _)
    {
        playerInventoryPanel.gameObject.SetActive(false);
        chestInventoryPanel.gameObject.SetActive(false);
    }

    private void OnToggleInventoryInput(InputAction.CallbackContext _)
    {
        playerInventoryPanel.gameObject.SetActive(!playerInventoryPanel.gameObject.activeInHierarchy);
    }

    private void DisplayInventory(InventorySystem inventoryToDisplay)
    {
        chestInventoryPanel.gameObject.SetActive(true);
        chestInventoryPanel.RefreshDynamicInventory(inventoryToDisplay);
    }

    private void OnDisable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested -= DisplayInventory;
        inventoryToggleInput.performed -= OnToggleInventoryInput;
        escapeFromInventoriesInput.performed -= OnEscapeFromInventoriesInput;
    }
}
