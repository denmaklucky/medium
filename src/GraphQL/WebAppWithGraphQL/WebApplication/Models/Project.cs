using System.Collections.Generic;

namespace WebApplication.Models
{
    public class Project
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();

        public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
    }
}