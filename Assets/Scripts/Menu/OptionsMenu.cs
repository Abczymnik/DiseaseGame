using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    private void OnEnable()
    {
        CursorSwitch.ShowCursor();
        CursorSwitch.SwitchSkin(CursorName.Options);
    }

    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    private void OnDisable()
    {
        CursorSwitch.HideCursor();
        CursorSwitch.SwitchSkin(CursorName.Standard);
    }
}