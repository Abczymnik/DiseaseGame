public class ZombieGoals: GAgent
{
    new void Start()
    {
        base.Start();

        SubGoal s1 = new SubGoal("caught", 1, false);
        goals.Add(s1, 3);

        SubGoal s2 = new SubGoal("spotted", 1, false);
        goals.Add(s2, 1);

        SubGoal s3 = new SubGoal("on spawn", 1, false);
        goals.Add(s3, 5);

        SubGoal s4 = new SubGoal("kill player", 1, false);
        goals.Add(s4, 4);
    }

    private void OnDisable()
    {
        PlanStop();
    }
}
