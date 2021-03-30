using System.Threading.Tasks;
using MongoDB.Driver;

namespace Metatable.Models
{
    public interface IAppRepository
    {
        Task<TableModel> GetFirstTable();
        Task UpdateTable(string id, TableModel table);
        void CreateTable(TableModel table);
    }

    public class AppRepository : IAppRepository
    {
        private readonly IMongoCollection<TableModel> _tables;

        public AppRepository(IMongoClient client)
        {
            var database = client.GetDatabase("Metatable");
            _tables = database.GetCollection<TableModel>("Tables");
        }

        public Task<TableModel> GetFirstTable()
            => _tables.FindAsync(table => true).Result.FirstOrDefaultAsync();

        public void CreateTable(TableModel table)
            => _tables.InsertOne(table);

        public Task UpdateTable(string id, TableModel table)
            => _tables.ReplaceOneAsync(t => t.Id == id, table);
    }
}