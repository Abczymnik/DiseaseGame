using System.Collections.Generic;

[System.Serializable]
public class WorldState
{
    public string Key { get; private set; }
    public int Value { get; private set; }

    public WorldState(string key, int value)
    {
        Key = key;
        Value = value;
    }
}

public class WorldStates
{
    public Dictionary<string, int> States { get; private set; }

    public WorldStates()
    {
        States = new Dictionary<string, int>();
    }

    public bool HasState(string key)
    {
        return States.ContainsKey(key);
    }

    public void AddState(string key, int value)
    {
        if (!States.ContainsKey(key)) States.Add(key, value);
    }

    public void ModifyState(string key, int value)
    {
        if (States.ContainsKey(key))
        {
            States[key] += value;
            if (States[key] <= 0) RemoveState(key);
        }
        else States.Add(key, value);
    }

    public void RemoveState(string key)
    {
        if (States.ContainsKey(key)) States.Remove(key);
    }

    public void SetState(string key, int value)
    {
        if(States.ContainsKey(key)) States[key] = value;
        else States.Add(key, value);
    }
}
