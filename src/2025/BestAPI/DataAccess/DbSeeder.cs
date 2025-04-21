using Bogus;

namespace DataAccess;

public static class DbSeeder
{
    public static void Seed(UsersDbContext dbContext)
    {
        const int maxUserCount = 10_000;

        var faker = new Faker();
        
        for (var index = 0; index < maxUserCount; index++)
        {
            dbContext.Users.Add(new User
            {
                Id = Guid.CreateVersion7(),
                Email =  faker.Internet.Email()
            });
        }

        dbContext.SaveChanges();
    }
}