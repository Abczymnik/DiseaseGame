using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private ScreenResolutionManager screenResolutionManager;

    private void OnValidate()
    {
        screenResolutionManager = GetComponentInChildren<ScreenResolutionManager>();
    }

    private void OnEnable()
    {
        CursorSwitch.ShowCursor();
        CursorSwitch.SwitchSkin(CursorName.Options);
    }

    public void SetResolution(int resolutionIndex)
    {
        screenResolutionManager.SetScreenResolution(resolutionIndex);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    private void OnDisable()
    {
        CursorSwitch.HideCursor();
        CursorSwitch.SwitchSkin(CursorName.Standard);
    }
}