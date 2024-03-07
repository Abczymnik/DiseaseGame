using UnityEngine.InputSystem;

public sealed class PlayerUI
{
    private static readonly object instanceLock = new object();
    public PlayerControls InputActions { get; private set; }

    private static PlayerUI _instance;
    public static PlayerUI Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (instanceLock)
                {
                    if (_instance == null) _instance = new PlayerUI();
                }
            }
            return _instance;
        }
    }

    private PlayerUI()
    {
        InputActions = new PlayerControls();
    }

    public static void SwitchActionMap(InputActionMap actionMap)
    {
        if(actionMap.enabled) { return; }

        Instance.InputActions.Disable();
        actionMap.Enable();
    }

    public static void BlockInput()
    {
        Instance.InputActions.Disable();
    }
}
 