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
        public ICommand StartQuestDragCommand { get; }
        public ICommand QuestDropCommand { get; }

        private QuestViewModel? _dragSourceQuest;

        public MainViewModel()
        {
            AddQuestCommand = new Command<object>(AddQuest);
            AddGoalCommand = new Command<QuestViewModel>(AddGoal);
            StartQuestDragCommand = new Command<QuestViewModel>(StartQuestDrag);
            QuestDropCommand = new Command<QuestViewModel>(QuestDrop);
        }

        private void QuestDrop(QuestViewModel dropDestionationQuest)
        {
            if (dropDestionationQuest == null || _dragSourceQuest == null || _dragSourceQuest == dropDestionationQuest)
                return;
            Quests.Move(Quests.IndexOf(_dragSourceQuest), Quests.IndexOf(dropDestionationQuest));
        }

        private void StartQuestDrag(QuestViewModel dragSourceQuest)
        {
            if (dragSourceQuest == null)
                return;
            _dragSourceQuest = dragSourceQuest;
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
