using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartGameAfterDeath : MonoBehaviour
{
    private HDAdditionalLightData[] lightsData;
    private float[] lightIntensities;
    private List<Image> GUIImages = new List<Image>();
    private Color[] GUIImagesColor;

    [SerializeField] private Volume skyLightIntense;

    private UnityAction onPlayerDeath;

    private void OnValidate()
    {
        skyLightIntense = GameObject.Find("/Settings/Dim Sky").GetComponent<Volume>();
    }

    private void OnEnable()
    {
        onPlayerDeath += OnPlayerDeath;
        EventManager.StartListening(UnityEventName.PlayerDeath, onPlayerDeath);
    }

    IEnumerator TransitionFromGameplayToMenu()
    {
        float timer = 0;
        float timeForChanges = 5f;

        while(timer < timeForChanges)
        {
            timer += Time.deltaTime;

            for(int i=0; i<GUIImages.Count; i++)
            {
                Color targetColor = GUIImagesColor[i];
                targetColor.a = Mathf.Lerp(0, targetColor.a, timer / timeForChanges);
                GUIImages[i].material.SetColor("_UnlitColor", targetColor);
            }

            for (int i = 0; i < lightsData.Length; i++)
            {
                lightsData[i].intensity = Mathf.Lerp(lightIntensities[i], 0, timer / timeForChanges);
            }

            skyLightIntense.weight = Mathf.Lerp(0, 1, timer / timeForChanges);

            yield return null;
        }
        SceneManager.LoadScene(0);
    }

    private void GetLightsData()
    {
        lightsData = FindObjectsByType<HDAdditionalLightData>(FindObjectsSortMode.None);
        lightIntensities = new float[lightsData.Length];

        for (int i = 0; i < lightsData.Length; i++)
        {
            lightIntensities[i] = lightsData[i].intensity;
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
        for (int i=0; i<GUIImages.Count; i++)
        {
            GUIImagesColor[i] = GUIImages[i].material.GetColor("_UnlitColor");
        }
    }

    private void OnPlayerDeath()
    {
        GetLightsData();
        GetGUIMaterials();
        StartCoroutine(TransitionFromGameplayToMenu());
    }

    private void OnDisable()
    {
        EventManager.StopListening(UnityEventName.PlayerDeath, onPlayerDeath);
        for (int i=0; i<GUIImages.Count; i++)
        {
            GUIImages[i].material.SetColor("_UnlitColor", GUIImagesColor[i]);
        }
    }
}
