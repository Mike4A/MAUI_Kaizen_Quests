using Kaizen_Quests.ViewModels;

namespace Kaizen_Quests.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel mainViewModel)
        {
            InitializeComponent();
            BindingContext = mainViewModel;
        }   

    }
}
