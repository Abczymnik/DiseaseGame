using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    [SerializeField, HideInInspector] private Volume dimVolume;
    [SerializeField] private GameObject bloodOnNote; //inactive -> referenced in editor 
    [SerializeField] private GameObject tipOnNote; //inactive -> referenced in editor

    private CanvasGroup currentNoteCanvasFade;
    private Image currentNoteImageFade;
    private Transform currentNoteTrans;
    private int currentMaterialMetallicPropertyID;
    private int currentMaterialColorPropertyID;

    private bool changeScene = false;
    private bool skipTriggered = false;

    private readonly float[] timersToReadNotes = { 20f, 15f };

    private InputAction skip;

    private void OnValidate()
    {
        dimVolume = GameObject.Find("/Settings/Dim Volume").GetComponent<Volume>();
    }

    private void OnEnable()
    {
        skip = PlayerUI.Instance.InputActions.Intro.Skip;
    }

    public IEnumerator Start()
    {
        CursorSwitch.ShowCursor();
        StartCoroutine(AsyncLoadScene());

        for (float f = 0; f < 1.5; f += Time.deltaTime)
        {
            dimVolume.weight = Mathf.Lerp(1, 0.85f, f / 1.5f);
            yield return null;
        }

        StartCoroutine(AnimateBasicNote(0));
    }

    private IEnumerator AnimateBasicNote(int noteIndex)
    {
        SetComponentsForNote(noteIndex+1);
        for (float f = 0; f < 3; f += Time.deltaTime)
        {
            currentNoteCanvasFade.alpha = f / 3;
            currentNoteImageFade.material.SetFloat(currentMaterialMetallicPropertyID, f / 3);
            yield return null;
        }

        yield return new WaitForSeconds(3f);
        EnableTipAndSkip();

        for (float f = 0; f < timersToReadNotes[noteIndex]; f += Time.deltaTime)
        {
            if (skipTriggered) { break; }
            yield return null;
        }

        DisableTipAndSkip();
        for (float f = 0; f < 3; f += Time.deltaTime)
        {
            currentNoteCanvasFade.alpha = 1 - f / 3;
            currentNoteImageFade.material.SetFloat(currentMaterialMetallicPropertyID, 1 - f / 3);
            yield return null;
        }

        currentNoteTrans.GetComponent<Canvas>().enabled = false;
        currentNoteImageFade.material.SetFloat(currentMaterialMetallicPropertyID, 1);

        if (noteIndex < timersToReadNotes.Length-1) yield return StartCoroutine(AnimateBasicNote(noteIndex+1));
        else if(noteIndex == timersToReadNotes.Length - 1) yield return StartCoroutine(AnimateBloodyNote());
        yield break;
    }

    private void SetComponentsForNote(int noteIndex)
    {
        currentNoteTrans = transform.GetChild(noteIndex);
        currentNoteCanvasFade = currentNoteTrans.GetComponent<CanvasGroup>();
        currentNoteImageFade = currentNoteTrans.GetChild(0).GetComponent<Image>();

        currentMaterialMetallicPropertyID = Shader.PropertyToID("_Metallic");
        currentNoteImageFade.material.SetFloat(currentMaterialMetallicPropertyID, 0);
        currentNoteTrans.GetComponent<Canvas>().enabled = true;
        if (noteIndex == timersToReadNotes.Length + 1) currentMaterialColorPropertyID = Shader.PropertyToID("_BaseColor");
    }

    private void EnableTipAndSkip()
    {
        tipOnNote.SetActive(true);
        skip.performed += SkipPerformed;
    }

    private void DisableTipAndSkip()
    {
        skip.performed -= SkipPerformed;
        skipTriggered = false;
        tipOnNote.SetActive(false);
    }

    private IEnumerator AnimateBloodyNote()
    {
        SetComponentsForNote(3);
        for (float f = 0; f < 3; f += Time.deltaTime)
        {
            currentNoteCanvasFade.alpha = f / 3;
            currentNoteImageFade.material.SetFloat(currentMaterialMetallicPropertyID, f / 3);
            yield return null;
        }

        EnableTipAndSkip();
        for (float f = 0; f < 7; f += Time.deltaTime)
        {
            if (skipTriggered) { break; }
            yield return null;
        }

        DisableTipAndSkip();
        bloodOnNote.SetActive(true);
        Image bloodFade = bloodOnNote.GetComponent<Image>();

        yield return new WaitForSeconds(4f);

        for (float f = 0; f < 3; f += Time.deltaTime)
        {
            currentNoteCanvasFade.alpha = 1 - f / 3;
            currentNoteImageFade.material.SetFloat(currentMaterialMetallicPropertyID, 1 - f / 3);
            bloodFade.material.SetColor(currentMaterialColorPropertyID, new Color(1, 1, 1, 1 - f / 3));
            yield return null;
        }

        currentNoteTrans.GetComponent<Canvas>().enabled = false;
        currentNoteImageFade.material.SetFloat(currentMaterialMetallicPropertyID, 1);
        bloodFade.material.SetColor(currentMaterialColorPropertyID, new Color(1, 1, 1, 1));
        bloodFade.enabled = false;
        changeScene = true;
        yield break;
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

    public void SkipPerformed(InputAction.CallbackContext _)
    {
        skipTriggered = true;
    }
}
