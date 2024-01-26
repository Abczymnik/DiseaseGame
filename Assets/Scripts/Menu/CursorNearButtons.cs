using UnityEngine;
using UnityEngine.EventSystems;

public class CursorNearButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        CursorSwitch.ShowCursor();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CursorSwitch.HideCursor();
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }
}
