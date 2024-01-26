using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.HighDefinition;

public class CandleIntense : MonoBehaviour
{
    private HDAdditionalLightData lightData;
    private UnityAction onLevelChange;

    private void OnEnable()
    {
        onLevelChange += OnLevelChange;
        EventManager.StartListening("ChangeLevel", onLevelChange);
    }

    private void Start()
    {
        lightData = GetComponent<HDAdditionalLightData>();
    }
    
    void Update()
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
