using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Notepad.Data;
using Notepad.Models;

namespace Notepad.Services
{
    public class SqlNoteService : INoteService
    {
        public async Task<NoteModel> Create(NoteModel model)
        {
            using (DataContext context = new DataContext())
            {
                context.Notes.Add(model);
                await context.SaveChangesAsync();
                return model;
            }
        }

        public async Task<NoteModel> Delete(int id)
        {
            using (DataContext context = new DataContext())
            {
                NoteModel entity = await context.Notes.FirstOrDefaultAsync(x => x.Id == id);
                if (entity == null)
                    return null;

                context.Remove(entity);
                await context.SaveChangesAsync();
                return entity;
            }
        }

        public async Task<IEnumerable<NoteModel>> Read()
        {
            using (DataContext context = new DataContext())
                return await context.Notes.ToListAsync();
        }

        public async Task<NoteModel> Read(int id)
        {
            using (DataContext context = new DataContext())
                return await context.Notes.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<NoteModel> Update(int id, NoteModel model)
        {
            using (DataContext context = new DataContext())
            {
                //context.Notes.Update(model);

                NoteModel entity = await context.Notes.FirstOrDefaultAsync(x => x.Id == id);
                if (entity == null)
                    return null;

                context.Entry(entity).CurrentValues.SetValues(model);
                await context.SaveChangesAsync();
                return entity;
            }
        }
    }
}