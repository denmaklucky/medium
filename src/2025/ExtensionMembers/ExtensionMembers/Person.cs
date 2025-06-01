namespace ExtensionMembers;

public abstract class Person()
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public sealed class Student : Person
    {
        public string Major { get; set; }

        public int Year { get; set; }
    }

    public sealed class Professor : Person
    {
        public string Department { get; set; }

        public string Title { get; set; }
    }

    public sealed class Administrator : Person
    {
        public string Office { get; set; }

        public string Role { get; set; }
    }
}