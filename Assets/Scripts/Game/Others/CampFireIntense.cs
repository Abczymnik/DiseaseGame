using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(HDAdditionalLightData))]
public class CampFireIntense : MonoBehaviour
{
    [SerializeField] private HDAdditionalLightData lightData;

    private void OnValidate()
    {
        lightData = GetComponent<HDAdditionalLightData>();
    }

    private void Update()
    {
        lightData.intensity = 12 + Mathf.PingPong(Time.time * 8, 4);
    }
}
