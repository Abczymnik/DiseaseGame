using UnityEngine;
using UnityEngine.InputSystem;

public class CursorSkinMenu : MonoBehaviour
{
    private InputAction mousePosition;
    private Camera cam;

    private void Awake()
    {
        PlayerUI.SwitchActionMap(PlayerUI.inputActions.Menu);
        mousePosition = PlayerUI.inputActions.Menu.Pointer;
        mousePosition.performed += MoveToCursor;
        cam = Camera.main;
    }

    private void Start()
    {
        CursorSwitch.SwitchSkin("Standard");
    }

    private void MoveToCursor(InputAction.CallbackContext context)
    {
        Vector2 mousePos = mousePosition.ReadValue<Vector2>();
        transform.position = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 1));
    }

    private void OnDisable()
    {
        mousePosition.performed -= MoveToCursor;
    }
}
