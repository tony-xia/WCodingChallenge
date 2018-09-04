using CodingChallenge.Api.Models;
using CodingChallenge.Api.Services;
using CodingChallenge.Api.Tests.Infrastructure;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CodingChallenge.Api.Tests
{
    public class ApiProgressTests
    {
        [Fact]
        public async Task GivenNoJobs_ThenProgressDoesNotContainAnyFloor()
        {
            var dataSeed = new DataSeed();

            using (var factory = new ServerFactory(dataSeed))
            {
                var response = await factory.CreateClient().GetAsync<List<FloorProgress>>("api/progress");

                response.Count.Should().Be(0);
            }
        }

        [Fact]
        public async Task GivenSomeFloorsHasJobs_ThenProgressOnlyContainsTheFloorsWhichHaveJobs()
        {
            var dataSeed = new DataSeed()
            {
                Jobs = new List<Job>()
                {
                    new Job { Floor = 1, StatusNum = JobStatus.Complete },
                    new Job { Floor = 3, StatusNum = JobStatus.Delayed },
                }
            };

            using (var factory = new ServerFactory(dataSeed))
            {
                var response = await factory.CreateClient().GetAsync<List<FloorProgress>>("api/progress");

                response.Count.Should().Be(2);
                response.Should().ContainSingle(f => f.Floor == 1);
                response.Should().ContainSingle(f => f.Floor == 3);
            }
        }

        [Fact]
        public async Task GivenAFloorHasNoJobInTheSpecificStatus_ThenProgressOfThatFloorDoesNotContainsThatStatus()
        {
            var dataSeed = new DataSeed()
            {
                Jobs = new List<Job>()
                {
                    new Job { Floor = 1, StatusNum = JobStatus.Complete },
                    new Job { Floor = 1, StatusNum = JobStatus.Delayed },
                    new Job { Floor = 1, StatusNum = JobStatus.InProgress },
                }
            };

            using (var factory = new ServerFactory(dataSeed))
            {
                var response = await factory.CreateClient().GetAsync<List<FloorProgress>>("api/progress");

                response.Count.Should().Be(1);
                response[0].StatusPercentage.Count.Should().Be(3);
                response[0].StatusPercentage.Should().NotContainKey(JobStatus.NotStarted);
            }
        }

        [Fact]
        public async Task GivenAFloorHasThreeJobInThreeDifferentStatus_ThenTotalPercentageShouldBe100()
        {
            var dataSeed = new DataSeed()
            {
                Jobs = new List<Job>()
                {
                    new Job { Floor = 1, StatusNum = JobStatus.Complete },
                    new Job { Floor = 1, StatusNum = JobStatus.Delayed },
                    new Job { Floor = 1, StatusNum = JobStatus.InProgress },
                }
            };

            using (var factory = new ServerFactory(dataSeed))
            {
                var response = await factory.CreateClient().GetAsync<List<FloorProgress>>("api/progress");

                response.Count.Should().Be(1);
                response[0].StatusPercentage.Sum(s => s.Value).Should().Be(1.00);
            }
        }

        [Theory]
        [InlineData(1, 1, 1, 1, 0.25, 0.25, 0.25, 0.25)]
        [InlineData(2, 3, 2, 3, 0.20, 0.30, 0.20, 0.30)]
        [InlineData(0, 7, 0, 7, 0.00, 0.50, 0.00, 0.50)]
        [InlineData(1, 2, 3, 4, 0.10, 0.20, 0.30, 0.40)]
        public async Task GivenJobsInDifferentStatus_ThenPercentagesAreCalculatedCorrectly(
            int completeJobCount,
            int notStartedJobCount,
            int inProgressJobCount,
            int delayedJobCount,
            double completePercentage,
            double notStartedPercentage,
            double inProgressPercentage,
            double delayedPercentage)
        {
            var dataSeed = new DataSeed();
            for (int i = 0; i < completeJobCount; i++)
            {
                dataSeed.Jobs.Add(new Job { Floor = 1, StatusNum = JobStatus.Complete });
            }
            for (int i = 0; i < notStartedJobCount; i++)
            {
                dataSeed.Jobs.Add(new Job { Floor = 1, StatusNum = JobStatus.NotStarted });
            }
            for (int i = 0; i < inProgressJobCount; i++)
            {
                dataSeed.Jobs.Add(new Job { Floor = 1, StatusNum = JobStatus.InProgress });
            }
            for (int i = 0; i < delayedJobCount; i++)
            {
                dataSeed.Jobs.Add(new Job { Floor = 1, StatusNum = JobStatus.Delayed });
            }

            using (var factory = new ServerFactory(dataSeed))
            {
                var response = await factory.CreateClient().GetAsync<List<FloorProgress>>("api/progress");

                response.Count.Should().Be(1);
                var progress = response[0];
                if (completeJobCount > 0)
                {
                    progress.StatusPercentage.Should().ContainKey(JobStatus.Complete);
                    progress.StatusPercentage[JobStatus.Complete].Should().Be(completePercentage);
                }
                else
                {
                    progress.StatusPercentage.Should().NotContainKey(JobStatus.Complete);
                }
                if (notStartedJobCount > 0)
                {
                    progress.StatusPercentage.Should().ContainKey(JobStatus.NotStarted);
                    progress.StatusPercentage[JobStatus.NotStarted].Should().Be(notStartedPercentage);
                }
                else
                {
                    progress.StatusPercentage.Should().NotContainKey(JobStatus.NotStarted);
                }
                if (inProgressJobCount > 0)
                {
                    progress.StatusPercentage.Should().ContainKey(JobStatus.InProgress);
                    progress.StatusPercentage[JobStatus.InProgress].Should().Be(inProgressPercentage);
                }
                else
                {
                    progress.StatusPercentage.Should().NotContainKey(JobStatus.InProgress);
                }
                if (delayedJobCount > 0)
                {
                    progress.StatusPercentage.Should().ContainKey(JobStatus.Delayed);
                    progress.StatusPercentage[JobStatus.Delayed].Should().Be(delayedPercentage);
                }
                else
                {
                    progress.StatusPercentage.Should().NotContainKey(JobStatus.Delayed);
                }
            }
        }

    }
}
