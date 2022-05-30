using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Notepad.Models;

namespace Notepad.Services
{
    // CRUD Filesystem service
    public class FileNoteService : INoteService
    {
        private static readonly string directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        private static int id = 1;

        static FileNoteService()
        {
            // Routine to convert file names
            foreach (var item in Directory.GetFiles(directory, "*.txt"))
            {
                string filename = Path.GetFileNameWithoutExtension(item);
                if (filename.Contains('-'))
                    break;

                File.Move(item, Path.Combine(directory, $"{id}-{filename}.txt"));
                id++;
            }

            // Search for max id
            id =
            Directory.GetFiles(directory, "*.txt")
               .Select(x => Path.GetFileNameWithoutExtension(x))
               .Select(x => x.Substring(0, x.IndexOf("-")))
               .Max(x => int.Parse(x)) + 1;
        }

        public async Task<IEnumerable<NoteModel>> Read()
        {
            return Directory.GetFiles(directory, "*.txt").Select(x =>
            {
                string filename = Path.GetFileNameWithoutExtension(x);
                int index = filename.IndexOf('-');

                return new NoteModel
                {
                    Id = int.Parse(filename.Substring(0, index)),
                    Name = filename.Substring(index + 1),
                    Content = File.ReadAllText(x)
                };
            });
        }

        public async Task<NoteModel> Read(int id)
        {
            string[] files = Directory.GetFiles(directory, $"{id}-*.txt");
            if (files.Length != 1)
                throw new KeyNotFoundException();

            string fileName = Path.GetFileNameWithoutExtension(files[0]);

            NoteModel model = new NoteModel()
            {
                Id = id,
                Name = fileName.Substring(fileName.IndexOf('-') + 1),
                Content = File.ReadAllText(files[0]),
            };

            return await Task.FromResult(model);
        }

        public async Task<NoteModel> Create(NoteModel model)
        {
            // Definition of id
            model.Id = id++;

            // File creation
            string filePath = Path.Combine(directory, $"{model.Id}-{model.Name}.txt");
            File.WriteAllText(filePath, model.Content);

            return await Task.FromResult(model);
        }

        public async Task<NoteModel> Update(int id, NoteModel model)
        {
            string[] files = Directory.GetFiles(directory, $"{id}-*.txt");
            if (files.Length != 1)
                throw new KeyNotFoundException();

            // Delete existing file
            File.Delete(files[0]);

            // File creation
            string filePath = Path.Combine(directory, $"{id}-{model.Name}.txt");
            File.WriteAllText(filePath, model.Content);
            model.Id = id;

            return await Task.FromResult(model);
        }

        public async Task<NoteModel> Delete(int id)
        {
            string[] files = Directory.GetFiles(directory, $"{id}-*.txt");
            if (files.Length != 1)
                throw new KeyNotFoundException();

            // Delete existing file
            File.Delete(files[0]);

            return await Task.FromResult(new NoteModel { Id = id });
        }
    }
}