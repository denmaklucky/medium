namespace Pipeline
{
    public class ValidationError
    {
        public ValidationError(string property, string error)
            => (Property, Error) = (property, error);

        public string Property { get; set; }

        public string Error { get; set; }
    }
}
