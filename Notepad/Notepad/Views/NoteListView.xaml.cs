using Notepad.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Notepad.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoteListView : ContentPage
    {
        public NoteListView()
        {
            InitializeComponent();
        }
    }
}