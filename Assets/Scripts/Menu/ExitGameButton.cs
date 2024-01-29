using UnityEngine;
using UnityEngine.EventSystems;

public class ExitGameButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Application.Quit();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        CursorSwitch.ShowCursor();
    }
}