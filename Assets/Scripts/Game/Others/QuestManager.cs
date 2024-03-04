using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuestManager : MonoBehaviour
{
    [field: SerializeField] public List<QuestData> UnAvailableQuests { get; private set; }
    [field: SerializeField] public List<QuestData> AvailableQuests { get; private set; }
    [field: SerializeField] public List<QuestData> ActiveQuests { get; private set; }
    [field: SerializeField] public List<QuestData> CompletedQuests { get; private set; }

    private UnityAction<object> onActiveQuestsComplete;
    private UnityAction<object> onLevelUp;
    private UnityAction<object> onNewQuest;

    private void OnEnable()
    {
        onActiveQuestsComplete += OnQuestComplete;
        onLevelUp += OnAvailabilityRefresh;
        EventManager.StartListening(TypedEventName.LevelUp, onLevelUp);
        onNewQuest += OnNewQuest;
        EventManager.StartListening(TypedEventName.NewQuest, onNewQuest);
    }

    private void Start()
    {
        for (int i = AvailableQuests.Count - 1; i >= 0; i--)
        {
            if (AvailableQuests[i].LevelRequirement > 5) continue;

            QuestData questToMove = AvailableQuests[i];
            AvailableQuests.RemoveAt(i);
            ActiveQuests.Add(questToMove);
        }
    }

    private void OnNewQuest(object questObj)
    {
        QuestData quest = (QuestData)questObj;

        if (ActiveQuests.Contains(quest)) return;

        if(AvailableQuests.Contains(quest))
        {
            ActiveQuests.Add(quest);
            AvailableQuests.Remove(quest);
            //EventManager.StartListening(quest.Name, onActiveQuestsComplete);
            Debug.Log(quest.StartDialogue);
        }
    }

    private void OnAvailabilityRefresh(object playerLevelObj)
    {
        int playerLevel = (int)playerLevelObj;
        for (int i = UnAvailableQuests.Count - 1; i >= 0; i--)
        {
            if (UnAvailableQuests[i].LevelRequirement > playerLevel) continue;

            QuestData questToMove = UnAvailableQuests[i];
            UnAvailableQuests.RemoveAt(i);
            AvailableQuests.Add(questToMove);
        }
    }

    private void OnQuestComplete(object questObj)
    {
        QuestData quest = (QuestData)questObj;
        if(ActiveQuests.Contains(quest))
        {
            ActiveQuests.Remove(quest);
            EventManager.TriggerEvent(TypedEventName.AddExperience, quest.ExperienceReward);
            //EventManager.StopListening(quest.name, onActiveQuestsComplete);
            Debug.Log(quest.EndDialogue);

            if (quest.IsRepeatable) AvailableQuests.Add(quest);
            CompletedQuests.Add(quest);
        }
    }

    private void OnDisable()
    {
        foreach(QuestData quest in ActiveQuests)
        {
            //EventManager.StopListening(quest.Name, onActiveQuestsComplete);
        }

        EventManager.StopListening(TypedEventName.LevelUp, onLevelUp);
        EventManager.StopListening(TypedEventName.NewQuest, onNewQuest);
    }
}
