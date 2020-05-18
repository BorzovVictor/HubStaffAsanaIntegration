using System;
using System.Threading.Tasks;
using HI.Api.UseCases;
using HI.Hubstaff;
using HI.SharedKernel.Models;
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
        
        [HttpGet("[action]")]
        public async Task<IActionResult> Tasks([FromServices] IHubstaffService service, string projects, int offset)
        {
            try
            {
                var result = await service.Tasks(projects, offset);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.GetBaseException().Message);
            }
        }
        
        [HttpGet("[action]")]
        public async Task<IActionResult> TeamByMember(
            [FromServices] IHubstaffService service, 
            [FromQuery]HsTeamMemberRequest req)
        {
            try
            {
                var result = await service.GetTasksDurations(req);
                
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.GetBaseException().Message);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> UpdateSumFields([FromServices] IUpdateSumFieldsCase service)
        {
            try
            {
                var result = await service.ExecuteNoUpdate(DateTime.Today.AddDays(-1), DateTime.Today);
                return result.Succeded
                    ? (IActionResult) Ok(result.Success)
                    : BadRequest(result.Failure);
            }
            catch (Exception e)
            {
                return BadRequest(e.GetBaseException().Message);
            }
        }
    }
}