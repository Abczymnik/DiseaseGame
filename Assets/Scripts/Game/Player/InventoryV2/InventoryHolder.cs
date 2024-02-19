using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InventoryHolder : MonoBehaviour
{
    [SerializeField] private int inventorySize = 10;
    [field: SerializeField] public InventorySystem InventorySystem { get; protected set; }

    public static UnityAction<InventorySystem> OnDynamicInventoryDisplayRequested;

    private void Awake()
    {
        InventorySystem = new InventorySystem(inventorySize);
    }
}
