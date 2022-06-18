using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Notepad.Models
{
    public class NoteModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public NoteModel()
        {
        }
        public int Id { get; set; }
        
        //[Required]
        public string Name { get; set; }
        public string Content { get; set; } = "";

        [NotMapped]
        public string _Content
        {
            get { return Content; }
            set { Content = value; OnPropertyChanged(nameof(_Content)); }
        }


        public string Excerpt { get
            {
                return this.Content.Substring(0, Math.Min(50, this.Content.Length));
            }
        }

        //public override string ToString()
        //{
        //    return $"{Id} - {Name}";
        //}

        public override bool Equals(object obj)
        {
            return base.Equals(obj) || (obj is NoteModel note && note.Id == Id);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}