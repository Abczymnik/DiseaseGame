using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventoryHolder : InventoryHolder
{
    [SerializeField] protected int playerInventorySize = 48;
    [field: SerializeField] public InventorySystem PlayerInventorySystem {get; protected set; }

    protected override void Awake()
    {
        PlayerInventorySystem = new InventorySystem(playerInventorySize);
    }
}
