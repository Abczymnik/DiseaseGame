using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class LanternCollider : MonoBehaviour
{
    [SerializeField, HideInInspector] private Rigidbody lanternRigbody;
    [SerializeField, HideInInspector] private CursorFollower lanternHandle;

    private const float PUSH_FORCE = 5f;

    private void OnValidate()
    {
        lanternRigbody = GetComponent<Rigidbody>();
        lanternHandle = FindAnyObjectByType<CursorFollower>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contactCount == 0) return;
        Vector3 collisionNormal = collision.GetContact(0).normal;
        PushCursorFromCollider(collisionNormal);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.contactCount == 0) return;
        Vector3 collisionNormal = collision.GetContact(0).normal;
        PushCursorFromCollider(collisionNormal*2);
    }

    private void PushCursorFromCollider(Vector3 dirToPush)
    {
        int cursorXAxisOffset = (int)Mathf.Ceil(dirToPush.x * lanternRigbody.velocity.magnitude * PUSH_FORCE);
        int cursorYAxisOffset = (int)Mathf.Ceil(dirToPush.z * lanternRigbody.velocity.magnitude * PUSH_FORCE);
        Vector2 cursorOffset = new Vector2(cursorXAxisOffset, cursorYAxisOffset);
        lanternHandle.LastCursorPosition += cursorOffset; 
        Mouse.current.WarpCursorPosition(lanternHandle.LastCursorPosition);
    }
}
