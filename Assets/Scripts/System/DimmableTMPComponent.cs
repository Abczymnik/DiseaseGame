using TMPro;
using UnityEngine;

public class DimmableTMPComponent : MonoBehaviour, IDimmable
{
    public float OriginalDimmableValue { get; set; }

    [SerializeField, HideInInspector] private TextMeshProUGUI thisText;
    private Color startColor;

    private void OnValidate()
    {
        thisText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Awake()
    {
        startColor = thisText.color;
        OriginalDimmableValue = startColor.a;
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
        Color newColor = thisText.color;
        newColor.a = OriginalDimmableValue - OriginalDimmableValue * dimPercentage;
        thisText.color = newColor;
    }

    public float CurrentDim()
    {
        return 1 - thisText.color.a / OriginalDimmableValue;
    }

    public void AnnounceDisable()
    {
        EventManager.TriggerEvent(TypedEventName.RemoveDimmable, this);
    }

    private void OnDisable()
    {
        thisText.color = startColor;
        AnnounceDisable();
    }
}
