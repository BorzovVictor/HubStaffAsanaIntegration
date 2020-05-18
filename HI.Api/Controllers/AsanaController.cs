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
                throw new Exception("test exception");
                var data = await service.GetById(taskId);
                return Json(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.GetBaseException().Message);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> UpdateSunHours([FromServices] IAsanaService service)
        {
            try
            {
                var result = await service.UpdateSumFieldTask("1174870923930519", 3);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.GetBaseException().Message);
            }
        }
    }
}