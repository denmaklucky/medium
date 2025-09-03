using Bogus;

namespace CompressionApp;

public sealed class UserFactory : Faker<User>
{
    public UserFactory()
    {
        RuleFor(user => user.Id, _ => Guid.CreateVersion7());
        RuleFor(user => user.FirstName, faker => faker.Name.FirstName());
        RuleFor(user => user.LastName, faker => faker.Name.LastName());
        RuleFor(user => user.Email, faker => faker.Internet.Email());
    }
}