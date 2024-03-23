using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(HDAdditionalLightData))]
public class CandleIntense : MonoBehaviour
{
    [SerializeField, HideInInspector] private HDAdditionalLightData lightData;
    private UnityAction<object> onChangeSceneWish;

    private void OnValidate()
    {
        lightData = GetComponent<HDAdditionalLightData>();
    }

    private void OnEnable()
    {
        onChangeSceneWish += OnChangeSceneWish;
        EventManager.StartListening(TypedEventName.ChangeSceneWish, onChangeSceneWish);
    }
    
    private void Update()
    {
        lightData.intensity = 20 + Mathf.PingPong(Time.time*100, 10);
    }

    private void OnChangeSceneWish(object _)
    {
        this.enabled = false;
    }

    private void OnDisable()
    {
        EventManager.StopListening(TypedEventName.ChangeSceneWish, onChangeSceneWish);
    }
}
