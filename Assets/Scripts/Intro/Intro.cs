using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    private CanvasGroup actualCanvasFade;
    private Image actualImageFade;
    private Transform actualSceneTrans;
    private GameObject blood;
    private GameObject tip;
    private bool changeScene = false;
    private InputAction skip;
    private bool skipTriggered = false;
    [SerializeField] private Volume dimVolume;

    private void Awake()
    {
        blood = transform.GetChild(4).gameObject;
        tip = transform.GetChild(5).gameObject;
        if(dimVolume == null) dimVolume = GameObject.Find("/Settings/Dim Volume").GetComponent<Volume>();
        skip = PlayerUI.inputActions.Intro.Skip;
        PlayerUI.SwitchActionMap(PlayerUI.inputActions.Intro);
    }

    public void SkipPerformed(InputAction.CallbackContext context)
    {
        skipTriggered = true;
    }

    private void SetComponentsForScene(int scene)
    {
        actualSceneTrans = transform.GetChild(scene);
        actualCanvasFade = actualSceneTrans.GetComponent<CanvasGroup>();
        actualImageFade = actualSceneTrans.GetChild(0).GetComponent<Image>();
        actualImageFade.material.SetFloat("_Metallic", 0);
        actualSceneTrans.GetComponent<Canvas>().enabled = true;
    }

    private void DisableTipAndSkip()
    {
        skip.performed -= SkipPerformed;
        skipTriggered = false;
        tip.SetActive(false);
    }

    private void EnableTipAndSkip()
    {
        tip.SetActive(true);
        skip.performed += SkipPerformed;
    }

    public IEnumerator Start()
    {
        CursorSwitch.ShowCursor();

        for (float f = 0; f < 1.5; f += Time.deltaTime)
        {
            dimVolume.weight = Mathf.Lerp(1, 0.85f, f / 1.5f);
            yield return null;
        }

        SetComponentsForScene(1);

        yield return new WaitForSeconds(1.5f);

        for (float f = 0; f < 3; f += Time.deltaTime)
        {
            actualCanvasFade.alpha = f / 3;
            actualImageFade.material.SetFloat("_Metallic", f / 3);
            yield return null;
        }

        StartCoroutine(AsyncLoadScene());
        yield return new WaitForSeconds(5);

        EnableTipAndSkip();

        for (float f = 0; f < 20; f += Time.deltaTime)
        {
            if (skipTriggered) { break; }
            yield return null;
        }

        DisableTipAndSkip();

        for (float f = 0; f < 3; f += Time.deltaTime)
        {
            actualCanvasFade.alpha = 1 - f / 3;
            actualImageFade.material.SetFloat("_Metallic", 1 - f / 3);
            yield return null;
        }

        actualSceneTrans.GetComponent<Canvas>().enabled = false;
        actualImageFade.material.SetFloat("_Metallic", 1);

        SetComponentsForScene(2);

        for (float f = 0; f < 3; f += Time.deltaTime)
        {
            actualCanvasFade.alpha = f / 3;
            actualImageFade.material.SetFloat("_Metallic", f / 3);
            yield return null;
        }

        EnableTipAndSkip();

        for (float f = 0; f < 15; f += Time.deltaTime)
        {
            if (skipTriggered) { break; }
            yield return null;
        }

        DisableTipAndSkip();

        for (float f = 0; f < 3; f += Time.deltaTime)
        {
            actualCanvasFade.alpha = 1 - f / 3;
            actualImageFade.material.SetFloat("_Metallic", 1 - f / 3);
            yield return null;
        }

        actualSceneTrans.GetComponent<Canvas>().enabled = false;
        actualImageFade.material.SetFloat("_Metallic", 1);

        SetComponentsForScene(3);

        for (float f = 0; f < 3; f += Time.deltaTime)
        {
            actualCanvasFade.alpha = f / 3;
            actualImageFade.material.SetFloat("_Metallic", f / 3);
            yield return null;
        }

        EnableTipAndSkip();

        for (float f = 0; f < 7; f += Time.deltaTime)
        {
            if (skipTriggered) { break; }
            yield return null;
        }

        DisableTipAndSkip();
        blood.SetActive(true);
        Image bloodFade = blood.GetComponent<Image>();

        for (float f = 0; f < 4; f += Time.deltaTime)
        {
            yield return null;
        }

        for (float f = 0; f < 3; f += Time.deltaTime)
        {
            actualCanvasFade.alpha = 1 - f / 3;
            actualImageFade.material.SetFloat("_Metallic", 1 - f / 3);
            bloodFade.material.SetColor("_BaseColor", new Color(1, 1, 1, 1 - f / 3));
            yield return null;
        }

        actualSceneTrans.GetComponent<Canvas>().enabled = false;
        actualImageFade.material.SetFloat("_Metallic", 1);
        bloodFade.material.SetColor("_BaseColor", new Color(1, 1, 1, 1));
        bloodFade.enabled = false;

        CursorSwitch.SwitchSkin("Standard");
        CursorSwitch.ShowCursor();
        changeScene = true;
    }

    private IEnumerator AsyncLoadScene()
    {
        yield return null;

        AsyncOperation asyncLoader = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        asyncLoader.allowSceneActivation = false;

        while(!asyncLoader.isDone)
        {
            if (asyncLoader.progress >= 0.9f)
            {
                if(changeScene) asyncLoader.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}