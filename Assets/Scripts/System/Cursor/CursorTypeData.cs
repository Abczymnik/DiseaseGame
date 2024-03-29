using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CursorType")]
public class CursorTypeData : ScriptableObject
{
    [field: SerializeField] public string CursorName { get; private set; }
    [field: SerializeField] public Material CursorSkin { get; private set; }
}