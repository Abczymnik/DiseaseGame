using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private DynamicInventoryDisplay chestInventoryPanel;
    [SerializeField] private StaticInventoryDisplay playerInventoryPanel;

    private InputAction inventoryToggleInput;
    private InputAction escapeFromNoteDisplay;
    private InputAction escapeFromInventories;

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
        escapeFromInventories = PlayerUI.Instance.InputActions.Gameplay.Back;
        escapeFromNoteDisplay = PlayerUI.Instance.InputActions.NoteUI.Escape;
        escapeFromNoteDisplay.performed += OnEscapeFromNoteDisplay;
    }

    private void Awake()
    {
        chestInventoryPanel.gameObject.SetActive(false);
        playerInventoryPanel.gameObject.SetActive(false);
    }

    private void OnToggleInventoryInput(InputAction.CallbackContext _)
    {
        if (playerInventoryPanel.gameObject.activeInHierarchy)
        {
            playerInventoryPanel.MouseInventoryItem.Tooltip.ClearTooltip();
            playerInventoryPanel.gameObject.SetActive(false);
            escapeFromInventories.performed -= OnEscapeFromInventories;
        }
        else
        {
            playerInventoryPanel.gameObject.SetActive(true);
            escapeFromInventories.performed += OnEscapeFromInventories;
        }
    }

    private void OnEscapeFromInventories(InputAction.CallbackContext _)
    {
        playerInventoryPanel.MouseInventoryItem.Tooltip.ClearTooltip();
        playerInventoryPanel.gameObject.SetActive(false);
        chestInventoryPanel.gameObject.SetActive(false);
    }

    private void OnEscapeFromNoteDisplay(InputAction.CallbackContext _)
    {
        playerInventoryPanel.HideCurrentNote();
        playerInventoryPanel.MouseInventoryItem.Tooltip.ClearTooltip();
        PlayerUI.SwitchActionMap(PlayerUI.Instance.InputActions.Gameplay);
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
        escapeFromNoteDisplay.performed -= OnEscapeFromNoteDisplay;
        escapeFromInventories.performed -= OnEscapeFromInventories;
    }
}
