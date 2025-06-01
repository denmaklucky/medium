using ExtensionMembers;


var person = new Person.Student();

if (person is Person.Student student)
{
    if (student.Major?.Contains("Computer") ?? false)
    {
        
    }
}


if (person.IsStudent)
{
    if (person.IsSoftwareEngineer)
    {

    }
}

Enum.Parse<Types>("One");

Types.Parse("One");

var list = new List<int>();

ArgumentException.ThrowIfEmpty(list);