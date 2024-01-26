using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExperienceInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Experience playerExperience;
    [SerializeField] private Canvas experienceInfoCanvas;
    [SerializeField] private TMPro.TMP_Text experienceText;
    private Coroutine refreshExp;

    private void Awake()
    {
        if (experienceText == null) experienceText = transform.GetChild(0).GetComponentInChildren<TMPro.TMP_Text>();
        if (experienceInfoCanvas == null) experienceInfoCanvas = transform.GetChild(0).GetComponent<Canvas>();
    }

    private void Start()
    {
        playerExperience = GetComponentInParent<Experience>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        experienceInfoCanvas.enabled = true;
        refreshExp = StartCoroutine(RefreshExp());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        experienceInfoCanvas.enabled = false;
        StopCoroutine(refreshExp);
    }

    IEnumerator RefreshExp()
    {
        while(true)
        {
            experienceText.text = ((int)playerExperience.CurrentExp).ToString() + "/" + ((int)playerExperience.MaxExp).ToString();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
