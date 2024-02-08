using UnityEngine;
using UnityEngine.InputSystem;

public class MouseFollower : MonoBehaviour
{
    private InputAction mousePosition;
    private Camera cam;

    private void Awake()
    {
        PlayerUI.SwitchActionMap(PlayerUI.Instance.InputActions.Gameplay);
        mousePosition = PlayerUI.Instance.InputActions.Gameplay.Pointer;
        cam = Camera.main;
    }

    void LateUpdate()
    {
        MoveToCursor();
    }

    private void MoveToCursor()
    {
        Vector2 mousePos = mousePosition.ReadValue<Vector2>();
        transform.position = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0.309f));
    }
}
