using System.Threading.Tasks;
using Xamarin.Forms;

namespace Notepad.Services
{
    public class DisplayService
    {
        public async Task<bool> AlertAsync(string title, string message, string accept, string cancel)
        {
            Page currentPage = (Application.Current.MainPage as NavigationPage).CurrentPage;
            return await currentPage.DisplayAlert(title, message, accept, cancel);
        }

        public async Task<string> PromptAsync(string title, string message, string accept = "OK", string cancel = "Cancel", string placeholder = null, int maxLength = -1, Keyboard keyboard = null, string initialValue = "")
        {
            Page currentPage = (Application.Current.MainPage as NavigationPage).CurrentPage;
            return await currentPage.DisplayPromptAsync(title, message, accept, cancel, placeholder, maxLength, keyboard, initialValue);
        }
    }
}