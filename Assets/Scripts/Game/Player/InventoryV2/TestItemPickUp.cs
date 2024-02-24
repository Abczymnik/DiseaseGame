using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TestItemPickUp : MonoBehaviour
{
    [SerializeField] private InventoryTestItem itemData;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PlayerInventoryHolder inventory))
        {
            inventory.PlayerInventorySystem.AddToInventory(itemData, 1);
            Destroy(this.gameObject);
        }
    }
}
