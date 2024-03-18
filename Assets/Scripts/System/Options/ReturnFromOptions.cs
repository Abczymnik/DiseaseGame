using UnityEngine;
using UnityEngine.EventSystems;

public class ReturnFromOptions : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private CanvasGroup[] mainButtonsCanvas;

    private void OnValidate()
    {
        mainButtonsCanvas = transform.root.GetComponentsInChildren<CanvasGroup>();
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
        foreach (CanvasGroup buttonCanvas in mainButtonsCanvas)
        {
            buttonCanvas.interactable = true;
            buttonCanvas.blocksRaycasts = true;

            if (buttonCanvas.TryGetComponent(out IDimmable dimmableButton))
            {
                buttonCanvas.alpha = dimmableButton.OriginalDimmableValue;
            }
            else buttonCanvas.alpha = 1;
        }
    }
}
