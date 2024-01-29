using UnityEngine;
using UnityEngine.EventSystems;

public class OptionsButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    private CanvasGroup[] buttonsArray;

    private void Start()
    {
        buttonsArray = transform.parent.GetComponentsInChildren<CanvasGroup>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        DisableMainButtons();
        EnableOptionsList();
    }

    private void DisableMainButtons()
    {
        foreach(CanvasGroup button in buttonsArray)
        {
            button.alpha = 0;
            button.interactable = false;
            button.blocksRaycasts = false;
        }
    }

    private void EnableOptionsList()
    {
        transform.GetChild(1).gameObject.SetActive(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        CursorSwitch.ShowCursor();
    }
}