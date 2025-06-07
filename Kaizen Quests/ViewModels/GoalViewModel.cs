using Kaizen_Quests.Models;
using System.ComponentModel;

namespace Kaizen_Quests.ViewModels
{
    public class GoalViewModel : INotifyPropertyChanged
    {
        #region Binding Fields

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int Id
        {
            get => _goal.Id;
            set
            {
                if (_goal.Id == value)
                    return;
                _goal.Id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public int QuestId
        {
            get => _goal.QuestId;
            set
            {
                if (_goal.QuestId == value)
                    return;
                _goal.QuestId = value;
                OnPropertyChanged(nameof(QuestId));
            }
        }

        public int Order
        {
            get => _goal.Order;
            set
            {
                if (_goal.Order == value)
                    return;
                _goal.Order = value;
                OnPropertyChanged(nameof(Order));
            }
        }

        public string? Description
        {
            get => _goal.Description;
            set
            {
                if (_goal.Description == value)
                    return;
                _goal.Description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public bool IsCompleted
        {
            get => _goal.IsCompleted;
            set
            {
                if (_goal.IsCompleted == value)
                    return;
                _goal.IsCompleted = value;
                OnPropertyChanged(nameof(IsCompleted));
            }
        }

        public bool IsRegularGoal { get => !_goal.IsAddGoal; }

        public bool IsAddGoal
        {
            get => _goal.IsAddGoal;
            set
            {
                if (_goal.IsAddGoal == value)
                    return;
                _goal.IsAddGoal = value;
                OnPropertyChanged(nameof(IsAddGoal));
            }
        }

        #endregion

        #region Private Fields

        private Goal _goal;

        #endregion

        public GoalViewModel(Goal goal)
        {
            _goal = goal;
        }

        public Goal ToModel()
        {
            return new Goal
            {
                Id = Id,
                QuestId = QuestId,
                Order = Order,
                Description = Description,
                IsCompleted = IsCompleted,
                IsAddGoal = IsAddGoal
            };
        }
    }
}
