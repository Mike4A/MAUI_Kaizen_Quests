using Kaizen_Quests.ViewModels;

namespace Kaizen_Quests.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel mainViewModel)
        {
            InitializeComponent();
            BindingContext = mainViewModel;
            mainViewModel.QuestAdded += async (newQuestVm) => { await ScrollToQuestAsync(newQuestVm); };
            mainViewModel.GoalAdded += async (parentQuestVm) => { await ScrollToQuestAsync(parentQuestVm); };
            mainViewModel.QuestsOrderChanged += async (changedQuestVm) => { await ScrollToQuestAsync(changedQuestVm); };
        }

        private async Task ScrollToQuestAsync(QuestViewModel questViewModel)
        {
            if (questViewModel == null)
                return;            
            await Task.Delay(100); //give the UI time to respond
            QuestsCollectionView.ScrollTo(questViewModel, position: ScrollToPosition.MakeVisible, animate: true);
        }
    }
}