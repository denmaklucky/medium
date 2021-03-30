using System.Collections.Generic;

namespace Metatable.Models
{
    public class Table
    {
        public string Name { get; set; }
        public List<Metadata> Data { get; set; }
    }
}