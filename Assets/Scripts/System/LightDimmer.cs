using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class LightDimmer : MonoBehaviour, IDimmable
{
    [SerializeField, HideInInspector] private HDAdditionalLightData thisLight;
    [field: SerializeField, HideInInspector] public float OriginalDimmableValue { get; set; }

    private void OnValidate()
    {
        thisLight = GetComponent<HDAdditionalLightData>();
        OriginalDimmableValue = thisLight.intensity;
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
        thisLight.intensity = OriginalDimmableValue - OriginalDimmableValue * dimPercentage;
    }

    public float CurrentDim()
    {
        return 1 - thisLight.intensity / OriginalDimmableValue;
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
