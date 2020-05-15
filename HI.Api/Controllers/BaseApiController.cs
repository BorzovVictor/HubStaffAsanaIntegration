using Microsoft.AspNetCore.Mvc;

namespace HI.Api.Controllers
{
    [Produces("application/json")]
    [ApiController]
    public abstract class BaseApiController : Controller
    {
    }
}