using UnityEngine;
using UnityEngine.EventSystems;

public class PickUpItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private Tooltip toolTip;
    private Item item;

    private void Start()
    {
        if (inventory == null) inventory = GameObject.Find("/Player/Inventory").GetComponent<Inventory>();
        if (toolTip == null) toolTip = GameObject.Find("/Player/Inventory/InventoryPanel/Tooltip").GetComponent<Tooltip>();
        item = inventory.ItemDatabase.GetItem(inventory.NotesOnMap - 1);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null) toolTip.GenerateTooltip(item);

        CursorSwitch.SwitchSkin("Note");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTip.gameObject.SetActive(false);
        CursorSwitch.SwitchSkin("Standard");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        toolTip.gameObject.SetActive(false);
        PickUp();
        CursorSwitch.SwitchSkin("Standard");
    }

    private void PickUp()
    {
        inventory.GiveItem(item.Id);
        transform.parent.gameObject.SetActive(false);
    }
}
