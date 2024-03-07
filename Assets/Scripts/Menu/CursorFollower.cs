using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class CursorFollower : MonoBehaviour
{
    private InputAction mousePosition;
    [SerializeField, HideInInspector] private Rigidbody cursorHandleRigbody;

    private Vector2 screenResolution;
    public Vector2 LastCursorPosition { get; set; }

    private const float SCENE_MIN_X = 25f;
    private const float SCENE_MIN_Z = 2f;
    private const float SCENE_MAX_X = 33.3f;
    private const float SCENE_MAX_Z = 12f;
    private const float SPEED = 15f;

    private void OnValidate()
    {
        cursorHandleRigbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        mousePosition = PlayerUI.Instance.InputActions.Menu.Pointer;
        mousePosition.performed += OnCursorPositionChange;
    }

    private void Start()
    {
        screenResolution = new Vector2(Screen.width, Screen.height);
        LastCursorPosition = screenResolution * 0.5f;
    }

    private void FixedUpdate()
    {
        cursorHandleRigbody.MovePosition(transform.position + SPEED * Time.fixedDeltaTime * GetMoveVector());
    }

    private Vector3 GetMoveVector()
    {
        return RemapCursorToScene(LastCursorPosition) - transform.position;
    }

    private Vector3 RemapCursorToScene(Vector2 cursorPosition)
    {
        float xValue = cursorPosition.x * (SCENE_MAX_X - SCENE_MIN_X) / screenResolution.x + SCENE_MIN_X;
        float yValue = 2f;
        float zValue = cursorPosition.y * (SCENE_MAX_Z - SCENE_MIN_Z) / screenResolution.y + SCENE_MIN_Z;

        return new Vector3(xValue, yValue, zValue);
    }

    private void OnCursorPositionChange(InputAction.CallbackContext context)
    {
        LastCursorPosition = context.ReadValue<Vector2>();
    }

    private void OnDisable()
    {
        mousePosition.performed -= OnCursorPositionChange;
    }
}