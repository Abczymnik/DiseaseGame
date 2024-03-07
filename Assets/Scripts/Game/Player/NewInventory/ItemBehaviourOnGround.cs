using UnityEngine;

public class ItemBehaviourOnGround : MonoBehaviour
{
    private float rotationSpeed = 50f;
    private float bounceHeight = 0.1f;
    private float bounceSpeed = 1f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;
        currentRotation.y += rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(currentRotation);

        Vector3 bounceOffset = Vector3.up * Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
        transform.position = startPosition + bounceOffset;
    }
}
