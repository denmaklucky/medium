using System.Collections.Generic;

namespace Metatable.Models
{
    public class TableModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<MetaObject> Objects { get; set; }
    }
}