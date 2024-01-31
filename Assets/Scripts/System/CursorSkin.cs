using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorSkin : MonoBehaviour
{
    private Vector2 mousePosition;
    private Camera cam;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnActiveSceneChange;
    }

    private void LateUpdate()
    {
        mousePosition = Mouse.current.position.ReadValue();
        transform.position = cam.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 1));
        transform.LookAt(transform.position + cam.transform.forward);
    }

    private void OnActiveSceneChange(Scene _, Scene next)
    {
        int sceneIndex = next.buildIndex;
        cam = Camera.main;

        switch (sceneIndex)
        {
            case 0:
                PlayerUI.SwitchActionMap(PlayerUI.inputActions.Menu);
                break;
            case 1:
                PlayerUI.SwitchActionMap(PlayerUI.inputActions.Intro);
                break;
            case 2:
                PlayerUI.SwitchActionMap(PlayerUI.inputActions.Gameplay);
                break;
            default:
                PlayerUI.SwitchActionMap(PlayerUI.inputActions.Gameplay);
                break;
        }
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChange;
    }
}
