using System;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace Kaizen_Quests
{
    internal class Program : MauiApplication
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        static void Main(string[] args)
        {
            Program app = new Program();
            app.Run(args);
        }
    }
}
