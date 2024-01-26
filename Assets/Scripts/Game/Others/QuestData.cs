using UnityEngine;

public enum QuestType
{
    Collect,
    Kill,
    Destroy,
    Reach
}

[CreateAssetMenu(menuName = "ScriptableObjects/Quest")]
public class QuestData : ScriptableObject
{
    [field: SerializeField] public QuestType Type { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int LevelRequirement { get; private set; }
    [field: SerializeField] public int ExperienceReward { get; private set; }
    [field: SerializeField] public string Target { get; private set; }
    [field: SerializeField] public int TargetCount { get; private set; }
    [field: SerializeField] public Vector3 StartPosition { get; private set; }
    [field: SerializeField] public Vector3 FinalPosition { get; private set; }
    [field: SerializeField] public string StartDialogue { get; private set; }
    [field: SerializeField] public string EndDialogue { get; private set; }
    [field: SerializeField] public bool IsRepeatable { get; private set; }
}
