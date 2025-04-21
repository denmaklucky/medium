using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Metatable.Models;

namespace Metatable.Services
{
    public interface IMetaService
    {
        void CreateTable(View table);
        Task UpdateTable(View table);
        Task<View> GetTable();
        int GetTableCount();
    }

    public class MetaService : IMetaService
    {
        private static List<View> _repository;

        public MetaService()
            => _repository = new List<View>();

        public void CreateTable(View table)
        {
            _repository.Add(table);
        }

        public Task UpdateTable(View table)
            => throw new NotImplementedException(); //_repository.UpdateTable(table.Id, table);

        public Task<View> GetTable()
            => throw new NotImplementedException(); // _repository.GetFirstTable();

        public int GetTableCount()
            => _repository.Count;
    }
}