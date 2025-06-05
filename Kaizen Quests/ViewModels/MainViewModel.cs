using Kaizen_Quests.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Kaizen_Quests.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<QuestViewModel> Quests { get; } = [];

        public ICommand AddQuestCommand { get; }
        public ICommand AddGoalCommand { get; }

        public MainViewModel()
        {
            AddQuestCommand = new Command<object>(AddQuest);
            AddGoalCommand = new Command<QuestViewModel>(AddGoal);
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
        private void AddGoal(QuestViewModel questViewModel)
        {
            if (questViewModel == null)
                return;

            int addGoalIndex = questViewModel.Goals.ToList().FindIndex(g => g.IsAddGoal);
            if (addGoalIndex == -1)
                addGoalIndex = questViewModel.Goals.Count;

            questViewModel.Goals.Insert(addGoalIndex, new GoalViewModel(new Goal()));
        }        
    }
}
