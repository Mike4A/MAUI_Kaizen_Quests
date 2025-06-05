namespace Kaizen_Quests.Models
{
    public class Quest
    {
        public string? Title { get; set; }
        public List<Goal> Goals { get; set; } = new();
        public string? Color { get; set; }
    }
}
