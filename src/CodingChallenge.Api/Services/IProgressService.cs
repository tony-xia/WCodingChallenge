using System.Collections.Generic;

namespace CodingChallenge.Api.Services
{
    public interface IProgressService
    {
        List<FloorProgress> GetProgress();
    }
}
