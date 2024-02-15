using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Inventory Test Item")]
public class InventoryTestItem : ScriptableObject
{
    [field: SerializeField] public int ID { get; private set; }
    [field: SerializeField] public string ItemName { get; private set; }
    [field: SerializeField] public int MaxStackSize { get; private set; }
    [field: TextArea(2,4), SerializeField] public string Description { get; private set; }
    [field: SerializeField] public Material Icon { get; private set; }
}
