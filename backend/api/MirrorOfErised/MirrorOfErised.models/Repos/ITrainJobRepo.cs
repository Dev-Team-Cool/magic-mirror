using System.Collections.Generic;
using System.Threading.Tasks;

namespace MirrorOfErised.models.Repos
{
    public interface ITrainJobRepo
    {
        Task<List<TrainJob>> GetAllJobs();
        Task<TrainJob> AddJob(TrainJob trainJob);
        Task<TrainJob> Update(TrainJob trainJob);
    }
}