using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private Transform cameraTrans;

    private void OnValidate()
    {
        cameraTrans = Camera.main.transform;
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + cameraTrans.forward);
    }
}