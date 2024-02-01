using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(VisualEffect))]
public class WindController : MonoBehaviour
{
    [SerializeField, HideInInspector] private VisualEffect fireVFX;
    private Vector3 baseWindVelocity;
    private int windVelocityID;

    private void OnValidate()
    {
        fireVFX = GetComponent<VisualEffect>();
    }

    private void Start()
    {
        windVelocityID = Shader.PropertyToID("_WindVelocity");
        baseWindVelocity = fireVFX.GetVector3(windVelocityID);
        StartCoroutine(WindVariations());
    }

    IEnumerator WindVariations()
    {
        while (true)
        {
            fireVFX.SetVector3(windVelocityID, baseWindVelocity + new Vector3(0.5f, -0.25f, 2f));
            yield return new WaitForSeconds(0.8f);
            fireVFX.SetVector3(windVelocityID, baseWindVelocity - new Vector3(0.5f, 0.25f, 2f));
            yield return new WaitForSeconds(0.6f);
            fireVFX.SetVector3(windVelocityID, baseWindVelocity);
            yield return new WaitForSeconds(2.4f);
            fireVFX.SetVector3(windVelocityID, baseWindVelocity + new Vector3(0.5f, -0.25f, 2f));
            yield return new WaitForSeconds(1f);
            fireVFX.SetVector3(windVelocityID, baseWindVelocity);
            yield return new WaitForSeconds(2.4f);
            fireVFX.SetVector3(windVelocityID, baseWindVelocity - new Vector3(0.5f, 0.25f, 0.5f));
            yield return new WaitForSeconds(1.4f);
            fireVFX.SetVector3(windVelocityID, baseWindVelocity - new Vector3(0.5f, 0.25f, 2f));
            yield return new WaitForSeconds(1f);
            fireVFX.SetVector3(windVelocityID, baseWindVelocity);
            yield return new WaitForSeconds(2.4f);
        }
    }
}
