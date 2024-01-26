using UnityEngine;
using UnityEngine.InputSystem;

public class CursorSkinGameplay : MonoBehaviour
{
    private InputAction mousePosition;
    private Camera cam;

    private void Awake()
    {
        PlayerUI.SwitchActionMap(PlayerUI.inputActions.Gameplay);
        mousePosition = PlayerUI.inputActions.Gameplay.Pointer;
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        MoveToCursor();
    }

    private void Start()
    {
        CursorSwitch.SwitchSkin("Standard");
        CursorSwitch.ShowCursor();
    }

    private void MoveToCursor()
    {
        Vector2 mousePos = mousePosition.ReadValue<Vector2>();
        transform.position = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 1));

    }
}
