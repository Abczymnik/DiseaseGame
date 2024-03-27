using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct ScreenResolution
{
    public readonly int width;
    public readonly int height;

    public ScreenResolution(int width, int height)
    {
        this.width = width;
        this.height = height;
    }
}

public class ScreenResolutionManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown thisDropdown;
    public ScreenResolution[] AvailableResolutions { get; private set; }

    public static UnityAction<ScreenResolution> onScreenResolutionChange;

    private void OnValidate()
    {
        thisDropdown = GetComponent<TMP_Dropdown>();
    }

    private void Awake()
    {
        SetBackendAvailableScreenResolutions();
        SetResolutionLabel();
    }

    public void SetScreenResolution(int resolutionIndex)
    {
        Screen.SetResolution(AvailableResolutions[resolutionIndex].width, AvailableResolutions[resolutionIndex].height, true);
        onScreenResolutionChange?.Invoke(AvailableResolutions[resolutionIndex]);
    }

    private void SetResolutionLabel()
    {
        for(int i=0; i<AvailableResolutions.Length; i++)
        {
            if(Screen.width == AvailableResolutions[i].width && Screen.height == AvailableResolutions[i].height)
            {
                thisDropdown.value = i;
                thisDropdown.captionText.text = thisDropdown.options[i].text;
                return;
            }
        }
    }

    private void SetBackendAvailableScreenResolutions()
    {
        int availableResSize = thisDropdown.options.Count;
        AvailableResolutions = new ScreenResolution[availableResSize];
        string[] splittedResolutionText;
        int width, height;

        for (int i = 0; i < availableResSize; i++)
        {
            splittedResolutionText = thisDropdown.options[i].text.Split('x');
            width = int.Parse(splittedResolutionText[0]);
            height = int.Parse(splittedResolutionText[1]);

            AvailableResolutions[i] = new ScreenResolution(width, height);
        }
    }
}
