using System.Collections.Generic;

namespace WebApplication.Models
{
    public class Note
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public string Value { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();

        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}