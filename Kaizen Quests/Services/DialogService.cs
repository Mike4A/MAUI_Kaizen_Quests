using Kaizen_Quests.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaizen_Quests.Services
{
    public interface IDialogService
    {
        Task<string> ShowActionSheet(string title, string cancel, string destruction, params string[] buttons);
        Task<string> ShowPrompt(string title, string message, string initialValue);
        Task<bool> ShowConfirmation(string title, string message);
    }

    public class DialogService : IDialogService
    {
        readonly MainPage _mainPage;

        public DialogService(MainPage mainPage)
        {
            _mainPage = mainPage;
        }

        public Task<string> ShowActionSheet(string title, string cancel, string destruction, params string[] buttons)
        {
            return _mainPage.DisplayActionSheet(title, cancel, destruction, buttons);
        }

        public async Task<string> ShowPrompt(string title, string message, string initialValue)
        {
            return await _mainPage.DisplayPromptAsync(title, message, initialValue: initialValue);
        }

        public async Task<bool> ShowConfirmation(string title, string message)
        {
            return await _mainPage.DisplayAlert(title, message, "Ja", "Nein");
        }
    }
}
