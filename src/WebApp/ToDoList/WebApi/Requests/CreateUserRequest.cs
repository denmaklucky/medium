namespace WebApi.Requests;

public class CreateUserRequest
{
    public string Login { get; set; }
    public string Password { get; set; }
}