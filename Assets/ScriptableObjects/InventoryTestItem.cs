using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Inventory Test Item")]
public class InventoryTestItem : ScriptableObject
{
    public int ID { get; private set; }
    public string ItemName { get; private set; }
    [field:TextArea(2,4)] public string Description { get; private set; }
    public Sprite Icon { get; private set; }
}
