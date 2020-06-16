using System.Collections.Generic;
using System.Threading.Tasks;

namespace MirrorOfErised.models.Repos
{
    public interface ITrainJobRepo: IBaseRepo
    {
        Task<List<TrainJob>> GetAllJobs();
        Task<TrainJob> AddJob(TrainJob trainJob);
        Task<TrainJob> Update(TrainJob trainJob);
    }
}