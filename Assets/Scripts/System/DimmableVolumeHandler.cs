using UnityEngine;
using UnityEngine.Rendering;

public class DimmableVolumeHandler : MonoBehaviour, IDimmable
{
    [SerializeField, HideInInspector] private Volume volumeIntense;
    [field: SerializeField] public float OriginalDimmableValue { get; set; }

    private void OnValidate()
    {
        volumeIntense = GetComponent<Volume>();
        OriginalDimmableValue = volumeIntense.CompareTag("DimmableSky") ? 1 : volumeIntense.weight;
    }

    private void Awake()
    {
        volumeIntense.weight = OriginalDimmableValue;
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
        volumeIntense.weight = OriginalDimmableValue * dimPercentage;
    }

    public float CurrentDim()
    {
        return volumeIntense.weight / OriginalDimmableValue;
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
