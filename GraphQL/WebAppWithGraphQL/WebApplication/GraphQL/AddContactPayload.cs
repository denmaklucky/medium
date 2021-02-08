using WebApplication.Models;

namespace WebApplication.GraphQL
{
    public class AddContactPayload
    {
        public AddContactPayload(Contact contact)
        {
            Contact = contact;
        }
        
        public Contact Contact { get; }
    }
}