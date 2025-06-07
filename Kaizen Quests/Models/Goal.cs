namespace Kaizen_Quests.Models
{
    public class Goal
    {
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsAddGoal { get; set; } // Flag for AddGoalGoal
    }
}
