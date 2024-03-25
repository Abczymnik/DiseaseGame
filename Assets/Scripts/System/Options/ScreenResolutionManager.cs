using TMPro;
using UnityEngine;

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

    private void OnValidate()
    {
        thisDropdown = GetComponent<TMP_Dropdown>();
    }

    private void Awake()
    {
        SetBackendAvailableScreenResolutions();
    }

    public void SetScreenResolution(int resolutionIndex)
    {
        Screen.SetResolution(AvailableResolutions[resolutionIndex].width, AvailableResolutions[resolutionIndex].height, true);
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
