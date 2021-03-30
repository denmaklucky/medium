using Metatable.Enums;

namespace Metatable.Models
{
    public class Metadata
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public DataType Type { get; set; }
        public bool IsHidden { get; set; }
    }
}