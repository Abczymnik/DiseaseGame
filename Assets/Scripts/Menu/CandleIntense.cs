using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(HDAdditionalLightData))]
public class CandleIntense : MonoBehaviour
{
    [SerializeField, HideInInspector] private HDAdditionalLightData lightData;
    private UnityAction onNextSceneWish;

    private void OnValidate()
    {
        lightData = GetComponent<HDAdditionalLightData>();
    }

    private void OnEnable()
    {
        onNextSceneWish += OnNextSceneWish;
        EventManager.StartListening(UnityEventName.NextSceneWish, onNextSceneWish);
    }
    
    private void Update()
    {
        lightData.intensity = 20 + Mathf.PingPong(Time.time*100, 10);
    }

    private void OnNextSceneWish()
    {
        this.enabled = false;
    }

    private void OnDisable()
    {
        EventManager.StopListening(UnityEventName.NextSceneWish, onNextSceneWish);
    }
}
