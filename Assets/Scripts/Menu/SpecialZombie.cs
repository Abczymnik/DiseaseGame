using UnityEngine;
using UnityEngine.Events;

public class SpecialZombie : BasicZombie
{
    private UnityAction onLevelChange;

    private void OnEnable()
    {
        onLevelChange += OnLevelChange;
        EventManager.StartListening(UnityEventName.ChangeLevel, onLevelChange);
    }

    private void OnLevelChange()
    {
        ZombieAnimator.SetBool("move", true);
        ZombieAnimator.SetFloat("distanceToPlayer", 6f);
        ZombieAgent.SetDestination(new Vector3(32, 2, 0));
    }

    private void OnDisable()
    {
        EventManager.StopListening(UnityEventName.ChangeLevel, onLevelChange);
    }
}