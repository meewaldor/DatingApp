using DatingAPI.Data;
using DatingAPI.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DatingAPI.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
    }

}
