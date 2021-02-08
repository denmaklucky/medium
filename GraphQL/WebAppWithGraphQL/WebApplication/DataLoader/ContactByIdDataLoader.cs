using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate.DataLoader;
using Microsoft.EntityFrameworkCore;
using WebApplication.Models;

namespace WebApplication.DataLoader
{
    public class ContactByIdDataLoader : BatchDataLoader<int, Contact>
    {
        private readonly AppContext _context;

        public ContactByIdDataLoader(IBatchScheduler batchScheduler, AppContext context) : base(batchScheduler)
        {
            _context = context;
        }

        protected override async Task<IReadOnlyDictionary<int, Contact>> LoadBatchAsync(IReadOnlyList<int> keys, CancellationToken cancellationToken)
        {
            return await _context.Contacts.Include(c => c.Notes).Where(c => keys.Contains(c.Id)).ToDictionaryAsync(t => t.Id, cancellationToken);
        }
    }
}