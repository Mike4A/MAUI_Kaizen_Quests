using Kaizen_Quests.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Kaizen_Quests.ViewModels
{
    public class QuestViewModel : INotifyPropertyChanged
    {
        private Quest _quest;

        public ObservableCollection<GoalViewModel> Goals { get; }

        public QuestViewModel(Quest quest)
        {
            _quest = quest;
            Goals = new ObservableCollection<GoalViewModel>();
            foreach (var goal in _quest.Goals)
            {
                Goals.Add(new GoalViewModel(goal));
            }
        }
                
        public string? Title
        {
            get => _quest.Title;
            set
            {
                if (_quest.Title != value)
                {
                    _quest.Title = value;
                    OnPropertyChanged(nameof(Title));
                }
            }
        }
        
        public string? Color
        {
            get => _quest.Color;
            set
            {
                if (_quest.Color != value)
                {
                    _quest.Color = value;
                    OnPropertyChanged(nameof(Color));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }    
    }
}
