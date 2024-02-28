using System.Collections.Generic;
using UnityEngine;

public class StaticInventoryDisplay : InventoryDisplay
{
    [SerializeField] private PlayerInventoryHolder playerInventoryHolder;
    [SerializeField] private InventorySlotUI[] slots;
    [SerializeField] private Transform noteDatabase;
    [field: SerializeField] public NoteItem NoteOnScreen { get; private set; }

    protected override void OnValidate()
    {
        base.OnValidate();

        noteDatabase = GameObject.FindGameObjectWithTag("ItemDatabase").transform.GetChild(0);
        playerInventoryHolder = FindAnyObjectByType<PlayerInventoryHolder>();
    }

    private void Start()
    {
        this.InventorySystem = playerInventoryHolder.PlayerInventorySystem;
        this.InventorySystem.onInventorySlotChanged += UpdateSlot;
        AssignSlots(this.InventorySystem);
    }

    protected override void AssignSlots(InventorySystem inventoryToDisplay)
    {
        SlotDictionary = new Dictionary<InventorySlotUI, InventorySlot>();

        for(int i=0; i < this.InventorySystem.InventorySlots.Count; i++)
        {
            SlotDictionary.Add(slots[i], this.InventorySystem.InventorySlots[i]);
            slots[i].Init(this.InventorySystem.InventorySlots[i]);
        }
    }

    public override void UseItem(InventorySlotUI selectedSlotUI)
    {
        if (selectedSlotUI.InventorySlot.ItemData is NoteItem noteItem)
        {
            PlayerUI.SwitchActionMap(PlayerUI.Instance.InputActions.NoteUI);
            noteDatabase.GetChild(noteItem.ID + 1).gameObject.SetActive(true);
            NoteOnScreen = noteItem;
            UIHelper.HideInventory();
            UIHelper.DisableGUI();
            noteDatabase.gameObject.SetActive(true);
        }
    }

    public void HideCurrentNote()
    {
        noteDatabase.gameObject.SetActive(false);
        noteDatabase.GetChild(NoteOnScreen.ID + 1).gameObject.SetActive(false);
        UIHelper.ShowInventory();
        UIHelper.EnableGUI();
        NoteOnScreen = null;
        PlayerUI.SwitchActionMap(PlayerUI.Instance.InputActions.Gameplay);
    }
}
