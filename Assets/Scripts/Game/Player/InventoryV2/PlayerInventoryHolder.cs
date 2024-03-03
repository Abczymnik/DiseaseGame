using UnityEngine;

public class PlayerInventoryHolder : InventoryHolder
{
    [SerializeField] protected int playerInventorySize = 48;
    [field: SerializeField] public InventorySystem PlayerInventorySystem {get; protected set; }

    protected override void Awake()
    {
        PlayerInventorySystem = new InventorySystem(playerInventorySize);
    }
}
