using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TestItemPickUp : MonoBehaviour
{
    public float PickUpRange = 2f;
    public InventoryTestItem itemData;

    public Collider thisCollider;

    private void OnValidate()
    {
        thisCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var inventory = other.transform.GetComponent<InventoryHolder>();
        if (!inventory) return;

        inventory.InventorySystem.AddToInventory(itemData, 1);
        Destroy(this.gameObject);
    }
}
