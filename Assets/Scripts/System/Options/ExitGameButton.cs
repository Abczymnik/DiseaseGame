using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class ExitGameButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IDimmable
{
    [SerializeField] private CanvasGroup thisCanvas;
    [field: SerializeField] public float OriginalDimmableValue { get; set; }

    private void OnValidate()
    {
        thisCanvas = GetComponent<CanvasGroup>();
        OriginalDimmableValue = thisCanvas.alpha;
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
        thisCanvas.alpha = OriginalDimmableValue - OriginalDimmableValue * dimPercentage;
    }

    public float CurrentDim()
    {
        return 1 - thisCanvas.alpha / OriginalDimmableValue;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Application.Quit();
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