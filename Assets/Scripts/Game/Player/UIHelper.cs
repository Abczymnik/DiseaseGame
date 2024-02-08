using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine;

public sealed class UIHelper
{
    private static readonly object instanceLock = new object();

    private static UIHelper _instance;
    public static UIHelper Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (instanceLock)
                {
                    if (_instance == null) _instance = new UIHelper();
                }
            }
            return _instance;
        }
    }

    private UIHelper()
    {

    }

    public static bool IsPointerOverUI()
    {
        PointerEventData pointerCurrentPosition = new PointerEventData(EventSystem.current);
        pointerCurrentPosition.position = Mouse.current.position.ReadValue();
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerCurrentPosition, results);

        foreach(RaycastResult result in results)
        {
            if (result.gameObject.layer == 5) return true;
        }

        return false;
    }

    public static void DisableGUI()
    {
        GameObject[] GUIElements = GameObject.FindGameObjectsWithTag("GUI Element");
        foreach(GameObject GUI in GUIElements)
        {
            GUI.GetComponent<Canvas>().enabled = false;
        }
    }

    public static void EnableGUI()
    {
        GameObject[] GUIElements = GameObject.FindGameObjectsWithTag("GUI Element");
        foreach (GameObject GUI in GUIElements)
        {
            GUI.GetComponent<Canvas>().enabled = true;
        }
    }
}
