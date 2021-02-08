using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using Microsoft.EntityFrameworkCore;
using WebApplication.DataLoader;
using WebApplication.Models;

namespace WebApplication.GraphQL
{
    public class Query
    {
        public IQueryable<Contact> GetContacts([Service] AppContext context)
            => context.Contacts.Include(c => c.Notes);

        public Task<Contact> GetContact(int id, ContactByIdDataLoader dataLoader, CancellationToken token)
            => dataLoader.LoadAsync(id, token);

        public IQueryable<Project> GetProjects([Service] AppContext context)
            => context.Projects;

        public IQueryable<Note> GetNotes([Service] AppContext context)
            => context.Notes;
    }
}