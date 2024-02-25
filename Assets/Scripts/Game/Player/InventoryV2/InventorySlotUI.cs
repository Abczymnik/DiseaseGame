using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image itemSprite;
    [SerializeField] private TextMeshProUGUI itemCount;
    [field: SerializeField] public InventorySlot InventorySlot { get; private set; }
    [field: SerializeField] public InventoryDisplay ParentDisplay { get; private set; }

    private Coroutine mouseInputHandlerCoroutine;
    private float requiredHoldButtonTime = 0.25f;
    private float currentHoldButtonTime;


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
        if (ParentDisplay.MouseInventoryItem.InventorySlot.ItemData != null) ParentDisplay.SlotClicked(this);
        else
        {
            if (mouseInputHandlerCoroutine != null) StopCoroutine(mouseInputHandlerCoroutine);
            mouseInputHandlerCoroutine = StartCoroutine(MouseHoldButtonCoroutine());
        }
    }

    public void OnPointerUp(PointerEventData _)
    {
        if (mouseInputHandlerCoroutine != null)
        {
            StopCoroutine(mouseInputHandlerCoroutine);
            mouseInputHandlerCoroutine = null;
        }
    }

    private IEnumerator MouseHoldButtonCoroutine()
    {
        currentHoldButtonTime = 0f;
        while(currentHoldButtonTime < requiredHoldButtonTime)
        {
            currentHoldButtonTime += Time.deltaTime;
            yield return null;
        }

        ParentDisplay.SlotClicked(this);
        currentHoldButtonTime = 0f;
        mouseInputHandlerCoroutine = null;
    }
}
