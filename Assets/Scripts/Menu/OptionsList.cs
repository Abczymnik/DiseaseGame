using UnityEngine;
using UnityEngine.EventSystems;

public class OptionsList : MonoBehaviour, IPointerClickHandler
{
    private Transform parentTrans;
    private CanvasGroup[] canvasArray;
    private ButtonHighlight thisButtonHighlight;

    private void Awake()
    {
        parentTrans = transform.parent;
        thisButtonHighlight = GetComponent<ButtonHighlight>();
    }

    private void DisableMainButtons()
    {
        canvasArray = parentTrans.GetComponentsInChildren<CanvasGroup>();
        foreach(CanvasGroup canvas in canvasArray)
        {
            canvas.alpha = 0;
            canvas.interactable = false;
            canvas.blocksRaycasts = false;
        }

        transform.GetChild(1).gameObject.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        DisableMainButtons();
        thisButtonHighlight.enabled = false;
    }
}
