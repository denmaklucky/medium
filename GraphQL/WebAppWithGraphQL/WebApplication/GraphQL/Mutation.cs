using System.Threading.Tasks;
using HotChocolate;
using WebApplication.Models;

namespace WebApplication.GraphQL
{
    public class Mutation
    {
        public async Task<AddContactPayload> AddContact(AddContactInput input, [Service] AppContext context)
        {
            var contact = new Contact
            {
                Name = input.Name,
                Email = input.Email,
                Phone = input.Phone
            };
            contact.Notes.Add(new Note
            {
                Title = input.Title,
                Value = input.Note
            });
            
            await context.Contacts.AddAsync(contact);
            await context.SaveChangesAsync();

            return new AddContactPayload(contact);
        }
    }
}