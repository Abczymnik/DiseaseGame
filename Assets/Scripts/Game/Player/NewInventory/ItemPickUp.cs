using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider))]
public class ItemPickUp : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private BaseItem itemData;

    public void OnPointerEnter(PointerEventData eventData)
    {
        CursorSwitch.SwitchSkin(CursorName.Note);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CursorSwitch.SwitchSkin(CursorName.Standard);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PlayerInventoryHolder inventory))
        {
            inventory.PlayerInventorySystem.AddToInventory(itemData, 1);
            CursorSwitch.SwitchSkin(CursorName.Standard);
            Destroy(this.gameObject);
        }
    }
}
