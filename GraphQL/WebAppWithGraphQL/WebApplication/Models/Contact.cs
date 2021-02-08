using System.Collections.Generic;

namespace WebApplication.Models
{
    public class Contact
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

        public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
    }
}