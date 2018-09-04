using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CodingChallenge.Api.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task TestApi()
        {
            using (var factory = new WebApplicationFactory<Startup>())
            using (var client = factory.CreateClient())
            {
                var response = await client.GetAsync("api/values");

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }
    }
}
