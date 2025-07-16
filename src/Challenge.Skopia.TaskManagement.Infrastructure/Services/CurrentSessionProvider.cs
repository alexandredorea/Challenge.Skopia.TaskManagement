namespace Challenge.Skopia.TaskManagement.Infrastructure.Services;

using Microsoft.AspNetCore.Http;
using System.Security.Claims;

public interface ICurrentSessionProvider
{
    int? GetUserId();
}

public class CurrentSessionProvider : ICurrentSessionProvider
{
    private readonly int? _currentUserId;

    public CurrentSessionProvider(IHttpContextAccessor accessor)
    {
        var userId = accessor.HttpContext?.User.FindFirstValue("userid");
        if (userId is null)
        {
            return;
        }

        _currentUserId = int.TryParse(userId, out var newId) ? newId : null;
    }

    public int? GetUserId() => _currentUserId;
}