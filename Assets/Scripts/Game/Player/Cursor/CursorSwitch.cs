using System.Collections.Generic;
using UnityEngine;

public class CursorSwitch : MonoBehaviour
{ 
    public static CursorSwitch Instance { get; private set; }
    [field: SerializeField] public List<CursorTypeData> CursorTypeDatas { get; private set; } 
    private SpriteRenderer spriteRend;
    private string actualSkinName;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }

        else
        {
            Instance = this;
            Cursor.visible = false;
            SetSpriteRenderer();
        } 
    }
    
    private void SetSpriteRenderer() 
    { 
        spriteRend = GetComponent<SpriteRenderer>(); 
    }
    
    public static void SwitchSkin(string cursorSkinName) 
    { 
        if (cursorSkinName == Instance.actualSkinName) return;

        foreach(CursorTypeData cursor in Instance.CursorTypeDatas) 
        {
            if (cursor.CursorName == cursorSkinName)
            {
                Instance.spriteRend.sprite = cursor.CursorSprite;
                Instance.spriteRend.material.mainTexture = cursor.CursorTexture;
                Instance.actualSkinName = cursor.CursorName;
                return;
            }
        }
    }

    public static void HideCursor()
    {
        if (Instance.spriteRend == null) return;
        Instance.spriteRend.enabled = false; 
    } 
    
    public static void ShowCursor() 
    { 
        if (Instance.spriteRend == null) return;
        Instance.spriteRend.enabled = true;
    } 
}