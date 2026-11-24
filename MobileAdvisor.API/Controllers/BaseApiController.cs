using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MobileAdvisor.API.Controllers;

[ApiController]
public abstract class BaseApiController : ControllerBase
{
    protected int CurrentUserId
    {
        get
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(idClaim, out var id) ? id : 0;
        }
    }

    protected string CurrentUserRole => User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
}
