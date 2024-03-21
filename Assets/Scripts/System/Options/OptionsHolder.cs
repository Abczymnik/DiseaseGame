using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class OptionsHolder : MonoBehaviour
{
    [SerializeField] private Canvas optionsCanvas;
    [SerializeField] private TMP_FontAsset menuFont; //Referenced in editor
    [SerializeField] private TMP_FontAsset inGameFont; //Referenced in editor
    [SerializeField] private Volume backgroundBlur;

    private InputAction optionsEnable;
    private InputAction optionsDisable;
    private UnityAction onVisibleOptionsByDefault;
    private UnityAction onDisableOptionsByDefault;

    private void OnValidate()
    {
        optionsCanvas = GetComponent<Canvas>();
        backgroundBlur = GetComponentInChildren<Volume>(true);
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SetupListeners();
    }

    private void OnOptionsEnable(InputAction.CallbackContext _)
    {
        BlurBackground(true);
        optionsCanvas.enabled = true;
        CursorSwitch.ShowCursor();
        CursorSwitch.LockCursorVisibility(true);
        PlayerUI.SwitchActionMap(PlayerUI.Instance.InputActions.Options);
    }

    private void OnOptionsDisable(InputAction.CallbackContext _)
    {
        BlurBackground(false);
        optionsCanvas.enabled = false;
        CursorSwitch.LockCursorVisibility(false);
        CursorSwitch.SwitchSkin(CursorName.Standard);
        CursorSwitch.ShowCursor();
        PlayerUI.SwitchActionMap(PlayerUI.Instance.InputActions.Gameplay);
    }

    private void OnVisibleOptionsByDefault()
    {
        optionsCanvas.enabled = true;
    }

    private void OnDisableOptionsByDefault()
    {
        optionsCanvas.enabled = false;
    }

    private void OnActiveSceneChange(Scene _, Scene next)
    {
        optionsCanvas.worldCamera = Camera.main;
        int newtSceneIndex = next.buildIndex;

        switch(newtSceneIndex)
        {
            case 0:
                ChangeFontAsset(menuFont);
                break;
            case 1:
                break;
            default:
                ChangeFontAsset(inGameFont);
                break;
        }
    }

    private void ChangeFontAsset(TMP_FontAsset font)
    {
        TextMeshProUGUI[] optionsTextComponents = GetComponentsInChildren<TextMeshProUGUI>(true);
        foreach (var textComponent in optionsTextComponents)
        {
            textComponent.font = font;
        }
    }

    private void BlurBackground(bool active)
    {
        if(active)
        {
            UIHelper.DisableGUI();
            backgroundBlur.enabled = true;
        }

        else
        {
            UIHelper.EnableGUI();
            backgroundBlur.enabled = false;
        }
    }

    private void SetupListeners()
    {
        optionsEnable = PlayerUI.Instance.InputActions.Gameplay.Options;
        optionsEnable.performed += OnOptionsEnable;
        optionsDisable = PlayerUI.Instance.InputActions.Options.Escape;
        optionsDisable.performed += OnOptionsDisable;
        onVisibleOptionsByDefault += OnVisibleOptionsByDefault;
        EventManager.StartListening(UnityEventName.OptionsVisibleByDefault, onVisibleOptionsByDefault);
        onDisableOptionsByDefault += OnDisableOptionsByDefault;
        EventManager.StartListening(UnityEventName.DisableOptionsByDefault, onDisableOptionsByDefault);
        SceneManager.activeSceneChanged += OnActiveSceneChange;
    }

    private void OnDisable()
    {
        optionsEnable.performed -= OnOptionsEnable;
        optionsDisable.performed -= OnOptionsDisable;
        EventManager.StopListening(UnityEventName.OptionsVisibleByDefault, onVisibleOptionsByDefault);
        EventManager.StopListening(UnityEventName.DisableOptionsByDefault, onDisableOptionsByDefault);
        SceneManager.activeSceneChanged -= OnActiveSceneChange;
    }
}
