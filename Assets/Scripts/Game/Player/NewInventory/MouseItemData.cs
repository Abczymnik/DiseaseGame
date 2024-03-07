using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MouseItemData : MonoBehaviour
{
    [field: SerializeField] public Image ItemSprite { get; private set; }
    [field: SerializeField] public TextMeshProUGUI ItemCount { get; private set; }
    [field: SerializeField] public InventorySlot InventorySlot { get; private set; }
    [field: SerializeField] public InventoryTooltip Tooltip { get; private set; }

    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private CanvasScaler scaler;
    private float screenWidthScalar;
    private float screenHeightScalar;

    private InputAction mouseLeftButtonInput;

    private void OnValidate()
    {
        ItemSprite = transform.GetChild(0).GetComponent<Image>();
        ItemCount = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        Tooltip = transform.GetComponentInChildren<InventoryTooltip>();

        rectTransform = GetComponent<RectTransform>();
        scaler = GetComponentInParent<CanvasScaler>();
    }

    private void OnEnable()
    {
        mouseLeftButtonInput = PlayerUI.Instance.InputActions.Gameplay.Select;
        mouseLeftButtonInput.performed += OnLeftButtonPress;
    }

    private void Awake()
    {
        ItemSprite.color = Color.clear;
        ItemCount.text = "";
        screenWidthScalar = scaler.referenceResolution.x / Screen.width;
        screenHeightScalar = scaler.referenceResolution.y / Screen.height;
    }

    private void LateUpdate()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        rectTransform.anchoredPosition = new Vector2(mousePosition.x * screenWidthScalar, mousePosition.y * screenHeightScalar);
    }

    private void OnLeftButtonPress(InputAction.CallbackContext _)
    {
        if (InventorySlot.ItemData != null && !UIHelper.IsPointerOverUI("InventoryUI")) ClearSlot();
    }

    public void UpdateMouseSlot(InventorySlot inventorySlot)
    {
        this.InventorySlot.AssignItem(inventorySlot);
        ItemSprite.material = inventorySlot.ItemData.Icon;
        if(inventorySlot.StackSize > 1) ItemCount.text = inventorySlot.StackSize.ToString();
        ItemSprite.color = Color.white;
    }

    public void ClearSlot()
    {
        this.InventorySlot.ClearSlot();
        ItemCount.text = "";
        ItemSprite.color = Color.clear;
        ItemSprite.material = null;
    }

    private void OnDisable()
    {
        mouseLeftButtonInput.performed -= OnLeftButtonPress;
    }
}
