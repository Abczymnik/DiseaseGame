using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public sealed class EnvironmentManager: MonoBehaviour
{
    public static EnvironmentManager Instance { get; private set; }

    private List<IDimmable> dimmables;
    private UnityAction<object> onNewDimmable;
    private UnityAction<object> onRemoveDimmable;

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

    public void Init()
    {
        dimmables = new List<IDimmable>();
        onNewDimmable += OnNewDimmable;
        EventManager.StartListening(TypedEventName.NewDimmable, onNewDimmable);
        onRemoveDimmable += OnRemoveDimmable;
        EventManager.StartListening(TypedEventName.RemoveDimmable, onRemoveDimmable);
    }

    private void OnNewDimmable(object newDimmableData)
    {
        IDimmable newDimmable = (IDimmable)newDimmableData;
        dimmables.Add(newDimmable);
    }

    private void OnRemoveDimmable(object dimmableToRemoveData)
    {
        IDimmable dimmableToRemove = (IDimmable)dimmableToRemoveData;
        if (dimmables.Contains(dimmableToRemove)) dimmables.Remove(dimmableToRemove);
    }

    public static void DimScene(float seconds, float dimPercentage)
    {
        Instance.StartCoroutine(Instance.DimSceneCoroutine(seconds, dimPercentage));
    }

    public static void DimScene(float seconds)
    {
        DimScene(seconds, 1);
    }

    public static void BrightenScene(float seconds, float brightenPercentage)
    {
        Instance.StartCoroutine(Instance.DimSceneCoroutine(seconds, 1 - brightenPercentage));
    }

    public static void BrightenScene(float seconds)
    {
        BrightenScene(seconds, 1);
    }

    private IEnumerator DimSceneCoroutine(float timeForChanges, float finalDimWish)
    {
        float timer = 0f;
        float currentDimWish;
        float[] dimmablesStartDim = new float[dimmables.Count];

        for(int i=0; i<dimmables.Count; i++)
        {
            dimmablesStartDim[i] = dimmables[i].CurrentDim();
        }

        while (timer < timeForChanges)
        {
            timer += Time.deltaTime;
            for(int i=0; i<dimmables.Count; i++)
            {
                currentDimWish = Mathf.Lerp(dimmablesStartDim[i], finalDimWish, timer / timeForChanges);
                dimmables[i].Dim(currentDimWish);
            }

            yield return null;
        }
    }

    private void OnDisable()
    {
        EventManager.StopListening(TypedEventName.NewDimmable, onNewDimmable);
        EventManager.StopListening(TypedEventName.RemoveDimmable, onRemoveDimmable);
    }
}
