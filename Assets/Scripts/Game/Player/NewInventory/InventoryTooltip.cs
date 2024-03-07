using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryTooltip : MonoBehaviour
{
    [SerializeField] private Image tooltipSprite;
    [SerializeField] private TextMeshProUGUI noteName;
    [SerializeField] private TextMeshProUGUI noteDescription;
    private NoteItem lastNoteData;

    private void OnValidate()
    {
        tooltipSprite = GetComponentInChildren<Image>();
        noteName = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        noteDescription = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    private void Awake()
    {
        tooltipSprite.color = Color.clear;
        noteName.text = "";
        noteDescription.text = "";
    }

    public void SwitchTooltip(NoteItem noteData)
    {
        if(noteData == lastNoteData) ClearTooltip();
        else
        {
            tooltipSprite.color = Color.white;
            noteName.text = noteData.ItemName;
            noteDescription.text = noteData.Description;
            lastNoteData = noteData;
        }
    }

    public void ClearTooltip()
    {
        if (lastNoteData == null) return;
        tooltipSprite.color = Color.clear;
        noteName.text = "";
        noteDescription.text = "";
        lastNoteData = null;
    }
}
