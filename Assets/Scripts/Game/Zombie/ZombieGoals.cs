public class ZombieGoals: GAgent
{
    private void Awake()
    {
        Init();
        SubGoal s1 = new SubGoal("caught", 1, false);
        Goals.Add(s1, 3);

        SubGoal s2 = new SubGoal("spotted", 1, false);
        Goals.Add(s2, 1);

        SubGoal s3 = new SubGoal("on spawn", 1, false);
        Goals.Add(s3, 5);

        SubGoal s4 = new SubGoal("kill player", 1, false);
        Goals.Add(s4, 4);
    }

    private void OnDisable()
    {
        PlanStop();
    }
}
