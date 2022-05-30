using System.Collections.Generic;
using System.Threading.Tasks;
using Notepad.Models;

namespace Notepad.Services
{
    public interface INoteService
    {
        Task<NoteModel> Create(NoteModel model);
        Task<NoteModel> Delete(int id);
        Task<IEnumerable<NoteModel>> Read();
        Task<NoteModel> Read(int id);
        Task<NoteModel> Update(int id, NoteModel model);
    }
}