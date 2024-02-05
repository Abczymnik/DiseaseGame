using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GAgent : MonoBehaviour
{
    private List<GAction> actions = new List<GAction>();
    public Dictionary<SubGoal, int> Goals { get; private set; } = new Dictionary<SubGoal, int>();
    public WorldStates Beliefs { get; private set; } = new WorldStates();
    public GAction CurrentAction { get; private set; }

    private GPlanner planner;
    private Queue<GAction> actionQueue;
    private SubGoal currentGoal;
    private Coroutine decisionMakerCoroutine;

    private float currentActionRefreshRate;

    private readonly Dictionary<string, float> actionsRefreshRateDict = new Dictionary<string, float>()
    {
        {"Search", 1f},
        {"Lost", 0.5f},
        {"StaticMovement", 1f},
        {"DynamicMovement", 0f},
        {"Attack", 0.1f}
    };

    protected void Init()
    {
        GAction[] agentAvailableActions = GetComponents<GAction>();
        foreach (GAction action in agentAvailableActions)
        {
            actions.Add(action);
        }

        decisionMakerCoroutine = StartCoroutine(DecisionMaker());
    }

    IEnumerator DecisionMaker()
    {
        while(true)
        {
            if(CurrentAction != null && CurrentAction.running)
            {
                if (CurrentAction.Success())
                {
                    CurrentAction.Agent.ResetPath();
                    CompleteAction();
                    continue;
                }

                if (!CurrentAction.Func()) 
                {
                    actionQueue = null;
                    CurrentAction.running = false;
                    continue;
                }

                yield return new WaitForSeconds(currentActionRefreshRate);

                continue;
            }

            if (planner == null || actionQueue == null) FindNewPlan();

            if (actionQueue != null && actionQueue.Count == 0) CurrGoalAchieved();

            if (actionQueue != null && actionQueue.Count > 0) TrySelectNextAction();

            yield return null;

        }
    }

    private void CompleteAction()
    {
        CurrentAction.running = false;
        CurrentAction.PostPerform();
    }

    private float GetRefreshRate(string actionType)
    {
        if(actionsRefreshRateDict.TryGetValue(actionType, out float refreshRate))
        {
            return refreshRate;
        }
        return 0f;
    }

    private void FindNewPlan()
    {
        planner = new GPlanner();
        var sortedGoals = from entry in Goals orderby entry.Value descending select entry;

        foreach (KeyValuePair<SubGoal, int> sg in sortedGoals)
        {
            actionQueue = planner.Plan(actions, sg.Key.SGoal, Beliefs);
            if (actionQueue != null)
            {
                currentGoal = sg.Key;
                break;
            }
        }
    }

    private void TrySelectNextAction()
    {
        CurrentAction = actionQueue.Dequeue();

        if (CurrentAction.PrePerform())
        {
            CurrentAction.running = true;
            currentActionRefreshRate = GetRefreshRate(CurrentAction.ActionType.ToString());
        }

        else actionQueue = null;
    }

    private void CurrGoalAchieved()
    {
        if (currentGoal.RemoveAfter) Goals.Remove(currentGoal);
        planner = null;
    }

    protected void PlanStop()
    {
        StopCoroutine(decisionMakerCoroutine);
        decisionMakerCoroutine = null;
    }
}
