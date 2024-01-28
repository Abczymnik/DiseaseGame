using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroudZombiesBehaviour : MonoBehaviour
{
    private List<BasicZombie> zombiesList = new List<BasicZombie>();
    private List<Vector3> startPoints = new List<Vector3>();
    private List<Vector3> endPoints = new List<Vector3>();
    private const float ZOMBIE_MOVEMENT_REFRESH_RATE = 0.1f;

    private void Start()
    {
        FillZombieList();
        FillStartEndPoints();
        SetZombieInitMovement();
        StartCoroutine(ZombiesMovementCoroutine());
    }

    private void FillZombieList()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform zombie = transform.GetChild(i);
            if (zombie.TryGetComponent(out SpecialZombie _)) continue;
            zombiesList.Add(zombie.GetComponent<BasicZombie>());
        }
    }

    private void FillStartEndPoints()
    {
        foreach (BasicZombie zombie in zombiesList)
        {
            startPoints.Add(zombie.transform.position);
            endPoints.Add(zombie.transform.position + zombie.transform.forward * Random.Range(4f, 7f) + Vector3.forward * Random.Range(2f, 4f));
        }
    }

    private void SetZombieInitMovement()
    {
        for(int i = 0; i < zombiesList.Count; i++)
        {
            zombiesList[i].ZombieAnimator.SetBool("move", true);
            zombiesList[i].ZombieAnimator.SetFloat("velocity", 1);
            zombiesList[i].ZombieAgent.SetDestination(endPoints[i]);
        }
    }

    private void SwapMoveDirection(int i)
    {
        Vector3 temp = startPoints[i];
        startPoints[i] = endPoints[i];
        endPoints[i] = temp;
    }

    private IEnumerator ZombiesMovementCoroutine()
    {
        while(true)
        {
            for (int i = 0; i < zombiesList.Count; i++)
            {
                if (Vector3.Distance(zombiesList[i].transform.position, zombiesList[i].ZombieAgent.destination) < 0.1f)
                {
                    SwapMoveDirection(i);
                    zombiesList[i].ZombieAgent.SetDestination(endPoints[i]);
                }
            }
            yield return new WaitForSeconds(ZOMBIE_MOVEMENT_REFRESH_RATE);
        }
    }
}
