using UnityEngine;

[System.Serializable]
public class InventoryHolder : MonoBehaviour
{
    [SerializeField] private int inventorySize = 10;
    [field: SerializeField] public InventorySystem InventorySystem { get; protected set; }

    private void Awake()
    {
        InventorySystem = new InventorySystem(inventorySize);
    }
}
