namespace FunctionalStuff;

public interface IAuthorizationService
{
    Task<bool> HaveAccessAsync(Guid userId);
}