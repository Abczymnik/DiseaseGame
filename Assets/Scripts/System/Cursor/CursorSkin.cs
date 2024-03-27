using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;

public class CursorSkin : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private CanvasScaler scaler;
    private float screenWidthScalar;
    private float screenHeightScalar;

    private void OnValidate()
    {
        rectTransform = GetComponent<RectTransform>();
        scaler = GetComponentInParent<CanvasScaler>();
    }

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnActiveSceneChange;
        ScreenResolutionManager.onScreenResolutionChange += OnScreenResolutionChange;
    }

    private void Start()
    {
        screenWidthScalar = scaler.referenceResolution.x / Screen.width;
        screenHeightScalar = scaler.referenceResolution.y / Screen.height;
    }

    private void LateUpdate()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        rectTransform.anchoredPosition = new Vector2(mousePosition.x * screenWidthScalar, mousePosition.y * screenHeightScalar);
    }

    public void OnScreenResolutionChange(ScreenResolution newScreenResolution)
    {
        StartCoroutine(WaitForNextFrameAndSetScreenScalars(newScreenResolution));
    }

    private IEnumerator WaitForNextFrameAndSetScreenScalars(ScreenResolution newScreenResolution)
    {
        yield return null;

        screenWidthScalar = scaler.referenceResolution.x / newScreenResolution.width;
        screenHeightScalar = scaler.referenceResolution.y / newScreenResolution.height;
    }

    private void OnActiveSceneChange(Scene _, Scene next)
    {
        int sceneIndex = next.buildIndex;
        transform.parent.GetComponent<Canvas>().worldCamera = Camera.main;

        switch (sceneIndex)
        {
            case 0:
                CursorSwitch.HideCursor();
                PlayerUI.SwitchActionMap(PlayerUI.Instance.InputActions.Menu);
                break;
            case 1:
                CursorSwitch.ShowCursor();
                PlayerUI.SwitchActionMap(PlayerUI.Instance.InputActions.Intro);
                break;
            case 2:
                CursorSwitch.ShowCursor();
                PlayerUI.SwitchActionMap(PlayerUI.Instance.InputActions.Gameplay);
                break;
            default:
                CursorSwitch.ShowCursor();
                PlayerUI.SwitchActionMap(PlayerUI.Instance.InputActions.Gameplay);
                break;
        }
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChange;
    }
}
