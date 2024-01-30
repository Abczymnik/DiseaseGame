using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class GameSceneLoader : MonoBehaviour
{
    private List<HDAdditionalLightData> lightsData = new List<HDAdditionalLightData>();
    private List<float> lightIntensities = new List<float>();
    private List<CanvasGroup> GUICanvas = new List<CanvasGroup>();
    [SerializeField] private Volume skyLightIntense;

    private void Start()
    {
        CursorSwitch.SwitchSkin(CursorName.Standard);
        if(skyLightIntense == null) skyLightIntense = GameObject.Find("/Settings/Dim Sky").GetComponent<Volume>();
        StartGameScene();
    }

    private void StartGameScene()
    {
        GetLights();
        GetGUICanvas();
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        float timer = 0;
        float speed = 2f;

        while (timer < speed)
        {
            timer += Time.deltaTime;

            foreach (CanvasGroup GUI in GUICanvas)
            {
                GUI.alpha = Mathf.Lerp(0, 1, timer / speed);
            }

            for (int i = 0; i < lightsData.Count; i++)
            {
                lightsData[i].intensity = Mathf.Lerp(0, lightIntensities[i], timer / speed);
            }

            skyLightIntense.weight = Mathf.Lerp(1, 0, timer / speed);

            yield return null;
        }

        CursorSwitch.SwitchSkin(CursorName.Standard);
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
        foreach (GameObject GUI in GUIObj)
        {
            GUICanvas.Add(GUI.GetComponent<CanvasGroup>());
        }
    }
}
