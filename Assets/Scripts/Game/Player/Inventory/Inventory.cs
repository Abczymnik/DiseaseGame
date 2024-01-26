using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private List<Item> playerNotes = new List<Item>();
    private UIInventory inventoryUI;
    private Transform inventoryPanel;
    private bool isInventoryEnabled;

    private UnityAction onNewNoteOnMap;

    private InputAction inventorySwitch;
    public int NotesOnMap { get; set; }
    public ItemDatabase ItemDatabase { get; private set; }

    private int _ownedNotes;
    public int OwnedNotes
    {
        get { return _ownedNotes; }
        private set
        {
            if (value < _ownedNotes) NotesOnMap--;
            _ownedNotes = value;
        }
    }

    private void Awake()
    {
        inventorySwitch = PlayerUI.inputActions.Gameplay.Inventory;
        inventorySwitch.performed += InventoryPerformed;
    }

    private void OnEnable()
    {
        onNewNoteOnMap += OnNewNoteOnMap;
        EventManager.StartListening("NewNoteOnMap", onNewNoteOnMap);
    }

    private void Start()
    {
        if (ItemDatabase == null) ItemDatabase = GameObject.Find("ItemDatabase").GetComponent<ItemDatabase>();
        inventoryUI = GetComponent<UIInventory>();
        inventoryPanel = transform.GetChild(0);

        GiveItem(0);
        GiveItem(1);
        GiveItem(2);
        NotesOnMap += 3;
    }

    private void InventoryPerformed(InputAction.CallbackContext context)
    {
        InventoryVisibility();
    }

    public void GiveItem()
    {
        Item itemToAdd = ItemDatabase.GetItem(OwnedNotes);
        playerNotes.Add(itemToAdd);
        inventoryUI.AddNewItem(itemToAdd);
        OwnedNotes++;
    }

    public void GiveItem(int id)
    {
        Item itemToAdd = ItemDatabase.GetItem(id);
        playerNotes.Add(itemToAdd);
        inventoryUI.AddNewItem(itemToAdd);
        OwnedNotes++;
    }

    public void GiveItem(string itemName)
    {
        Item itemToAdd = ItemDatabase.GetItem(itemName);
        playerNotes.Add(itemToAdd);
        inventoryUI.AddNewItem(itemToAdd);
        OwnedNotes++;
    }

    public Item CheckForItem(int id)
    {
        return playerNotes.Find(item => item.Id == id);
    }

    public void RemoveItem(int id)
    {
        Item itemToRemove = CheckForItem(id);
        if (itemToRemove != null)
        {
            playerNotes.Remove(itemToRemove);
            inventoryUI.RemoveItem(itemToRemove);
            OwnedNotes--;
        }
    }

    public void InventoryVisibility()
    {
        isInventoryEnabled = !isInventoryEnabled;
        inventoryPanel.GetComponent<Image>().enabled = isInventoryEnabled;
        inventoryPanel.GetChild(0).gameObject.SetActive(isInventoryEnabled);

        if (isInventoryEnabled) CursorSwitch.SwitchSkin("Note");
        else
        {
            UIHelper.EnableGUI();
            CursorSwitch.SwitchSkin("Standard");
            inventoryPanel.GetChild(1).gameObject.SetActive(isInventoryEnabled);
        }
    }

    private void OnNewNoteOnMap()
    {
        NotesOnMap++;
    }

    private void OnDisable()
    {
        inventorySwitch.performed -= InventoryPerformed;
        EventManager.StopListening("NewNoteOnMap", onNewNoteOnMap);
    }
}
