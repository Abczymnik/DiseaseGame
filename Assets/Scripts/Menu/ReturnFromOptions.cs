using UnityEngine;
using UnityEngine.EventSystems;

public class ReturnFromOptions : MonoBehaviour, IPointerClickHandler
{
    private CanvasGroup[] buttonsArray;

    private void Start()
    {
        buttonsArray = transform.root.GetComponentsInChildren<CanvasGroup>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        DisableOptionsList();
        EnableMainButtons();
        CursorSwitch.HideCursor();
    }

    private void DisableOptionsList()
    {
        transform.parent.gameObject.SetActive(false);
    }

    private void EnableMainButtons()
    {
        foreach (CanvasGroup button in buttonsArray)
        {
            button.alpha = 1;
            button.interactable = true;
            button.blocksRaycasts = true;
        }
    }
}
