using SQLite;

namespace Kaizen_Quests.Models
{
    public class Quest : IEquatable<Quest>
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string? Title { get; set; }
        public int Order { get; set; }
        public string? Color { get; set; }
        public bool IsExpanded { get; set; }
        [Ignore]
        public List<Goal> Goals { get; set; } = new();

        public bool Equals(Quest? other)
        {
            if (other is null)
                return false;            
            return Id == other.Id &&
                   Title == other.Title &&
                   Order == other.Order &&
                   Color == other.Color &&
                   IsExpanded == other.IsExpanded &&
                   Goals.SequenceEqual(other.Goals);
        }        
    }
}