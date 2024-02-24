using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MouseItemData : MonoBehaviour
{
    [field: SerializeField] public Image ItemSprite { get; private set; }
    [field: SerializeField] public TextMeshProUGUI ItemCount { get; private set; }
    [field: SerializeField] public InventorySlot InventorySlot { get; private set; }

    private InputAction mouseLeftButtonInput;

    private void OnValidate()
    {
        ItemSprite = GetComponentInChildren<Image>();
        ItemCount = GetComponentInChildren<TextMeshProUGUI>();
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
    }

    private void OnLeftButtonPress(InputAction.CallbackContext _)
    {
        if (!UIHelper.IsPointerOverUI("InventoryUI")) ClearSlot();
    }

    public void UpdateMouseSlot(InventorySlot inventorySlot)
    {
        StartCoroutine(LateUpdateCoroutine());
        this.InventorySlot.AssignItem(inventorySlot);
        ItemSprite.material = inventorySlot.ItemData.Icon;
        ItemCount.text = inventorySlot.StackSize.ToString();
        ItemSprite.color = Color.white;
    }

    public void ClearSlot()
    {
        StopCoroutine(LateUpdateCoroutine());
        this.InventorySlot.ClearSlot();
        ItemCount.text = "";
        ItemSprite.color = Color.clear;
        ItemSprite.material = null;
    }

    private IEnumerator LateUpdateCoroutine()
    {
        transform.localPosition = Mouse.current.position.ReadValue();
        while (true)
        {
            yield return new WaitForEndOfFrame();

            transform.localPosition = Mouse.current.position.ReadValue();
        }
    }

    private void OnDisable()
    {
        mouseLeftButtonInput.performed -= OnLeftButtonPress;
    }
}
