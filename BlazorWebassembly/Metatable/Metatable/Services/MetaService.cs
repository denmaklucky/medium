using System.Threading.Tasks;
using Metatable.Models;

namespace Metatable.Services
{
    public interface IMetaService
    {
        void CreateTable(TableModel table);
        Task UpdateTable(TableModel table);
        Task<TableModel> GetTable();
    }

    public class MetaService : IMetaService
    {
        private readonly IAppRepository _repository;

        public MetaService(IAppRepository repository)
            => _repository = repository;

        public void CreateTable(TableModel table)
        {
            _repository.CreateTable(table);
        }

        public Task UpdateTable(TableModel table)
            => _repository.UpdateTable(table.Id, table);

        public Task<TableModel> GetTable()
            => _repository.GetFirstTable();
    }
}