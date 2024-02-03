using System.Collections.Generic;

public class Node
{
    public Node Parent { get; private set; }
    public float Cost { get; private set; }
    public Dictionary<string, int> State { get; private set; }
    public GAction Action { get; private set; }

    public Node(Node parent, float cost, Dictionary<string, int> allstates, GAction action)
    {
        Parent = parent;
        Cost = cost;
        State = new Dictionary<string, int>(allstates);
        Action = action;
    }

    public Node(Node parent, float cost, Dictionary<string, int> allstates, Dictionary<string, int> beliefstates, GAction action)
    {
        Parent = parent;
        Cost = cost;
        State = new Dictionary<string, int>(allstates);
        foreach(KeyValuePair<string, int> state in beliefstates)
        {
            if(!State.ContainsKey(state.Key)) State.Add(state.Key, state.Value);
        }

        Action = action;
    }
}

public class GPlanner
{
    public Queue<GAction> Plan(List<GAction> actions, Dictionary<string, int> goal, WorldStates beliefstates)
    {
        List<GAction> usableActions = new List<GAction>();
        foreach(GAction action in actions)
        {
            if(action.IsAchievable()) usableActions.Add(action);
        }

        List<Node> leaves = new List<Node>();
        Node start = new Node(null, 0, GWorld.Instance.World.States, beliefstates.States, null);

        bool success = BuildGraph(start, leaves, usableActions, goal);

        if(!success) return null;

        Node cheapest = null;
        foreach(Node leaf in leaves)
        {
            if(cheapest == null) cheapest = leaf;
            else if (leaf.Cost < cheapest.Cost) cheapest = leaf;
        }

        List<GAction> actionsTrace = new List<GAction>();
        Node node = cheapest;
        while( node != null)
        {
            if(node.Action != null) actionsTrace.Insert(0, node.Action);
            node = node.Parent;
        }

        Queue<GAction> queue = new Queue<GAction>();
        foreach(GAction action in actionsTrace)
        {
            queue.Enqueue(action);
        }

        return queue;
    }

    private bool BuildGraph(Node parent, List<Node> leaves, List<GAction> usableActions, Dictionary<string, int> goal)
    {
        bool foundPath = false;
        foreach(GAction action in usableActions)
        {
            if(action.IsAchievableGiven(parent.State))
            {
                Dictionary<string, int> currentState = new Dictionary<string, int>(parent.State);
                foreach(KeyValuePair<string, int> effect in action.Effects)
                {
                    if(!currentState.ContainsKey(effect.Key)) currentState.Add(effect.Key, effect.Value);
                }

                Node node = new Node(parent, parent.Cost + action.Cost, currentState, action);

                if (GoalAchieved(goal, currentState))
                {
                    leaves.Add(node);
                    foundPath = true;
                }

                else
                {
                    List<GAction> subset = ActionSubset(usableActions, action);
                    bool found = BuildGraph(node, leaves, subset, goal);
                    if(found) foundPath = true;
                }
            }
        }
        return foundPath;
    }

    private bool GoalAchieved(Dictionary<string, int> goals, Dictionary<string, int> state)
    {
        foreach(KeyValuePair<string, int> goal in goals)
        {
            if(!state.ContainsKey(goal.Key)) return false;
        }

        return true;
    }

    private List<GAction> ActionSubset(List<GAction> actions, GAction actionToRemove)
    {
        List<GAction> subset = new List<GAction>();
        foreach(GAction action in actions)
        {
            if(!action.Equals(actionToRemove)) subset.Add(action);
        }

        return subset;
    }
}
