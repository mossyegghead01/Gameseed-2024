using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSystem : MonoBehaviour
{
    public enum QuestTypes { Kill, Harvest, Loot }
    public class Quest
    {
        public float ScoreReward { get; set; }
        public bool Completed { get; set; }
        public QuestTypes Type { get; set; }

        public virtual void OnCompleted() { }
        public virtual bool CheckCompleted() { return false; }

        public Quest(float scoreReward, bool completed, QuestTypes type)
        {
            ScoreReward = scoreReward;
            Completed = completed;
            Type = type;
        }
    }

    public class KillQuest : Quest
    {
        public KillQuest(float scoreReward) : base(scoreReward, false, QuestTypes.Kill)
        {
        }
    }

    public class HarvestQuest : Quest
    {
        public HarvestQuest(float scoreReward) : base(scoreReward, false, QuestTypes.Harvest)
        {
        }
    }

    public class LootQuest : Quest
    {
        public LootQuest(float scoreReward) : base(scoreReward, false, QuestTypes.Loot)
        {
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
