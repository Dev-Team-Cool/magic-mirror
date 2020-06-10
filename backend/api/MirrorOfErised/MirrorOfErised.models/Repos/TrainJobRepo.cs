using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MirrorOfErised.models.Data;

namespace MirrorOfErised.models.Repos
{
    public class TrainJobRepo: ITrainJobRepo
    {
        private readonly ApplicationDbContext _context;

        public TrainJobRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TrainJob>> GetAllJobs()
        {
            return await _context.TrainJobs.ToListAsync();
        }

        public async Task<TrainJob> AddJob(TrainJob trainJob)
        {
            await _context.TrainJobs.AddAsync(trainJob);
            await _context.SaveChangesAsync();
            return trainJob;
        }

        public async Task<TrainJob> Update(TrainJob trainJob)
        {
            _context.TrainJobs.Update(trainJob);
            await _context.SaveChangesAsync();

            return trainJob;
        }
    }
}