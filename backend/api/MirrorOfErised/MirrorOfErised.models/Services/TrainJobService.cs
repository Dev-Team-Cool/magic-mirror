using System.Collections.Generic;
using System.Threading.Tasks;
using MirrorOfErised.models.Repos;

namespace MirrorOfErised.models.Services
{
    public class TrainJobService: ITrainJobService
    {
        private readonly ITrainJobRepo _trainJobRepo;
        private readonly PythonRunner _pythonRunner;

        public TrainJobService(ITrainJobRepo trainJobRepo, PythonRunner pythonRunner)
        {
            _trainJobRepo = trainJobRepo;
            _pythonRunner = pythonRunner;
        }

        public async Task<List<TrainJob>> GetAllJobs()
        {
            return await _trainJobRepo.GetAllJobs();
        }

        public async Task<string> StartJob()
        {
            TrainJob startedJob = await _trainJobRepo.AddJob(new TrainJob());
            RunnerResult result = await _pythonRunner.StartTraining();
            if (!result.Failed)
            {
                startedJob.IsSuccessful = true;
            }
            await _trainJobRepo.Update(startedJob);
            return string.IsNullOrEmpty(result.Errors) ? result.Output : result.Errors;
        }
    }
}