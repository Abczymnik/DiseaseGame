using UnityEngine;
using UnityEngine.UI;

public class DimmableMaterialComponent : MonoBehaviour, IDimmable
{
    public float OriginalDimmableValue { get; set; }

    [SerializeField] private Image thisImage;
    private Color startColor;
    private int materialColorPropertyID;

    private void OnValidate()
    {
        thisImage = GetComponent<Image>();
    }

    private void Awake()
    {
        materialColorPropertyID = Shader.PropertyToID("_UnlitColor");
        startColor = thisImage.material.GetColor(materialColorPropertyID);
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
        Color newColor = thisImage.material.GetColor(materialColorPropertyID);
        newColor.a = OriginalDimmableValue - OriginalDimmableValue * dimPercentage;
        thisImage.material.SetColor(materialColorPropertyID, newColor);
    }

    public float CurrentDim()
    {
        return 1 - thisImage.material.GetColor(materialColorPropertyID).a / OriginalDimmableValue;
    }

    public void AnnounceDisable()
    {
        EventManager.TriggerEvent(TypedEventName.RemoveDimmable, this);
    }

    private void OnDisable()
    {
        thisImage.material.SetColor(materialColorPropertyID, startColor);
        AnnounceDisable();
    }
}
