using Kaizen_Quests.Pages;

namespace Kaizen_Quests
{
    public partial class App : Application
    {
        public App(MainPage mainPage)
        {
            InitializeComponent();
            MainPage = mainPage;
        }
    }
}
