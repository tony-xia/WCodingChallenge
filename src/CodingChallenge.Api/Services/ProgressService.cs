using CodingChallenge.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodingChallenge.Api.Services
{
    public class ProgressService : IProgressService
    {
        private BuildingDbContext _dbContext;

        public ProgressService(BuildingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<FloorProgress> GetProgress()
        {
            var result = from job in _dbContext.Jobs
                          group job by new { job.Floor, job.StatusNum } into g
                          select new
                          {
                              Floor = g.Key.Floor,
                              Status = g.Key.StatusNum,
                              Count = g.Count()
                          };

            var progress = new Dictionary<int, FloorProgress>();
            foreach (var floorStatusJobs in result)
            {
                if (!progress.TryGetValue(floorStatusJobs.Floor, out FloorProgress floorProgress))
                {
                    floorProgress = new FloorProgress
                    {
                        Floor = floorStatusJobs.Floor,
                        StatusPercentage = new Dictionary<JobStatus, double>(),
                        StatusJobCount = new Dictionary<JobStatus, int>()
                    };
                    progress[floorStatusJobs.Floor] = floorProgress;
                }

                floorProgress.StatusJobCount[floorStatusJobs.Status] = floorStatusJobs.Count;
            }

            ComputePercentage(progress);

            return progress.Values.OrderBy(f => f.Floor).ToList();
        }

        private void ComputePercentage(Dictionary<int, FloorProgress> progress)
        {
            foreach (var floorProgress in progress.Values)
            {
                double totalCount = floorProgress.StatusJobCount.Values.Sum();

                var leftPercentage = 1.00;
                for (int i = 0; i < floorProgress.StatusJobCount.Count; i++)
                {
                    var status = floorProgress.StatusJobCount.Keys.ElementAt(i);

                    bool isLastStatus = i == floorProgress.StatusJobCount.Count - 1;
                    if (!isLastStatus)
                    {
                        var count = floorProgress.StatusJobCount[status];
                        var percentage = Math.Round(count / totalCount, 2);
                        floorProgress.StatusPercentage[status] = percentage;
                        leftPercentage -= percentage;
                    }
                    else
                    {
                        floorProgress.StatusPercentage[status] = Math.Round(leftPercentage, 2);
                    }
                }
            }
        }
    }

}
