using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.UI;

public sealed class SceneLoaderManager : MonoBehaviour
{
    public static SceneLoaderManager Instance { get; private set; }

    [SerializeField] private Slider loadingSlider; //inactive -> referenced in editor
    private UnityAction<object> onChangeSceneWish;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        onChangeSceneWish += OnChangeSceneWish;
        EventManager.StartListening(TypedEventName.ChangeSceneWish, onChangeSceneWish);
        SceneManager.activeSceneChanged += OnActiveSceneChange;
    }

    private void OnChangeSceneWish(object changeSceneWishData)
    {
        int wishSceneindex = (int)changeSceneWishData;

        switch (wishSceneindex)
        {
            case 1:
                StartCoroutine(WaitBeforeSceneChange(3f, wishSceneindex));
                SceneDeactivationTransition();
                EnableLoadingSlider();
                StartCoroutine(FakeLoadingSlider(3f));
                break;
            case 2:
                break;
            default:
                StartCoroutine(WaitBeforeSceneChange(3f, wishSceneindex));
                SceneDeactivationTransition();
                break;
        }
    }

    private void OnActiveSceneChange(Scene current, Scene next)
    {
        int sceneIndex = next.buildIndex;

        switch (sceneIndex)
        {
            case 0:
                EventManager.TriggerEvent(UnityEventName.OptionsVisibleByDefault);
                SceneActivationTransition();
                break;
            case 1:
                EventManager.TriggerEvent(UnityEventName.DisableOptionsByDefault);
                DisableAndResetLoadingSlider();
                break;
            default:
                EventManager.TriggerEvent(UnityEventName.DisableOptionsByDefault);
                SceneActivationTransition();
                break;
        }
    }

    private void SceneActivationTransition()
    {
        EnvironmentManager.DimScene(0, 1);
        EnvironmentManager.BrightenScene(3);
    }

    private void SceneDeactivationTransition()
    {
        EnvironmentManager.DimScene(3);
    }

    private void DisableAndResetLoadingSlider()
    {
        loadingSlider.transform.parent.gameObject.SetActive(false);
        loadingSlider.value = 0;
    }

    private void EnableLoadingSlider()
    {
        loadingSlider.transform.parent.gameObject.SetActive(true);
    }

    IEnumerator WaitBeforeSceneChange(float seconds, int nextSceneIndex)
    {
        yield return new WaitForSeconds(seconds);

        SceneManager.LoadScene(nextSceneIndex);
    }

    IEnumerator FakeLoadingSlider(float seconds)
    {
        EnableLoadingSlider();

        float timer = 0f;
        float timeForChanges = seconds;

        while(timer < timeForChanges)
        {
            timer += Time.deltaTime;

            loadingSlider.value = Mathf.Lerp(0, 1, timer / timeForChanges);
            yield return null;
        }
    }

    private void OnDisable()
    {
        EventManager.StopListening(TypedEventName.ChangeSceneWish, onChangeSceneWish);
    }
}
