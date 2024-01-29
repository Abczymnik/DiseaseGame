using UnityEngine;
using UnityEngine.EventSystems;

public class StartGameButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        EventManager.TriggerEvent("ChangeLevel");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        CursorSwitch.ShowCursor();
    }
}