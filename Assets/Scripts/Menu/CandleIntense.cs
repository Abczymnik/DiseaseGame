using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(HDAdditionalLightData))]
public class CandleIntense : MonoBehaviour
{
    [SerializeField, HideInInspector] private HDAdditionalLightData lightData;
    private UnityAction onLevelChange;

    private void OnValidate()
    {
        lightData = GetComponent<HDAdditionalLightData>();
    }

    private void OnEnable()
    {
        onLevelChange += OnLevelChange;
        EventManager.StartListening("ChangeLevel", onLevelChange);
    }
    
    private void Update()
    {
        lightData.intensity = 20 + Mathf.PingPong(Time.time*100, 10);
    }

    private void OnLevelChange()
    {
        this.enabled = false;
    }

    private void OnDisable()
    {
        EventManager.StopListening("ChangeLevel", onLevelChange);
    }
}
