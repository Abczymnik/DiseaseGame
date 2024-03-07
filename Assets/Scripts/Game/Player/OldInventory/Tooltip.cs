using UnityEngine;

public class Tooltip : MonoBehaviour
{
    private TMPro.TMP_Text tooltip;

    void Awake()
    {
        tooltip = GetComponentInChildren<TMPro.TMP_Text>();
        gameObject.SetActive(false);
    }

    public void GenerateTooltip(Item item)
    {
        string text = item.Title + " " + item.Description;
        tooltip.text = text;
        gameObject.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
