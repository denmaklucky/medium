using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Metatable.Models;

namespace Metatable.Services
{
    public interface IMetaService
    {
        void CreateTable(TableModel table);
        Task UpdateTable(TableModel table);
        Task<TableModel> GetTable();
        int GetTableCount();
    }

    public class MetaService : IMetaService
    {
        private static List<TableModel> _repository;

        public MetaService()
            => _repository = new List<TableModel>();

        public void CreateTable(TableModel table)
        {
            _repository.Add(table);
        }

        public Task UpdateTable(TableModel table)
            => throw new NotImplementedException(); //_repository.UpdateTable(table.Id, table);

        public Task<TableModel> GetTable()
            => throw new NotImplementedException(); // _repository.GetFirstTable();

        public int GetTableCount()
            => _repository.Count;
    }
}