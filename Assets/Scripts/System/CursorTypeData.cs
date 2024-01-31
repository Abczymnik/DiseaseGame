using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CursorType")]
public class CursorTypeData : ScriptableObject
{
    [field: SerializeField] public string CursorName { get; private set; }
    [field: SerializeField] public Sprite CursorSprite { get; private set; }
    [field: SerializeField] public Texture2D CursorTexture { get; private set; }
}