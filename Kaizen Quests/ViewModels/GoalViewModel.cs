using Kaizen_Quests.Models;
using System.ComponentModel;

namespace Kaizen_Quests.ViewModels
{    
    public class GoalViewModel : INotifyPropertyChanged
    {
        private Goal _goal;

        public GoalViewModel(Goal goal)
        {
            _goal = goal;
        }
               
        public string? Description
        {
            get => _goal.Description;
            set
            {
                if (_goal.Description != value)
                {
                    _goal.Description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        public bool IsCompleted
        {
            get => _goal.IsCompleted;
            set
            {
                if (_goal.IsCompleted != value)
                {
                    _goal.IsCompleted = value;
                    OnPropertyChanged(nameof(IsCompleted));
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
