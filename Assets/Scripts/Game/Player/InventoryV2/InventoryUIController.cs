using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private DynamicInventoryDisplay chestInventoryPanel;
    [SerializeField] private StaticInventoryDisplay playerInventoryPanel;

    private InputAction inventoryToggle;
    private InputAction escapeFromNoteDisplay;
    private InputAction escapeFromInventories;
    private InputAction swipeNote;

    private void OnValidate()
    {
        playerInventoryPanel = transform.GetComponentInChildren<StaticInventoryDisplay>();
        chestInventoryPanel = transform.GetComponentInChildren<DynamicInventoryDisplay>();
    }

    private void OnEnable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested += DisplayInventory;
        inventoryToggle = PlayerUI.Instance.InputActions.Gameplay.Inventory;
        inventoryToggle.performed += OnToggleInventory;
        escapeFromInventories = PlayerUI.Instance.InputActions.Gameplay.Back;
        escapeFromNoteDisplay = PlayerUI.Instance.InputActions.NoteUI.Escape;
        escapeFromNoteDisplay.performed += OnEscapeFromNoteDisplay;
        swipeNote = PlayerUI.Instance.InputActions.NoteUI.Navigation;
        swipeNote.performed += OnNoteSwipe;
    }

    private void Awake()
    {
        chestInventoryPanel.gameObject.SetActive(false);
        playerInventoryPanel.gameObject.SetActive(false);
    }

    private void OnToggleInventory(InputAction.CallbackContext _)
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

    private void OnNoteSwipe(InputAction.CallbackContext context)
    {
        Debug.Log("Event");
        int currentNoteIndexOnPanel = FindIndexCurrentNote();
        bool swipeToRight = context.ReadValue<float>() > 0;

        if (swipeToRight)
        {
            //Debug.Log("Right");
            for(int i = currentNoteIndexOnPanel; i < playerInventoryPanel.Slots.Length; i++)
            {
                if (playerInventoryPanel.Slots[i].InventorySlot.ItemData == null) continue;
                else
                {
                    playerInventoryPanel.HideCurrentNote();
                    playerInventoryPanel.UseItem(playerInventoryPanel.Slots[i]);
                }
            }
        }
        else
        {
            //Debug.Log("Left");
            for (int i = currentNoteIndexOnPanel; i >= 0; i--)
            {
                if (playerInventoryPanel.Slots[i].InventorySlot.ItemData == null) continue;
                else
                {
                    playerInventoryPanel.HideCurrentNote();
                    playerInventoryPanel.UseItem(playerInventoryPanel.Slots[i]);
                }
            }
        }
    }

    private int FindIndexCurrentNote()
    {
        for (int i = 0; i < playerInventoryPanel.Slots.Length; i++)
        {
            if (playerInventoryPanel.Slots[i].InventorySlot.ItemData == null) continue;

            if (playerInventoryPanel.Slots[i].InventorySlot.ItemData.ID == playerInventoryPanel.NoteOnScreen.ID)
            {
                return i;
            }
        }
        return 0;
    }

    private void DisplayInventory(InventorySystem inventoryToDisplay)
    {
        chestInventoryPanel.gameObject.SetActive(true);
        chestInventoryPanel.RefreshDynamicInventory(inventoryToDisplay);
    }

    private void OnDisable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested -= DisplayInventory;
        inventoryToggle.performed -= OnToggleInventory;
        escapeFromNoteDisplay.performed -= OnEscapeFromNoteDisplay;
        escapeFromInventories.performed -= OnEscapeFromInventories;
    }
}
