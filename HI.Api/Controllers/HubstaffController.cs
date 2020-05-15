using System;
using System.Threading.Tasks;
using HI.Hubstaff;
using Microsoft.AspNetCore.Mvc;

namespace HI.Api.Controllers
{
    [Route("api/[controller]")]
    public class HubstaffController : BaseApiController
    {
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAuth([FromServices] IHubstaffService service)
        {
            try
            {
                var result = await service.GenAuth();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.GetBaseException().Message);
            }
        }
    }
}