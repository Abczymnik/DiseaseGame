using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class HealthInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private HealthBar playerHealth;
    [SerializeField] private TMPro.TMP_Text healthText;
    [SerializeField] private Canvas healthInfoCanvas;
    private Coroutine refreshHealth;

    private void OnValidate()
    {
        playerHealth = GetComponentInParent<HealthBar>();
        healthText = transform.GetChild(0).GetComponentInChildren<TMPro.TMP_Text>();
        healthInfoCanvas = transform.GetChild(0).GetComponent<Canvas>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        healthInfoCanvas.enabled = true;
        refreshHealth = StartCoroutine(RefreshHealth());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopCoroutine(refreshHealth);
        healthInfoCanvas.enabled = false;
    }

    IEnumerator RefreshHealth()
    {
        while(true)
        {
            healthText.text = ((int)playerHealth.CurrentHealth).ToString() + "/" + ((int)playerHealth.MaxHealth).ToString();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
