using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField] private Image itemSprite;
    [SerializeField] private TextMeshProUGUI itemCount;
    [field: SerializeField] public InventorySlot InventorySlot { get; private set; }
    [field: SerializeField] public InventoryDisplay ParentDisplay { get; private set; }

    private Coroutine mouseHoldButtonCoroutine;
    private float requiredHoldButtonTime = 0.25f;
    private float currentHoldButtonTime;

    private Coroutine checkForDoubleClickCoroutine;
    private float requiredDoubleClickTime = 0.2f;
    private float currentDoubleClickTime;
    private bool doubleClickCandidate;


    private void OnValidate()
    {
        itemSprite = transform.GetChild(0).GetComponent<Image>();
        itemCount = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Awake()
    {
        ParentDisplay = transform.parent.GetComponent<InventoryDisplay>();
        ClearSlot();
    }

    public void Init(InventorySlot slot)
    {
        this.InventorySlot = slot;
        UpdateUISlot(slot);
    }

    public void UpdateUISlot(InventorySlot slot)
    {
        if (slot.ItemData != null)
        {
            itemSprite.material = slot.ItemData.Icon;
            itemSprite.color = Color.white;

            if (slot.StackSize > 1) itemCount.text = slot.StackSize.ToString();
            else itemCount.text = "";
        }
        else ClearSlot();
    }

    public void UpdateUISlot()
    {
        if (this.InventorySlot != null) UpdateUISlot(this.InventorySlot);
    }

    public void ClearSlot()
    {
        if (this.InventorySlot != null) this.InventorySlot.ClearSlot();

        itemSprite.color = Color.clear;
        itemCount.text = "";
    }

    public void OnPointerDown(PointerEventData _)
    {
        if (ParentDisplay.MouseInventoryItem.InventorySlot.ItemData != null) ParentDisplay.SlotSelected(this);
        else
        {
            if (this.InventorySlot.ItemData == null) return;

            if (doubleClickCandidate) HandleDoubleClick();

            else CheckForHoldButton();
        }
    }

    private void HandleDoubleClick()
    {
        this.StopCoroutine(ref checkForDoubleClickCoroutine);

        this.StopCoroutine(ref mouseHoldButtonCoroutine);

        doubleClickCandidate = false;
        ParentDisplay.UseItem(this);
    }

    public void OnPointerUp(PointerEventData _)
    {
        if (mouseHoldButtonCoroutine != null)
        {
            WaitForSecondClick();

            this.StopCoroutine(ref mouseHoldButtonCoroutine);
        }
    }

    private void WaitForSecondClick()
    {
        this.StopCoroutine(ref checkForDoubleClickCoroutine);
        checkForDoubleClickCoroutine = StartCoroutine(CheckForDoubleClick());
    }

    private IEnumerator CheckForDoubleClick()
    {
        currentDoubleClickTime = 0f;
        doubleClickCandidate = true;
        while (currentDoubleClickTime < requiredDoubleClickTime)
        {
            currentDoubleClickTime += Time.deltaTime;
            yield return null;
        }

        HandleSingleClick();
        doubleClickCandidate = false;
        checkForDoubleClickCoroutine = null;
    }

    private void CheckForHoldButton()
    {
        this.StopCoroutine(ref mouseHoldButtonCoroutine);
        mouseHoldButtonCoroutine = StartCoroutine(MouseHoldButtonCoroutine());
    }

    private IEnumerator MouseHoldButtonCoroutine()
    {
        currentHoldButtonTime = 0f;
        while (currentHoldButtonTime < requiredHoldButtonTime)
        {
            currentHoldButtonTime += Time.deltaTime;
            yield return null;
        }

        ParentDisplay.SlotSelected(this);
        currentHoldButtonTime = 0f;
        doubleClickCandidate = false;
        mouseHoldButtonCoroutine = null;
    }

    private void HandleSingleClick()
    {
        ParentDisplay.SwitchTooltip(this);
    }

    public void OnPointerExit(PointerEventData _)
    {
        if (this.InventorySlot.ItemData == null) return;
        ParentDisplay.ClearTooltip();
    }

    private void StopCoroutine(ref Coroutine coroutine)
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }
}
