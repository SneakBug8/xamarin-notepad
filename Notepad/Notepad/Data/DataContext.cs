using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Notepad.Models;

namespace Notepad.Data
{
    public class DataContext : DbContext
    {
        private static readonly string directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public DbSet<NoteModel> Notes { get; set; }

        public DataContext()
            : base()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string filePath = Path.Combine(directory, "Notepadv1.db3");
            optionsBuilder.UseSqlite($"Filename={filePath}");
        }
    }
}