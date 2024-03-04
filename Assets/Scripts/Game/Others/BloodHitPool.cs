using System.Collections.Generic;
using UnityEngine;

public sealed class BloodHitPool : MonoBehaviour
{
    public static BloodHitPool Instance { get; private set; }

    [SerializeField] private List<GameObject> pooledObjects;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else Instance = this;
    }

    public GameObject GetPooledObject()
    {
        for(int i=0; i < pooledObjects.Count; i++)
        {
            if(!pooledObjects[i].activeInHierarchy) return pooledObjects[i];
        }

        return null;
    }
}
