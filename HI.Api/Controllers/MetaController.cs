using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace HI.Api.Controllers
{
    public class MetaController : BaseApiController
    {
        /// <summary>
        /// returns ProductVersion and GetCreationTime
        /// </summary>
        /// <returns></returns>
        [HttpGet("/info")]
        [ProducesResponseType(typeof(string), 200)]
        public ActionResult<string> Info()
        {
            var assembly = typeof(Startup).Assembly;

            var creationDate = System.IO.File.GetCreationTime(assembly.Location);
            var version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

            return Ok($"Version: {version}, Last Updated: {creationDate}");
        }
    }
}