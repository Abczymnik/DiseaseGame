using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    [SerializeField] private Transform zombiesTrans;

    public void Awake()
    {
        if (zombiesTrans == null) zombiesTrans = GameObject.Find("/Zombie").transform;
        int zombieCount = zombiesTrans.childCount;

        for(int i = 1; i<zombieCount+1; i++)
        {
            GameObject spawnPoint = new GameObject("SpawnPoint " + i.ToString());
            spawnPoint.transform.position = zombiesTrans.GetChild(i-1).position;
            spawnPoint.tag = "Spawn point";
            spawnPoint.transform.SetParent(transform);
        }
    }
}
