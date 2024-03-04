using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class SceneLoader : MonoBehaviour
{
    [SerializeField, HideInInspector] private Volume skyLightIntense;
    [SerializeField] private Slider loadingSlider; //inactive -> referenced in editor
    private CanvasGroup[] buttons;
    private HDAdditionalLightData[] lightsData;
    private float[] lightIntensities;

    private UnityAction onLevelChange;

    private void OnValidate()
    {
        skyLightIntense = GameObject.Find("/Settings/Dim Sky").GetComponent<Volume>();
    }

    private void OnEnable()
    {
        onLevelChange += OnLevelChange;
        EventManager.StartListening(UnityEventName.ChangeLevel, onLevelChange);
    }

    private void Awake()
    {
        GetLights();
        TurnOffLights();
    }

    private void Start()
    {
        GetButtons();
        StartCoroutine(LoadFirstScene());
    }

    private void GetLights()
    {
        lightsData = FindObjectsByType<HDAdditionalLightData>(FindObjectsSortMode.None);
        lightIntensities = new float[lightsData.Length];

        for (int i = 0; i < lightsData.Length; i++)
        {
            lightIntensities[i] = lightsData[i].intensity;
        }
    }

    private void TurnOffLights()
    {
        foreach(HDAdditionalLightData light in lightsData)
        {
            light.intensity = 0f;
        }
    }

    private void GetButtons()
    {
        buttons = FindObjectsByType<CanvasGroup>(FindObjectsSortMode.None);
    }

    IEnumerator LoadFirstScene()
    {
        float timer = 0;
        float speed = 3f;

        while (timer < speed)
        {
            timer += Time.deltaTime;
            for (int i = 0; i < lightsData.Length; i++)
            {
                lightsData[i].intensity = Mathf.Lerp(0, lightIntensities[i], timer / speed);
            }

            skyLightIntense.weight = Mathf.Lerp(1, 0, timer / speed);
            yield return null;
        }
 
        timer = 0;
        speed = 1f;

        while (timer < speed)
        {
            timer += Time.deltaTime;
            foreach (CanvasGroup button in buttons)
            {
                button.alpha = Mathf.Lerp(0, 1, timer / speed);
            }
            yield return null;
        }
    }

    IEnumerator LoadNextScene()
    {
        InputSystem.DisableDevice(Mouse.current);
        loadingSlider.transform.parent.gameObject.SetActive(true);
        float timer = 0;
        float speed = 1f;

        while (timer < speed)
        {
            timer += Time.deltaTime;
            foreach (CanvasGroup button in buttons)
            {
                button.alpha = Mathf.Lerp(1, 0, timer / speed);
            }

            loadingSlider.value = timer;
            yield return null;
        }

        timer = 0;
        speed = 2.5f;

        while (timer < speed)
        {
            timer += Time.deltaTime;
            loadingSlider.value = timer+1;

            for (int i = 0; i < lightsData.Length; i++)
            {
                lightsData[i].intensity = Mathf.Lerp(lightIntensities[i], 0, timer / speed);
            }

            skyLightIntense.weight = Mathf.Lerp(0, 1, timer / speed);
            yield return null;
        }
        InputSystem.EnableDevice(Mouse.current);
        SceneManager.LoadScene(1);
    }

    private void OnLevelChange()
    {
        StartCoroutine(LoadNextScene());
    }

    private void OnDisable()
    {
        EventManager.StopListening(UnityEventName.ChangeLevel, onLevelChange);
    }
}
