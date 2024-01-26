using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;

public class RestartGameAfterDeath : MonoBehaviour
{
    private List<HDAdditionalLightData> lightsData = new List<HDAdditionalLightData>();
    private List<float> lightIntensities = new List<float>();
    private List<CanvasGroup> GUICanvas = new List<CanvasGroup>();
    [SerializeField] private Volume skyLightIntense;

    private UnityAction onPlayerDeath;

    private void Awake()
    {
        if (skyLightIntense == null) skyLightIntense = GameObject.Find("/Settings/Dim Sky").GetComponent<Volume>();
    }

    private void OnEnable()
    {
        onPlayerDeath += OnPlayerDeath;
        EventManager.StartListening("PlayerDeath", onPlayerDeath);
    }

    IEnumerator LoadMenu()
    {
        float timer = 0;
        float timeForChanges = 5f;

        while(timer < timeForChanges)
        {
            timer += Time.deltaTime;

            foreach(CanvasGroup GUI in GUICanvas)
            {
                GUI.alpha = Mathf.Lerp(1, 0, timer / timeForChanges);
            }

            for (int i = 0; i < lightsData.Count; i++)
            {
                lightsData[i].intensity = Mathf.Lerp(lightIntensities[i], 0, timer / timeForChanges);
            }

            skyLightIntense.weight = Mathf.Lerp(0, 1, timer / timeForChanges);

            yield return null;
        }
        SceneManager.LoadScene(0);
    }

    private void GetLights()
    {
        GameObject[] lightsObj = GameObject.FindGameObjectsWithTag("Light");
        foreach (GameObject light in lightsObj)
        {
            HDAdditionalLightData lightAddData = light.GetComponent<HDAdditionalLightData>();
            lightsData.Add(lightAddData);
            lightIntensities.Add(lightAddData.intensity);
        }
    }

    private void GetGUICanvas()
    {
        GameObject[] GUIObj = GameObject.FindGameObjectsWithTag("GUI Element");
        foreach(GameObject GUI in GUIObj)
        {
            GUICanvas.Add(GUI.GetComponent<CanvasGroup>());
        }
    }

    private void OnPlayerDeath()
    {
        GetLights();
        GetGUICanvas();
        StartCoroutine(LoadMenu());
    }

    private void OnDisable()
    {
        EventManager.StopListening("PlayerDeath", onPlayerDeath);
    }
}
