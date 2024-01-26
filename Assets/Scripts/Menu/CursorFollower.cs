using UnityEngine;
using UnityEngine.InputSystem;

public class CursorFollower : MonoBehaviour
{
    private InputAction mousePosition;
    private Rigidbody cursorHandleRigbody;

    public Vector2 ScreenResolution { get; private set; }
    public Vector2 LastCursorPosition { get; set; }

    private void Awake()
    {
        mousePosition = PlayerUI.inputActions.Menu.Pointer;
        mousePosition.performed += MoveCursorToTarget;
        cursorHandleRigbody = GetComponent<Rigidbody>();
        cursorHandleRigbody.constraints = RigidbodyConstraints.FreezePositionY;
        ScreenResolution = new Vector2(Screen.currentResolution.width,Screen.currentResolution.height);
    }

    private void Start()
    {
        CursorSwitch.HideCursor();
        PlayerUI.SwitchActionMap(PlayerUI.inputActions.Menu);
        LastCursorPosition = ScreenResolution * 0.5f;
    }

    private void FixedUpdate()
    {
        cursorHandleRigbody.MovePosition(RemapCursorToScene(LastCursorPosition));
    }

    private void MoveCursorToTarget(InputAction.CallbackContext context)
    {
        LastCursorPosition = mousePosition.ReadValue<Vector2>();
    }

    private Vector3 RemapCursorToScene(Vector2 cursorPosition)
    {
        float cursorMinX = 0f;
        float cursorMinY = 0f;
        float cursorMaxX = ScreenResolution.x;
        float cursorMaxY = ScreenResolution.y;
        float sceneMinX = 25f;
        float sceneMinZ = 2f;
        float sceneMaxX = 33.3f;
        float sceneMaxZ = 12f;

        if (cursorMaxX == 0 || cursorMaxY == 0) return Vector3.zero;

        float xValue = (cursorPosition.x - cursorMinX) * (sceneMaxX - sceneMinX) / (cursorMaxX - cursorMinX) + sceneMinX;
        float zValue = (cursorPosition.y - cursorMinY) * (sceneMaxZ - sceneMinZ) / (cursorMaxY - cursorMinY) + sceneMinZ;
        float yValue = 2f;

        return new Vector3(xValue, yValue, zValue);
    }

    private void OnDisable()
    {
        mousePosition.performed -= MoveCursorToTarget;
    }
}