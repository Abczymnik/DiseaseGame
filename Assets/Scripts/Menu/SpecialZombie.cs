using UnityEngine;
using UnityEngine.Events;

public class SpecialZombie : BasicZombie
{
    private UnityAction<object> onChangeSceneWish;

    private void OnEnable()
    {
        onChangeSceneWish += OnChangeLevelWish;
        EventManager.StartListening(TypedEventName.ChangeSceneWish, onChangeSceneWish);
    }

    private void OnChangeLevelWish(object _)
    {
        ZombieAnimator.SetBool("move", true);
        ZombieAnimator.SetFloat("distanceToPlayer", 6f);
        ZombieAgent.SetDestination(new Vector3(32, 2, 0));
    }

    private void OnDisable()
    {
        EventManager.StopListening(TypedEventName.ChangeSceneWish, onChangeSceneWish);
    }
}