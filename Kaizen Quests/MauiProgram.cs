using Kaizen_Quests.Services;
using Kaizen_Quests.ViewModels;
using Microsoft.Extensions.Logging;

namespace Kaizen_Quests
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            MauiAppBuilder builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
                        
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "kaizen_quests.db3");

            // Services registrieren
            builder.Services.AddSingleton<DatabaseService>(sp => new DatabaseService(dbPath));
            builder.Services.AddTransient<MainViewModel>();

            return builder.Build();
        }
    }
}
