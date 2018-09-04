using CodingChallenge.Api.Models;
using System.Collections.Generic;

namespace CodingChallenge.Api.Controllers
{
    public class FloorProgressResponse
    {
        public int Floor { get; set; }
        public Dictionary<JobStatus, double> StatusPercentage { get; set; }
    }

}
