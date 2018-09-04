using CodingChallenge.Api.Models;
using System.Collections.Generic;

namespace CodingChallenge.Api.Services
{
    public class FloorProgress
    {
        public int Floor { get; set; }
        public Dictionary<JobStatus, double> StatusPercentage { get; set; }
        public Dictionary<JobStatus, int> StatusJobCount { get; set; }
    }
}
