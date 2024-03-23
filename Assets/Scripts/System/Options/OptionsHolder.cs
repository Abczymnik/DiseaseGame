using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsHolder : MonoBehaviour
{
    [SerializeField] private Canvas optionsCanvas;
    [SerializeField] private Button[] mainButtons;
    [SerializeField] private TMP_FontAsset menuFont; //Referenced in editor
    [SerializeField] private TMP_FontAsset inGameFontStandardSize; //Referenced in editor
    [SerializeField] private TMP_FontAsset inGameFontSmallSize; //Referenced in editor
    [SerializeField] private Volume backgroundBlur;

    private InputAction optionsEnable;
    private InputAction optionsDisable;
    private UnityAction onVisibleOptionsByDefault;
    private UnityAction onDisableOptionsByDefault;
    private UnityAction<object> onChangeSceneWish;

    private const float SMALL_FONT_SIZE = 24f;

    private void OnValidate()
    {
        optionsCanvas = GetComponent<Canvas>();
        mainButtons = GetComponentsInChildren<Button>();
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

    private void OnChangeSceneWish(object _)
    {
        BlurBackground(false);
        DisableButtonsInteraction();
    }

    private void EnableButtonsInteraction()
    {
        foreach (var button in mainButtons)
        {
            button.GetComponent<CanvasGroup>().blocksRaycasts = true;
            button.interactable = true;
        }
    }

    private void DisableButtonsInteraction()
    {
        foreach(var button in mainButtons)
        {
            button.GetComponent<CanvasGroup>().blocksRaycasts = false;
            button.interactable = false;
        }
    }

    private void OnActiveSceneChange(Scene _, Scene next)
    {
        BlurBackground(false);
        EnableButtonsInteraction();
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
                ChangeFontAsset(inGameFontStandardSize);
                break;
        }
    }

    private void ChangeFontAsset(TMP_FontAsset font)
    {
        TextMeshProUGUI[] optionsTextComponents = GetComponentsInChildren<TextMeshProUGUI>(true);
        foreach (var textComponent in optionsTextComponents)
        {
            if(font == menuFont) textComponent.font = font;
            else
            {
                if (textComponent.fontSize > SMALL_FONT_SIZE) textComponent.font = font;
                else textComponent.font = inGameFontSmallSize;
            }
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
        onChangeSceneWish += OnChangeSceneWish;
        EventManager.StartListening(TypedEventName.ChangeSceneWish, onChangeSceneWish);
        SceneManager.activeSceneChanged += OnActiveSceneChange;
    }

    private void OnDisable()
    {
        optionsEnable.performed -= OnOptionsEnable;
        optionsDisable.performed -= OnOptionsDisable;
        EventManager.StopListening(UnityEventName.OptionsVisibleByDefault, onVisibleOptionsByDefault);
        EventManager.StopListening(UnityEventName.DisableOptionsByDefault, onDisableOptionsByDefault);
        EventManager.StopListening(TypedEventName.ChangeSceneWish, onChangeSceneWish);
        SceneManager.activeSceneChanged -= OnActiveSceneChange;
    }
}
