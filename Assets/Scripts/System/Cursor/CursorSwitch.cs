using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorSwitch : MonoBehaviour
{
    public static CursorSwitch Instance { get; private set; }
    [field: SerializeField] public List<CursorTypeData> CursorTypeDatas { get; private set; }
    [SerializeField] private Image cursorImage;
    private CursorName actualSkinName;
    private bool isLocked;

    private void OnValidate()
    {
        cursorImage = GetComponentInChildren<Image>();
    }

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
        }
    }

    public static void SwitchSkin(CursorName cursorSkinName)
    {
        if (cursorSkinName == Instance.actualSkinName) return;

        foreach (CursorTypeData cursor in Instance.CursorTypeDatas)
        {
            if (cursor.CursorName == cursorSkinName.ToString())
            {
                Instance.cursorImage.material = cursor.CursorSkin;
                Instance.actualSkinName = cursorSkinName;
                return;
            }
        }
    }

    public static void HideCursor()
    {

        if (Instance.cursorImage == null || Instance.isLocked) return;
        Instance.cursorImage.color = Color.clear;
    }

    public static void ShowCursor()
    {
        if (Instance.cursorImage == null || Instance.isLocked) return;
        Instance.cursorImage.color = Color.white;
    }

    public static void LockCursorVisibility(bool active)
    {
        Instance.isLocked = active;
    }
}