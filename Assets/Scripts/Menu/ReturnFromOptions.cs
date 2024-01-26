using UnityEngine;
using UnityEngine.EventSystems;

public class ReturnFromOptions : MonoBehaviour, IPointerClickHandler
{
    private Transform mainMenuTrans;
    private Transform optionsButton;
    private Transform optionsTrans;
    private CanvasGroup[] canvasArray;

    private void Awake()
    {
        optionsTrans = transform.parent;
        optionsButton = optionsTrans.parent;
        mainMenuTrans = optionsButton.parent;
    }

    private void OnEnable()
    {
        CursorSwitch.ShowCursor();
    }

    private void EnableMainButtons()
    {
        canvasArray = mainMenuTrans.GetComponentsInChildren<CanvasGroup>();
        foreach (CanvasGroup canvas in canvasArray)
        {
            canvas.alpha = 1;
            canvas.interactable = true;
            canvas.blocksRaycasts = true;
        }

        optionsTrans.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EnableMainButtons();
        CursorSwitch.HideCursor();
        optionsButton.GetComponent<ButtonHighlight>().enabled = true;
    }
}
