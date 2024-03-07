using UnityEngine;

public class BaseItem : ScriptableObject
{
    [field: SerializeField] public int ID { get; private set; }
    [field: SerializeField] public string ItemName { get; private set; }
    [field: SerializeField] public int MaxStackSize { get; private set; }
    [field: TextArea(2, 4), SerializeField] public string Description { get; private set; }
    [field: SerializeField] public Material Icon { get; private set; }
}
