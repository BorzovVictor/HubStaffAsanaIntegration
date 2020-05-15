using System;
using System.Threading.Tasks;
using HI.Asana;
using Microsoft.AspNetCore.Mvc;

namespace HI.Api.Controllers
{
    [Route("api/[controller]")]
    public class AsanaController : BaseApiController
    {
        [HttpGet("[action]/{taskId}")]
        public async Task<IActionResult> GetTaskById(string taskId, [FromServices] IAsanaService service)
        {
            try
            {
                var data = await service.GetById(taskId);
                return Json(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.GetBaseException().Message);
            }
        }
    }
}