using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine;

public class UIHelper : MonoBehaviour
{
    public static UIHelper Instance { get; private set;}

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
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
