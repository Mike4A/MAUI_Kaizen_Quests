using Kaizen_Quests.Helpers;
using Kaizen_Quests.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Kaizen_Quests.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<QuestViewModel> Quests { get; } = [];

        public ICommand AddQuestCommand { get; }

        public MainViewModel()
        {
            AddQuestCommand = new Command<object>(AddQuest);
            //Test Data
            Quests.Add(new QuestViewModel(new Quest
            {
                Title = "Learn C#",
                Color = GetRandomColorHexStringFromResourcePalette(),
                Goals = new List<Goal>
                {
                    new Goal { Description = "Complete C# Basics Course", IsCompleted = false },
                    new Goal { Description = "Build a simple console app", IsCompleted = false }
                }
            }));
            Quests.Add(new QuestViewModel(new Quest
            {
                Title = "Improve Fitness",
                Color = GetRandomColorHexStringFromResourcePalette(),
                Goals = new List<Goal>
                {
                    new Goal { Description = "Run 5km", IsCompleted = false },
                    new Goal { Description = "Do 50 push-ups", IsCompleted = false }
                }
            }));
            Quests.Add(new QuestViewModel(new Quest
            {
                Title = "Read More Books",
                Color = GetRandomColorHexStringFromResourcePalette(),
                Goals = new List<Goal>
                {
                    new Goal { Description = "Read 12 books this year", IsCompleted = false },
                    new Goal { Description = "Join a book club", IsCompleted = false }
                }
            }));
            Quests.Add(new QuestViewModel(new Quest
            {
                Title = "Learn a New Language",
                Color = GetRandomColorHexStringFromResourcePalette(),
                Goals = new List<Goal>
                {
                    new Goal { Description = "Complete 30 Duolingo lessons", IsCompleted = false },
                    new Goal { Description = "Practice speaking with a native speaker", IsCompleted = false }
                }
            }));
            Quests.Add(new QuestViewModel(new Quest
            {
                Title = "Travel More",
                Color = GetRandomColorHexStringFromResourcePalette(),
                Goals = new List<Goal>
                {
                    new Goal { Description = "Visit 3 new countries this year", IsCompleted = false },
                    new Goal { Description = "Plan a road trip", IsCompleted = false }
                }
            }));
        }

        private void AddQuest(object param)
        {
            if (param is not string indexString || !int.TryParse(indexString, out int index))
                return;

            string colorKey = $"Rainbow{index}";
            string color = ((Color)Application.Current!.Resources[colorKey]).ToHex();

            Quest newQuest = new()
            {
                Color = color,
                Goals = [new Goal { IsAddGoal = true }]
            };

            Quests.Add(new QuestViewModel(newQuest));
        }

        private string GetRandomColorHexStringFromResourcePalette()
        {
            string name = $"Rainbow{RngHelper.Random.Next(8)}";
            return ((Color)Application.Current!.Resources[name]).ToHex();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
