using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class WindController : MonoBehaviour
{
    private VisualEffect fireVFX;
    private Vector3 windVelocity;

    private void Awake()
    {
        fireVFX = GetComponent<VisualEffect>();
    }

    private void Start()
    {
        windVelocity = fireVFX.GetVector3("Wind Velocity");
        StartCoroutine(WindVariations());
    }

    IEnumerator WindVariations()
    {
        while (true)
        {
            fireVFX.SetVector3("Wind Velocity", windVelocity + new Vector3(0.5f, -0.25f, 2f));
            yield return new WaitForSeconds(0.8f);
            fireVFX.SetVector3("Wind Velocity", windVelocity - new Vector3(0.5f, 0.25f, 2f));
            yield return new WaitForSeconds(0.6f);
            fireVFX.SetVector3("Wind Velocity", windVelocity);
            yield return new WaitForSeconds(2.4f);
            fireVFX.SetVector3("Wind Velocity", windVelocity + new Vector3(0.5f, -0.25f, 2f));
            yield return new WaitForSeconds(1f);
            fireVFX.SetVector3("Wind Velocity", windVelocity);
            yield return new WaitForSeconds(2.4f);
            fireVFX.SetVector3("Wind Velocity", windVelocity - new Vector3(0.5f, 0.25f, 0.5f));
            yield return new WaitForSeconds(1.4f);
            fireVFX.SetVector3("Wind Velocity", windVelocity - new Vector3(0.5f, 0.25f, 2f));
            yield return new WaitForSeconds(1f);
            fireVFX.SetVector3("Wind Velocity", windVelocity);
            yield return new WaitForSeconds(2.4f);
        }
    }
}
