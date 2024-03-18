using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class OptionsHolder : MonoBehaviour
{
    [SerializeField] private Canvas optionsCanvas;

    private InputAction optionsToggle;
    private UnityAction onVisibleOptionsByDefault;
    private UnityAction onDisableOptionsByDefault;

    private void OnValidate()
    {
        optionsCanvas = GetComponent<Canvas>();
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        optionsToggle = PlayerUI.Instance.InputActions.Gameplay.Options;
        optionsToggle.performed += OnOptionsToggle;
        onVisibleOptionsByDefault += OnVisibleOptionsByDefault;
        EventManager.StartListening(UnityEventName.OptionsVisibleByDefault, onVisibleOptionsByDefault);
        onDisableOptionsByDefault += OnDisableOptionsByDefault;
        EventManager.StartListening(UnityEventName.DisableOptionsByDefault, onDisableOptionsByDefault);
    }

    private void OnOptionsToggle(InputAction.CallbackContext _)
    {
        optionsCanvas.enabled = !optionsCanvas.enabled;
    }

    private void OnVisibleOptionsByDefault()
    {
        optionsCanvas.enabled = true;
    }

    private void OnDisableOptionsByDefault()
    {
        optionsCanvas.enabled = false;
    }

    private void OnDisable()
    {
        optionsToggle.performed -= OnOptionsToggle;
        EventManager.StopListening(UnityEventName.OptionsVisibleByDefault, onVisibleOptionsByDefault);
        EventManager.StopListening(UnityEventName.DisableOptionsByDefault, onDisableOptionsByDefault);
    }
}
