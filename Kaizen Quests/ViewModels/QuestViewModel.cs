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
            get => _quest.Id;
            set
            {
                if (_quest.Id == value)
                    return;
                _quest.Id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public int Order
        {
            get => _quest.Order;
            set
            {
                if (_quest.Order == value)
                    return;
                _quest.Order = value;
                OnPropertyChanged(nameof(Order));
            }
        }

        public string? Title
        {
            get => _quest.Title;
            set
            {
                if (_quest.Title == value)
                    return;
                _quest.Title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public string? Color
        {
            get => _quest.Color;
            set
            {
                if (_quest.Color == value)
                    return;
                _quest.Color = value;
                OnPropertyChanged(nameof(Color));
            }
        }

        public ObservableCollection<GoalViewModel> Goals { get; }

        #endregion

        #region Private Fields

        private Quest _quest;

        #endregion

        public QuestViewModel(Quest quest)
        {
            _quest = quest;
            Goals = [.. _quest.Goals.Select(g => new GoalViewModel(g))]; 
            Goals.CollectionChanged += Goals_CollectionChanged;
        }

        private void Goals_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    // Neue Goals hinzufügen ins Model
                    foreach (GoalViewModel newGoalVm in e.NewItems!)
                    {
                        _quest.Goals.Add(newGoalVm.ToModel());
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    // Entfernte Goals aus Model löschen
                    foreach (GoalViewModel oldGoalVm in e.OldItems!)
                    {
                        var toRemove = _quest.Goals.FirstOrDefault(g => g.Id == oldGoalVm.Id);
                        if (toRemove != null)
                            _quest.Goals.Remove(toRemove);
                    }
                    break;

                case NotifyCollectionChangedAction.Move:
                    // Reihenfolge anpassen
                    if (e.OldStartingIndex >= 0 && e.NewStartingIndex >= 0)
                    {
                        var movedGoal = _quest.Goals[e.OldStartingIndex];
                        _quest.Goals.RemoveAt(e.OldStartingIndex);
                        _quest.Goals.Insert(e.NewStartingIndex, movedGoal);
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    // Ersatz-Logik (optional)
                    break;

                case NotifyCollectionChangedAction.Reset:
                    // z.B. bei Clear()
                    _quest.Goals.Clear();
                    break;
            }
        }
    }
}
