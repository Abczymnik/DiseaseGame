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
        if(TrySetScreenResolution(resolutionIndex, out int highestPossibleResolutionIndex))
        {
            Screen.SetResolution(AvailableResolutions[resolutionIndex].width, AvailableResolutions[resolutionIndex].height, true);
            onScreenResolutionChange?.Invoke(AvailableResolutions[resolutionIndex]);
        }
        else
        {
            if (highestPossibleResolutionIndex == -1) return;

            Screen.SetResolution(AvailableResolutions[highestPossibleResolutionIndex].width,
                AvailableResolutions[highestPossibleResolutionIndex].height, true);

            SetResolutionLabel(highestPossibleResolutionIndex);
            onScreenResolutionChange?.Invoke(AvailableResolutions[highestPossibleResolutionIndex]);
        }
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

    private void SetResolutionLabel(int resolutionIndex)
    {
        thisDropdown.value = resolutionIndex;
        thisDropdown.captionText.text = thisDropdown.options[resolutionIndex].text;
    }

    private bool TrySetScreenResolution(int resolutionIndex, out int highestPossibleResolutionIndex)
    {
        foreach(var resolution in Screen.resolutions)
        {
            if(resolution.width == AvailableResolutions[resolutionIndex].width &&
                resolution.height == AvailableResolutions[resolutionIndex].height)
            {
                highestPossibleResolutionIndex = resolutionIndex;
                return true;
            }
        }

        for (int i=resolutionIndex+1; i< AvailableResolutions.Length; i++)
        {
            foreach(var resolution in Screen.resolutions)
            {
                if (resolution.width == AvailableResolutions[i].width && resolution.height == AvailableResolutions[i].height)
                {
                    highestPossibleResolutionIndex = i;
                    return false;
                }
            }
        }

        highestPossibleResolutionIndex = -1;
        return false;
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
