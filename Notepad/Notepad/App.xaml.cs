using Notepad.Services;
using Notepad.Views;
using Xamarin.Forms;

namespace Notepad
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Ajout des vues
            NavigationService.AddView(nameof(NoteEditView), typeof(NoteEditView));

            MainPage = new NavigationPage(new NoteListView());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}