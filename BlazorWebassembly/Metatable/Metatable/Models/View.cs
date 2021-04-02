using Metatable.Enums;
using System.Collections.Generic;

namespace Metatable.Models
{
    public class View
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ViewType Type { get; set; }
        public List<MetaObject> Objects { get; set; }
    }
}