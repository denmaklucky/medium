using System.Collections.Generic;

namespace Metatable.Models
{
    public class MetaObject
    {
        public string Id { get; set; }

        public List<MetaProperty> Properties { get; set; }
    }
}
