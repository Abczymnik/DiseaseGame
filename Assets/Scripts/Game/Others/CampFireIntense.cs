using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class CampFireIntense : MonoBehaviour
{
    private HDAdditionalLightData lightData;

    void Start()
    {
        lightData = GetComponent<HDAdditionalLightData>();
    }

    void Update()
    {
        lightData.intensity = 12 + Mathf.PingPong(Time.time * 8, 4);
    }
}
