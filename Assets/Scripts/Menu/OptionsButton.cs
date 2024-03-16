using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class OptionsButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IDimmable
{
    [SerializeField, HideInInspector] private CanvasGroup[] mainButtonsCanvas;
    [SerializeField] private CanvasGroup thisCanvas;

    [field: SerializeField] public float OriginalDim { get; set; }

    private void OnValidate()
    {
        thisCanvas = GetComponent<CanvasGroup>();
        OriginalDim = thisCanvas.alpha;
        mainButtonsCanvas = transform.parent.GetComponentsInChildren<CanvasGroup>();
    }

    private void OnEnable()
    {
        AnnounceEnable();
    }

    public void AnnounceEnable()
    {
        EventManager.TriggerEvent(TypedEventName.NewDimmable, this);
    }

    public void Dim(float dimPercentage)
    {
        thisCanvas.alpha = OriginalDim - OriginalDim * dimPercentage;
    }

    public float CurrentDim()
    {
        return OriginalDim - thisCanvas.alpha / OriginalDim;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        DisableMainButtons();
        EnableOptionsList();
    }

    private void DisableMainButtons()
    {
        foreach(CanvasGroup buttonCanvas in mainButtonsCanvas)
        {
            buttonCanvas.alpha = 0;
            buttonCanvas.interactable = false;
            buttonCanvas.blocksRaycasts = false;
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
    
    public void AnnounceDisable()
    {
        EventManager.TriggerEvent(TypedEventName.RemoveDimmable, this);
    }

    private void OnDisable()
    {
        AnnounceDisable();
    }
}