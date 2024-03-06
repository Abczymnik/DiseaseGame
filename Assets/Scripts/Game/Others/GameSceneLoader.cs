using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

public class GameSceneLoader : MonoBehaviour
{ 
    [SerializeField] private Volume skyLightIntense;

    private HDAdditionalLightData[] lightsData;
    private float[] lightIntensities;
    private List<Image> GUIImages = new List<Image>();
    private Color[] GUIImagesColor;

    private void OnValidate()
    {
        skyLightIntense = GameObject.Find("/Settings/Dim Sky").GetComponent<Volume>();
    }

    private void Awake()
    {
        GetLights();
        TurnOffLights();
        GetGUIMaterials();
        TurnOffGUI();
    }

    private void Start()
    {
        StartCoroutine(LoadSceneLights());
        StartCoroutine(DelayedLoadGUI());
    }

    IEnumerator LoadSceneLights()
    {
        float timer = 0;
        float timeForChanges = 3f;

        while(timer < timeForChanges)
        {
            timer += Time.deltaTime;

            for (int i = 0; i < lightsData.Length; i++)
            {
                lightsData[i].intensity = Mathf.Lerp(0, lightIntensities[i], timer / timeForChanges);
            }

            skyLightIntense.weight = Mathf.Lerp(1, 0, timer / timeForChanges);

            yield return null;
        }
    }

    IEnumerator DelayedLoadGUI()
    {
        yield return new WaitForSeconds(1);

        float timer = 0;
        float timeForChanges = 3f;

        while (timer < timeForChanges)
        {
            timer += Time.deltaTime;

            for (int i = 0; i < GUIImages.Count; i++)
            {
                Color targetColor = GUIImagesColor[i];
                targetColor.a = Mathf.Lerp(0, targetColor.a, timer / timeForChanges);
                GUIImages[i].material.SetColor("_UnlitColor", targetColor);
            }

            yield return null;
        }

        OnDisable();
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
        foreach (HDAdditionalLightData light in lightsData)
        {
            light.intensity = 0f;
        }
    }

    private void GetGUIMaterials()
    {
        GameObject[] GUIObjects = GameObject.FindGameObjectsWithTag("GUI Element");
        foreach (GameObject GUIObject in GUIObjects)
        {
            Image[] GUIComponents = GUIObject.GetComponentsInChildren<Image>();
            GUIImages.AddRange(GUIComponents);
        }

        GUIImagesColor = new Color[GUIImages.Count];
        for (int i = 0; i < GUIImages.Count; i++)
        {
            GUIImagesColor[i] = GUIImages[i].material.GetColor("_UnlitColor");
        }
    }

    private void TurnOffGUI()
    {
        for (int i = 0; i < GUIImages.Count; i++)
        {
            Color targetColor = GUIImagesColor[i];
            targetColor.a = 0;
            GUIImages[i].material.SetColor("_UnlitColor", targetColor);
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < GUIImages.Count; i++)
        {
            GUIImages[i].material.SetColor("_UnlitColor", GUIImagesColor[i]);
        }
    }
}
