using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TestItemPickUp : MonoBehaviour
{
    public InventoryTestItem itemData;

    public Collider thisCollider;

    private void OnValidate()
    {
        thisCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var inventory = other.transform.GetComponent<PlayerInventoryHolder>();
        if (!inventory) return;

        inventory.PlayerInventorySystem.AddToInventory(itemData, 1);
        Destroy(this.gameObject);
    }
}
