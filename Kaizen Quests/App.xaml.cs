using Kaizen_Quests.Services;

namespace Kaizen_Quests
{
    public partial class App : Application
    {
        private readonly DatabaseService _dbs;

        public App(DatabaseService dbs)
        {
            InitializeComponent();
            _dbs = dbs;
            InitializeDatabase();
            MainPage = new AppShell();
        }

        private async void InitializeDatabase()
        {
            await _dbs.InitializeAsync();
        }
    }
}
