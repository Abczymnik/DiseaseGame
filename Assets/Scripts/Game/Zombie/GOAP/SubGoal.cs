using System.Collections.Generic;

public class SubGoal
{
    public Dictionary<string, int> SGoal { get; private set; }
    public bool RemoveAfter { get; private set; }

    public SubGoal(string s, int i, bool r)
    {
        SGoal = new Dictionary<string, int>();
        SGoal.Add(s, i);
        RemoveAfter = r;
    }
}