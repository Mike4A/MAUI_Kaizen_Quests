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
            get => _goalModel.Id;
            set
            {
                if (_goalModel.Id == value)
                    return;
                _goalModel.Id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public int QuestId
        {
            get => _goalModel.QuestId;
            set
            {
                if (_goalModel.QuestId == value)
                    return;
                _goalModel.QuestId = value;
                OnPropertyChanged(nameof(QuestId));
            }
        }

        public int Order
        {
            get => _goalModel.Order;
            set
            {
                if (_goalModel.Order == value)
                    return;
                _goalModel.Order = value;
                OnPropertyChanged(nameof(Order));
            }
        }

        public string? Text
        {
            get => _goalModel.Description;
            set
            {
                if (_goalModel.Description == value)
                    return;
                _goalModel.Description = value;
                OnPropertyChanged(nameof(Text));
            }
        }

        public bool IsCompleted
        {
            get => _goalModel.IsCompleted;
            set
            {
                if (_goalModel.IsCompleted == value)
                    return;
                _goalModel.IsCompleted = value;
                OnPropertyChanged(nameof(IsCompleted));
            }
        }

        public bool IsRegularGoal { get => !_goalModel.IsAddGoal; }

        public bool IsAddGoal
        {
            get => _goalModel.IsAddGoal;
            set
            {
                if (_goalModel.IsAddGoal == value)
                    return;
                _goalModel.IsAddGoal = value;
                OnPropertyChanged(nameof(IsAddGoal));
            }
        }

        #endregion

        #region Other Fields

        private Goal _goalModel;
        public Goal GoalModel => _goalModel; // Read-only property to expose the model directly for Sync-reasons

        #endregion

        public GoalViewModel(Goal goal)
        {
            _goalModel = goal;
        }

        public Goal ToModel()
        {
            return new Goal
            {
                Id = Id,
                QuestId = QuestId,
                Order = Order,
                Description = Text,
                IsCompleted = IsCompleted,
                IsAddGoal = IsAddGoal
            };
        }
    }
}
