using UnityEngine;
using UnityEngine.EventSystems;

public class CursorTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        CursorSwitch.SwitchSkin("Attack");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CursorSwitch.SwitchSkin("Standard");
    }
}
