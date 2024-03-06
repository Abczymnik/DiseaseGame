using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorSkin : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;

    private void OnValidate()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnActiveSceneChange;
    }

    private void LateUpdate()
    {
        rectTransform.anchoredPosition = Mouse.current.position.ReadValue();
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
