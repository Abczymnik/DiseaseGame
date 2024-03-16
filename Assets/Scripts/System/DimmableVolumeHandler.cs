using UnityEngine;
using UnityEngine.Rendering;

public class DimmableVolumeHandler : MonoBehaviour, IDimmable
{
    [SerializeField, HideInInspector] private Volume skyLightIntense;
    [field: SerializeField] public float OriginalDim { get; set; }

    private void OnValidate()
    {
        skyLightIntense = GetComponent<Volume>();
        OriginalDim = skyLightIntense.weight;
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
        skyLightIntense.weight = OriginalDim * dimPercentage;
    }

    public float CurrentDim()
    {
        return skyLightIntense.weight / OriginalDim;
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
