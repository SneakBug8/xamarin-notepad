using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Notepad.Models;
using Xamarin.Forms;

namespace Notepad.ViewModels
{
    public class NoteListViewModel : BaseViewModel
    {
        private NoteModel selected;
        private bool isRefreshing;

        public NoteModel Selected
        {
            get => selected;
            set
            {
                selected = value;
                if (selected != null)
                    OpenCommand.Execute(selected.Id);
                SetProperty(ref selected, null); //Less visible selection and possibility to select the same object again
            }
        }

        public bool IsRefreshing
        {
            get => isRefreshing;
            set => SetProperty(ref isRefreshing, value);
        }

        public Command CreateCommand { get; }
        public Command RefreshCommand { get; }
        public Command<int> OpenCommand { get; }
        public ObservableCollection<NoteModel> Items { get; } = new ObservableCollection<NoteModel>();

        public NoteListViewModel()
        {
            CreateCommand = new Command(async () => await CreateAsync());
            RefreshCommand = new Command(async () => await RefreshAsync());
            OpenCommand = new Command<int>(async (id) => await OpenAsync(id));

            messagingService.Subscribe<NoteModel>(this, NoteUpdated, (model) =>
            {
                Items.Remove(model);
                Items.Add(model);
            });
            messagingService.Subscribe<NoteModel>(this, NoteDeleted, (model) => Items.Remove(model));

            RefreshCommand.Execute(null);
        }

        public async Task RefreshAsync()
        {
            IsRefreshing = true;

            Items.Clear();
            foreach (var item in await noteService.Read())
                Items.Add(item);

            IsRefreshing = false;
        }

        public async Task CreateAsync()
        {
            string noteName = await displayService.PromptAsync("Nouvelle note", "Nom pour la note:");
            if (string.IsNullOrEmpty(noteName))
                return;

            // File creation
            NoteModel model = new NoteModel() { Name = noteName };
            model = await noteService.Create(model);

            Items.Add(model);

            // View created note
            await navigationService.PushAsync("NoteEditView");
            messagingService.Send(LoadNote, model.Id);
        }

        public async Task OpenAsync(int id)
        {
            await navigationService.PushAsync("NoteEditView");
            messagingService.Send(LoadNote, id);
        }
    }
}