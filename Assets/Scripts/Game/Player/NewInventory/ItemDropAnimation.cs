using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ItemDropAnimation : MonoBehaviour
{
    private float throwForce = 5f;
    private float rotationSpeed = 300f;
    private float unitsAboveGround = 0.2f;
    [SerializeField] private int terrainLayer;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider thisCollider;
    [SerializeField] private Collider parentCollider;
    [SerializeField] private ItemBehaviourOnGround itemBehaviourOnGround;

    private void OnValidate()
    {
        terrainLayer = LayerMask.NameToLayer("Terrain");
        rb = GetComponent<Rigidbody>();
        thisCollider = GetComponent<Collider>();
        parentCollider = transform.parent.GetComponent<Collider>();
        itemBehaviourOnGround = GetComponent<ItemBehaviourOnGround>();
    }

    private void OnEnable()
    {
        rb.isKinematic = false;
        rb.AddForce((transform.forward + Vector3.up * 2).normalized * throwForce, ForceMode.Impulse);
        StartCoroutine(RotateObjectDuringThrow());
    }

    IEnumerator RotateObjectDuringThrow()
    {
        while (true)
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == terrainLayer)
        {
            StopAllCoroutines();
            DisablePhysicsForItem();
            SetItemAboveGround(unitsAboveGround);
        }
    }

    private void DisablePhysicsForItem()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
    }

    private void SetItemAboveGround(float unitsAboveGround)
    {
        Vector3 currentPosition = transform.position;
        currentPosition.y += unitsAboveGround;
        transform.position = currentPosition;
        thisCollider.enabled = false;

        BringParentColliderToChildPosition();
        parentCollider.enabled = true;

        itemBehaviourOnGround.enabled = true;
        this.enabled = false;
    }

    private void BringParentColliderToChildPosition()
    {
        transform.parent.position = transform.position;
        transform.localPosition = Vector3.zero;
    }
}
