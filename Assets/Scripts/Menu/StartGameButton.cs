using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class StartGameButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IDimmable
{
    [SerializeField] private CanvasGroup thisCanvas;
    [field: SerializeField] public float OriginalDim { get; set; }

    private void OnValidate()
    {
        thisCanvas = GetComponent<CanvasGroup>();
        OriginalDim = thisCanvas.alpha;
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
        EventManager.TriggerEvent(UnityEventName.ChangeLevel);
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