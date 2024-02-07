using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform targetToFollow;
    [SerializeField] private Vector3 cameraOffset = new Vector3(-3.3f, 6.3f, -3.8f);
    private Vector3 currentVelocity;
    private const float smoothTime = 0.1f;

    private void OnValidate()
    {
        targetToFollow = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        Vector3 targetPosition = targetToFollow.position + cameraOffset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
    }
}