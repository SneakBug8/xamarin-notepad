using System.Threading.Tasks;
using System.Windows.Input;
using Notepad.Models;
using Xamarin.Forms;

namespace Notepad.ViewModels
{
    public class NoteEditViewModel : BaseViewModel
    {
        private NoteModel model;

        public NoteModel Model
        {
            get => model;
            set => SetProperty(ref model, value);
        }

        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public NoteEditViewModel()
        {
            SaveCommand = new Command(async () => await SaveAsync());
            DeleteCommand = new Command(async () => await DeleteAsync());

            messagingService.Subscribe<int>(this, LoadNote, async (id) => await LoadAsync(id));
        }

        public async Task LoadAsync(int id)
        {
            messagingService.Unsubscribe<int>(this, LoadNote);
            Model = await noteService.Read(id);
        }

        public async Task SaveAsync()
        {
            await noteService.Update(Model.Id, Model);
            messagingService.Send(NoteUpdated, Model);
            await navigationService.PopAsync();
        }

        public async Task DeleteAsync()
        {
            // Confirmation
            bool deleteFile = await displayService.AlertAsync("Deletion", "Are you sure you want to delete this note?", "Yes", "No");
            if (!deleteFile)
                return;

            await noteService.Delete(Model.Id);
            messagingService.Send(NoteDeleted, Model);
            await navigationService.PopAsync();
        }
    }
}