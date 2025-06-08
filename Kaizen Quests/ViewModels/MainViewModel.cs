using Kaizen_Quests.Models;
using Kaizen_Quests.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;

namespace Kaizen_Quests.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Binding Fields

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading == value)
                    return;
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        public ObservableCollection<QuestViewModel> Quests { get; } = [];

        public ICommand AddQuestCommand => new Command<string>(async (indexString) => await AddQuest(indexString));
        public ICommand AddGoalCommand => new Command<QuestViewModel>(async (qvm) => await AddGoal(qvm));
        public ICommand StartQuestDragCommand => new Command<QuestViewModel>(StartQuestDrag);
        public ICommand StartGoalDragCommand => new Command<GoalViewModel>(StartGoalDrag);
        public ICommand QuestDropCommand => new Command<QuestViewModel>(async (qvm) => await QuestDrop(qvm));
        public ICommand GoalDropCommand => new Command<GoalViewModel>(async (gvm) => await GoalDrop(gvm));
        public ICommand QuestTappedCommand => new Command<QuestViewModel>(async (qvm) => await QuestTapped(qvm));
        public ICommand GoalTappedCommand => new Command<GoalViewModel>(async (gvm) => await GoalTapped(gvm));

        #endregion

        #region Other Fields

        private QuestViewModel? _dragSourceQuest;
        private GoalViewModel? _dragSourceGoal;
        private QuestViewModel? _dragSourceGoalParent;
        private readonly DatabaseService _dbs;
        public event Action<QuestViewModel>? QuestAdded;
        public event Action<QuestViewModel>? GoalAdded;
        public event Action<QuestViewModel>? QuestsOrderChanged;
        public IDialogService? DialogService;

        #endregion

        public MainViewModel(DatabaseService dbs)
        {
            _dbs = dbs;
            _ = LoadDataAsync();
            // Update the order of quests after any change
            Quests.CollectionChanged += (s, e) => { for (int i = 0; i < Quests.Count; i++) { Quests[i].Order = i + 1; } };
        }

        private async Task QuestTapped(QuestViewModel qvm)
        {
            if (DialogService == null)
                return;
            string action = await DialogService.ShowActionSheet("Was möchtest du tun?", "❌ Abbrechen", "", "✏️ Bearbeiten", "🗑️ Löschen");
            switch (action)
            {
                case "✏️ Bearbeiten":
                    string newTitle = await DialogService.ShowPrompt("✏️ Bearbeiten", "Neuer Titel:", qvm.Title ?? "");
                    if (!String.IsNullOrWhiteSpace(newTitle))
                    {
                        qvm.Title = newTitle;
                        await SaveDataAsync();
                    }
                    break;
                case "🗑️ Löschen":
                    bool confirm = await DialogService.ShowConfirmation("🗑️ Löschen", "Willst du die Quest wirklich löschen?");
                    if (confirm)
                    {
                        Quests.Remove(qvm);
                        await SaveDataAsync();
                    }
                    break;
                default:
                    break;
            }
        }


        private async Task GoalTapped(GoalViewModel gvm)
        {
            if (DialogService == null)
                return;
            string action = await DialogService.ShowActionSheet("Was möchtest du tun?", "❌ Abbrechen", "", "✏️ Bearbeiten", "🗑️ Löschen");
            switch (action)
            {
                case "✏️ Bearbeiten":
                    string newText = await DialogService.ShowPrompt("✏️ Bearbeiten", "Neuer Text:", gvm.Text ?? "");
                    if (!String.IsNullOrWhiteSpace(newText))
                    {
                        gvm.Text = newText;
                        await SaveDataAsync();
                    }
                    break;
                case "🗑️ Löschen":
                    bool confirm = await DialogService.ShowConfirmation("🗑️ Löschen", "Willst du den Text wirklich löschen?");
                    if (confirm)
                    {
                        QuestViewModel? parentQuest = FindParentQuest(gvm);
                        if (parentQuest == null)
                            return;
                        parentQuest.Goals.Remove(gvm);
                        await SaveDataAsync();
                    }
                    break;
                default:
                    break;
            }
        }


        private async Task LoadDataAsync()
        {
            IsLoading = true;
            List<Quest> loadedQuests = await _dbs.GetQuestsWithGoalsAsync();
            Quests.Clear();
            foreach (Quest quest in loadedQuests)
            {
                Quests.Add(new QuestViewModel(quest));
            }
            IsLoading = false;
        }

        public async Task SaveDataAsync()
        {
            try
            {
                List<Quest> questsToSave = Quests.Select(qvm => qvm.QuestModel).ToList();
                await _dbs.SaveChangesIfNeededAsync(questsToSave);
            }
            catch (Exception ex)
            {
                // User benachrichtigen
                await Application.Current!.MainPage!.DisplayAlert("Fehler", "Speichern fehlgeschlagen: " + ex.Message, "OK");
                // Fehler weiterwerfen
                throw;
            }
        }

        private void StartQuestDrag(QuestViewModel dragSourceQuest)
        {
            if (dragSourceQuest == null)
                return;
            _dragSourceQuest = dragSourceQuest;
            _dragSourceGoal = null; // Reset goal drag source when starting quest drag
            _dragSourceGoalParent = null; // Reset goal parent when starting quest drag
        }
        private async Task QuestDrop(QuestViewModel dropDestinationQuest)
        {
            if (dropDestinationQuest == null || _dragSourceQuest == null || _dragSourceQuest == dropDestinationQuest)
                return;
            Quests.Move(Quests.IndexOf(_dragSourceQuest), Quests.IndexOf(dropDestinationQuest));
            QuestsOrderChanged?.Invoke(_dragSourceQuest);
            await SaveDataAsync();
        }

        private void StartGoalDrag(GoalViewModel dragSourceGoal)
        {
            if (dragSourceGoal == null)
                return;
            _dragSourceGoal = dragSourceGoal;
            _dragSourceGoalParent = FindParentQuest(dragSourceGoal);
            _dragSourceQuest = null; // Reset quest drag source when starting goal drag
        }
        private async Task GoalDrop(GoalViewModel dropDestionationGoal)
        {
            if (_dragSourceQuest != null && dropDestionationGoal != null)
            {
                await QuestDrop(FindParentQuest(dropDestionationGoal)!);
                return;
            }
            if (dropDestionationGoal == null ||
                _dragSourceGoal == null ||
                _dragSourceGoal == dropDestionationGoal)
                return;
            QuestViewModel? dropDestionationGoalParent = FindParentQuest(dropDestionationGoal);
            if (_dragSourceGoalParent == null ||
                dropDestionationGoalParent == null ||
                _dragSourceGoalParent != dropDestionationGoalParent)
                return;
            int oldIndex = _dragSourceGoalParent.Goals.IndexOf(_dragSourceGoal);
            int newIndex = _dragSourceGoalParent.Goals.IndexOf(dropDestionationGoal);
            _dragSourceGoalParent.Goals.Move(oldIndex, newIndex);
            await SaveDataAsync();
        }
        private QuestViewModel? FindParentQuest(GoalViewModel goal)
        {
            foreach (QuestViewModel quest in Quests)
            {
                if (quest.Goals.Contains(goal))
                {
                    return quest;
                }
            }
            return null;
        }

        private async Task AddQuest(object colorIndexString)
        {
            if (colorIndexString is not string || !int.TryParse(colorIndexString as string, out int index))
                return;
            string colorKey = $"Rainbow{index}";
            string color = ((Color)Application.Current!.Resources[colorKey]).ToHex();
            Quest newQuest = new()
            {
                Title = "Titel",
                Color = color,
                Goals = [new Goal { IsAddGoal = true }]
            };
            QuestViewModel newQuestVm = new(newQuest);
            Quests.Add(newQuestVm);
            QuestAdded?.Invoke(newQuestVm);
            await SaveDataAsync();
        }
        private async Task AddGoal(QuestViewModel questViewModel)
        {
            if (questViewModel == null)
                return;
            int index = questViewModel.Goals.ToList().FindIndex(g => g.IsAddGoal);
            Goal goal = new Goal() { Description = "Text" };
            questViewModel.Goals.Insert(index, new GoalViewModel(goal));
            GoalAdded?.Invoke(questViewModel);
            await SaveDataAsync();
        }
    }
}
