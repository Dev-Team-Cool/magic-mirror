using System.Collections.Generic;
using System.Threading.Tasks;

namespace MirrorOfErised.models.Services
{
    public interface ITrainJobService
    {
        public Task<List<TrainJob>> GetAllJobs();
        public Task<RunnerResult> StartJob();
    }
}