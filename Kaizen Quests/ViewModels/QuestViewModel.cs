using Kaizen_Quests.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Kaizen_Quests.ViewModels
{
    public class QuestViewModel : INotifyPropertyChanged
    {
        #region Binding Fields

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public int Id
        {
            get => _questModel.Id;
            set
            {
                if (_questModel.Id == value)
                    return;
                _questModel.Id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public int Order
        {
            get => _questModel.Order;
            set
            {
                if (_questModel.Order == value)
                    return;
                _questModel.Order = value;
                OnPropertyChanged(nameof(Order));
            }
        }

        public string? Title
        {
            get => _questModel.Title;
            set
            {
                if (_questModel.Title == value)
                    return;
                _questModel.Title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public string? Color
        {
            get => _questModel.Color;
            set
            {
                if (_questModel.Color == value)
                    return;
                _questModel.Color = value;
                OnPropertyChanged(nameof(Color));
            }
        }

        public ObservableCollection<GoalViewModel> Goals { get; }

        #endregion

        #region Other Fields

        private Quest _questModel;
        public Quest QuestModel { get => _questModel; }

        #endregion

        public QuestViewModel(Quest quest)
        {
            _questModel = quest;
            Goals = [.. _questModel.Goals.Select(g => new GoalViewModel(g))];
            Goals.CollectionChanged += Goals_CollectionChanged;
        }

        private void Goals_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (GoalViewModel newGoalVm in e.NewItems!)
                    {
                        _questModel.Goals.Add(newGoalVm.GoalModel);
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (GoalViewModel oldGoalVm in e.OldItems!)
                    {
                        _questModel.Goals.Remove(oldGoalVm.GoalModel);
                    }
                    break;

                case NotifyCollectionChangedAction.Move:
                    if (e.OldStartingIndex != e.NewStartingIndex)
                    {
                        Goal movedGoal = _questModel.Goals[e.OldStartingIndex];
                        _questModel.Goals.RemoveAt(e.OldStartingIndex);
                        _questModel.Goals.Insert(e.NewStartingIndex, movedGoal);
                    }
                    break;

                case NotifyCollectionChangedAction.Reset:
                    _questModel.Goals.Clear();
                    foreach (GoalViewModel goalVm in Goals)
                    {
                        _questModel.Goals.Add(goalVm.GoalModel);
                    }
                    break;
            }

            // Update the order of goals after any change
            for (int i = 0; i < Goals.Count; i++)
            {
                Goals[i].Order = i + 1;
            }
        }
    }
}
