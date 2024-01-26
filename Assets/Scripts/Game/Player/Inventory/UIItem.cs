using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UIItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Item Item { get; private set; }
    public Image SpriteImage { get; private set; }
    [SerializeField] private Tooltip toolTip;
    [SerializeField] private Transform dataBaseNotes;
    [SerializeField] private Inventory playerInventory;

    private int lastEnabledNoteIndex;
    private Image scrollImage;

    private InputAction escape;
    private InputAction navigation;

    private void Awake()
    {
        SpriteImage = GetComponent<Image>();
        scrollImage = transform.GetChild(0).GetComponent<Image>();
        UpdateItem(null);

        escape = PlayerUI.inputActions.NoteUI.Escape;
        navigation = PlayerUI.inputActions.NoteUI.Navigation;
    }

    private void Start()
    {
        toolTip = GameObject.Find("/Player/Inventory/InventoryPanel/Tooltip").GetComponent<Tooltip>();
        dataBaseNotes = GameObject.Find("ItemDatabase").transform.GetChild(0);
        playerInventory = GameObject.Find("/Player/Inventory").GetComponent<Inventory>();
    }

    private void NavigationPerformed(InputAction.CallbackContext context)
    {
        int ownedNotes = playerInventory.OwnedNotes;
        int swipeNoteDir = (int)context.ReadValue<float>();

        if (lastEnabledNoteIndex + swipeNoteDir < 1 || lastEnabledNoteIndex + swipeNoteDir > ownedNotes) return;

        dataBaseNotes.GetChild(lastEnabledNoteIndex).gameObject.SetActive(false);
        lastEnabledNoteIndex += swipeNoteDir;
        dataBaseNotes.GetChild(lastEnabledNoteIndex).gameObject.SetActive(true);
    }

    private void EscapePerformed(InputAction.CallbackContext context)
    {
        dataBaseNotes.GetChild(lastEnabledNoteIndex).gameObject.SetActive(false);
        dataBaseNotes.gameObject.SetActive(false);
        playerInventory.InventoryVisibility();
        UIHelper.EnableGUI();
        navigation.performed -= NavigationPerformed;
        escape.performed -= EscapePerformed;
        PlayerUI.SwitchActionMap(PlayerUI.inputActions.Gameplay);
    }

    public void UpdateItem(Item item)
    {
        this.Item = item;
        if(item != null)
        {
            scrollImage.enabled = true;
        }
        else
        {
            scrollImage.enabled = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Item != null)
        {
            toolTip.GenerateTooltip(Item);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTip.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Item == null) return;
        lastEnabledNoteIndex = Item.Id + 1;
        dataBaseNotes.gameObject.SetActive(true);
        dataBaseNotes.GetChild(lastEnabledNoteIndex).gameObject.SetActive(true);
        playerInventory.InventoryVisibility();
        UIHelper.DisableGUI();
        PlayerUI.SwitchActionMap(PlayerUI.inputActions.NoteUI);
        escape.performed += EscapePerformed;
        navigation.performed += NavigationPerformed;
    }

    private void OnDisable()
    {
        escape.performed -= EscapePerformed;
        navigation.performed -= NavigationPerformed;
    }
}
