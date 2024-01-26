using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GAgent : MonoBehaviour
{
    private List<GAction> actions = new List<GAction>();
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();
    public WorldStates beliefs = new WorldStates();

    public GAction CurrentAction { get; private set; }
    private GPlanner planner;
    private Queue<GAction> actionQueue;
    private SubGoal currentGoal;
    private Coroutine decisionMakerCoroutine;

    private Dictionary<string, float> refreshRateDict = new Dictionary<string, float>()
    {
        {"Search", 1f},
        {"Lost", 0.5f},
        {"Static movement", 1f},
        {"Dynamic movement", 0f},
        {"Attack", 0.1f}
    };

    public void Start()
    {
        GAction[] acts = GetComponents<GAction>();
        foreach (GAction a in acts)
        {
            actions.Add(a);
        }

        decisionMakerCoroutine = StartCoroutine(DecisionMaker());
    }

    private void CompleteAction()
    {
        CurrentAction.running = false;
        CurrentAction.PostPerform();
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
                }

                yield return new WaitForSeconds(GetRefreshRate(CurrentAction.ActionType));

                continue;
            }

            if (planner == null || actionQueue == null) FindNewPlan();

            if (actionQueue != null && actionQueue.Count == 0) CurrGoalAchieved();

            if (actionQueue != null && actionQueue.Count > 0) TrySelectNextAction();

            yield return null;

        }
    }

    private float GetRefreshRate(string actionType)
    {
        bool hasValue = refreshRateDict.TryGetValue(actionType, out float refreshRate);
        if (hasValue) return refreshRate;
        return 0f;
    }

    private void FindNewPlan()
    {
        planner = new GPlanner();

        var sortedGoals = from entry in goals orderby entry.Value descending select entry;

        foreach (KeyValuePair<SubGoal, int> sg in sortedGoals)
        {
            actionQueue = planner.Plan(actions, sg.Key.sgoals, beliefs);
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

        if (CurrentAction.PrePerform()) CurrentAction.running = true;

        else actionQueue = null;
    }

    private void CurrGoalAchieved()
    {
        if (currentGoal.remove) goals.Remove(currentGoal);
        planner = null;
    }

    protected void PlanStop()
    {
        StopCoroutine(decisionMakerCoroutine);
        decisionMakerCoroutine = null;
    }
}
