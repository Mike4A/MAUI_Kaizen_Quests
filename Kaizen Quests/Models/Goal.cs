using SQLite;

namespace Kaizen_Quests.Models
{
    public class Goal : IEquatable<Goal>
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int QuestId { get; set; }  // Fremdschlüssel
        public string? Text { get; set; }
        public int Order { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsAddGoal { get; set; } // Flag for AddGoalGoal

        public bool Equals(Goal? other)
        {
            if (other is null)
                return false;
            return Id == other.Id &&
                   QuestId == other.QuestId &&
                   Text == other.Text &&
                   Order == other.Order &&
                   IsCompleted == other.IsCompleted &&
                   IsAddGoal == other.IsAddGoal;
        }
    }
}
