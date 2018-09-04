using CodingChallenge.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        [ProducesResponseType(typeof(List<FloorProgress>), (int)HttpStatusCode.OK)]
        public IActionResult GetProgress()
        {
            var result = _progressService.GetProgress();
            return Ok(result);
        }
    }
}
