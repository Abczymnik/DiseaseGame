using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InventoryHolder : MonoBehaviour
{
    [SerializeField] private int basicInventorySize = 12;
    [field: SerializeField] public InventorySystem BasicInventorySytem { get; protected set; }

    public static UnityAction<InventorySystem> OnDynamicInventoryDisplayRequested;

    protected virtual void Awake()
    {
        BasicInventorySytem = new InventorySystem(basicInventorySize);
    }
}
