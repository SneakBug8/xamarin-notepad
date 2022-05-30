using System.ComponentModel;
using System.Runtime.CompilerServices;
using Notepad.Services;

namespace Notepad.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        protected const string LoadNote = "LoadNote";
        protected const string NoteUpdated = "NoteUpdated";
        protected const string NoteDeleted = "NoteDeleted";

        protected static readonly INoteService noteService = new SqlNoteService();
        protected static readonly DisplayService displayService = new DisplayService();
        protected static readonly MessagingService messagingService = new MessagingService();
        protected static readonly NavigationService navigationService = new NavigationService();

        public event PropertyChangedEventHandler PropertyChanged;

        protected void SetProperty<T>(ref T oldValue, T newValue, [CallerMemberName] string name = "")
        {
            //TODO Check equality

            oldValue = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}