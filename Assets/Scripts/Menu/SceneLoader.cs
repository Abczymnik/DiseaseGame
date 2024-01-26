using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering.HighDefinition;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private GameObject loadingBar;
    [SerializeField] private Volume skyLightIntense;
    private Slider loadingSlider;
    private List<CanvasGroup> buttonsAlpha = new List<CanvasGroup>();
    private List<HDAdditionalLightData> lightsData = new List<HDAdditionalLightData>();
    private List<float> lightIntensities = new List<float>();

    private UnityAction onLevelChange;

    private void Awake()
    {
        if(loadingBar == null) loadingBar = GameObject.Find("/Menu").transform.GetChild(0).gameObject;
        loadingSlider = loadingBar.transform.GetChild(1).GetComponent<Slider>();
        if(skyLightIntense == null) skyLightIntense = GameObject.Find("/Settings/Dim Sky").GetComponent<Volume>();
        GetLights();
        TurnOffLights();
    }

    private void OnEnable()
    {
        onLevelChange += OnLevelChange;
        EventManager.StartListening("ChangeLevel", onLevelChange);
    }

    private void Start()
    {
        GetButtons();
        StartCoroutine(LoadFirstScene());
    }

    private void TurnOffLights()
    {
        foreach(HDAdditionalLightData light in lightsData)
        {
            light.intensity = 0f;
        }
    }

    private void GetLights()
    {
        GameObject[] lightsObj = GameObject.FindGameObjectsWithTag("Light");
        foreach(GameObject light in lightsObj)
        {
            HDAdditionalLightData lightAddData = light.GetComponent<HDAdditionalLightData>();
            lightsData.Add(lightAddData);
            lightIntensities.Add(lightAddData.intensity);
        }
    }

    private void GetButtons()
    {
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Menu Buttons");
        foreach(GameObject button in buttons)
        {
            buttonsAlpha.Add(button.GetComponent<CanvasGroup>());
        }
    }

    IEnumerator LoadFirstScene()
    {
        float timer = 0;
        float speed = 3f;

        while (timer < speed)
        {
            timer += Time.deltaTime;

            for (int i = 0; i < lightsData.Count; i++)
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

            foreach (CanvasGroup button in buttonsAlpha)
            {
                button.alpha = Mathf.Lerp(0, 1, timer / speed);
            }

            yield return null;
        }
    }

    IEnumerator LoadNextScene()
    {
        InputSystem.DisableDevice(Mouse.current);
        loadingBar.SetActive(true);
        float timer = 0;
        float speed = 1f;

        while (timer < speed)
        {
            timer += Time.deltaTime;

            foreach (CanvasGroup button in buttonsAlpha)
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

            for (int i = 0; i < lightsData.Count; i++)
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
        EventManager.StopListening("ChangeLevel", onLevelChange);
    }
}
