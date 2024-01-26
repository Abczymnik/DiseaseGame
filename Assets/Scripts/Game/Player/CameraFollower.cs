using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(-0.5f, 6, -5);

    private void Awake()
    {
        if(target == null) target = GameObject.Find("/Player").transform;
    }

    private void LateUpdate()
    {
        transform.position = target.position + offset;
    }
}