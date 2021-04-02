using Metatable.Enums;

namespace Metatable.Models
{
    public class MetaProperty
    {
        public MetaProperty(string name, object value, PropertyType type, bool isHidden = false)
        {
            Name = name;
            Value = value;
            Type = type;
            IsHidden = isHidden;
        }

        public string Name { get; set; }
        public object Value { get; set; }
        public PropertyType Type { get; set; }
        public bool IsHidden { get; set; }
    }
}