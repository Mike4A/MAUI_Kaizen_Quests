using Kaizen_Quests.Services;
using Kaizen_Quests.ViewModels;

namespace Kaizen_Quests.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel mainViewModel)
        {
            InitializeComponent();
            BindingContext = mainViewModel;
            mainViewModel.DialogService = new DialogService(this);
            mainViewModel.QuestAdded += async (newQuestVm) => { await ScrollToQuestAsync(newQuestVm); };
            mainViewModel.GoalAdded += async (parentQuestVm) => { await ScrollToQuestAsync(parentQuestVm); };
            mainViewModel.QuestsOrderChanged += async (changedQuestVm) => { await ScrollToQuestAsync(changedQuestVm); };
        }

        private async Task ScrollToQuestAsync(QuestViewModel questViewModel)
        {
            if (questViewModel == null)
                return;
            //Looks weird but is the only solution i've found to prevent a bug where it scrolls to last quest if index == 0
            await Task.Delay(250);
            Dispatcher.Dispatch(async () =>
            {
                await Task.Delay(250); 
                QuestsCollectionView.ScrollTo(questViewModel, position: ScrollToPosition.MakeVisible, animate: true);
            });
        }
    }
}