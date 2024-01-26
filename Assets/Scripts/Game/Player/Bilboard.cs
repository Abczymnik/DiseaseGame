using UnityEngine;

public class Bilboard : MonoBehaviour
{
    [SerializeField] private Transform cameraTrans;

    private void Start()
    {
        if (cameraTrans == null) cameraTrans = Camera.main.transform;
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + cameraTrans.forward);
    }
}