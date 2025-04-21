using Metatable.Enums;

namespace Metatable.Models
{
    public class MetaProperty
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public PropertyType Type { get; set; }
        public bool IsHidden { get; set; }
    }
}