using UnityEngine;

public class SlotPrefab : MonoBehaviour
{
    private Canvas thisCanvas;

    private void Awake()
    {
        thisCanvas = GetComponent<Canvas>();
        thisCanvas.worldCamera = Camera.main;
        thisCanvas.planeDistance = 0.309f;
    }
}