using UnityEngine;
using UnityEngine.InputSystem;

public class LanternCollider : MonoBehaviour
{
    private const float PUSH_FORCE = 6f;

    private Rigidbody lanternRigbody;
    [SerializeField] private CursorFollower lanternHandle;

    private void Awake()
    {
        lanternRigbody = GetComponent<Rigidbody>();
        lanternRigbody.isKinematic = true;
    }

    private void Start()
    {
        if(lanternHandle == null) lanternHandle = GameObject.Find("/Cursor Drag Point").GetComponent<CursorFollower>();
        Cursor.lockState = CursorLockMode.Confined;
        lanternRigbody.isKinematic = false;
        lanternRigbody.constraints = RigidbodyConstraints.FreezePositionY;
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
        PushCursorFromCollider(collisionNormal);
    }

    private void PushCursorFromCollider(Vector3 collisionNormal)
    {
        int cursorXAxisOffset = (int)Mathf.Ceil(collisionNormal.x * lanternRigbody.velocity.magnitude * PUSH_FORCE);
        int cursorYAxisOffset = (int)Mathf.Ceil(collisionNormal.z * lanternRigbody.velocity.magnitude * PUSH_FORCE);
        Vector2 cursorOffset = new Vector2(cursorXAxisOffset, cursorYAxisOffset);
        lanternHandle.LastCursorPosition += cursorOffset; 
        Mouse.current.WarpCursorPosition(lanternHandle.LastCursorPosition);
    }
}
