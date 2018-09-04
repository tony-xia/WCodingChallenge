using CodingChallenge.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CodingChallenge.Api.Controllers
{
    [ApiController]
    public class ProgressController : ControllerBase
    {
        private readonly IProgressService _progressService;

        public ProgressController(IProgressService progressService)
        {
            _progressService = progressService;
        }

        [HttpGet]
        [Route("api/progress")]
        [ProducesResponseType(typeof(List<FloorProgressResponse>), (int)HttpStatusCode.OK)]
        public IActionResult GetProgress()
        {
            var progress = _progressService.GetProgress();
            var result = progress
                             .Select(p => new FloorProgressResponse { Floor = p.Floor, StatusPercentage = p.StatusPercentage })
                             .ToList();
            return Ok(result);
        }
    }
}
