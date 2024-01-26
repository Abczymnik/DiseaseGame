using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombiesMenu : MonoBehaviour
{
    private List<Transform> zombiesList = new List<Transform>();
    private List<NavMeshAgent> zombieAgents = new List<NavMeshAgent>();
    private List<Animator> zombieAnimators = new List<Animator>();
    private List<Vector3> StartPoints = new List<Vector3>();
    private List<Vector3> EndPoints = new List<Vector3>();

    private void Start()
    {
        FillZombieList();
        FillStartEndPoints();
        SetZombieAgentsAnimators();
    }

    private void Update()
    {
        ZombiesMovement();
    }

    private void FillZombieList()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "Special Zombie") continue;
            zombiesList.Add(transform.GetChild(i));
        }
    }

    private void SetZombieAgentsAnimators()
    {
        for(int i = 0; i < zombiesList.Count; i++)
        {
            zombieAgents.Add(zombiesList[i].GetComponent<NavMeshAgent>());
            zombieAnimators.Add(zombiesList[i].GetComponent<Animator>());
            zombieAgents[i].SetDestination(EndPoints[i]);
            zombieAnimators[i].SetBool("move", true);
            zombieAnimators[i].SetFloat("velocity", 1);
        }
    }

    private void FillStartEndPoints()
    {
        foreach(Transform zombie in zombiesList)
        {
            StartPoints.Add(zombie.position);
            EndPoints.Add(zombie.position + zombie.forward * Random.Range(4f,7f) + Vector3.forward * Random.Range(2f,4f));
        }
    }

    private void SwapMoveDirection(int i)
    {
        Vector3 temp = StartPoints[i];
        StartPoints[i] = EndPoints[i];
        EndPoints[i] = temp;
    }

    private void ZombiesMovement()
    {
        for(int i = 0; i<zombieAgents.Count; i++)
        {
            float distToTarget = Vector3.Distance(zombieAgents[i].transform.position, zombieAgents[i].destination);
            if(distToTarget < 0.5f)
            {
                SwapMoveDirection(i);
                zombieAgents[i].SetDestination(EndPoints[i]);
            }
        }
    }
}
