using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MirrorOfErised.models.Repos
{
    public interface IImageEntryRepo: IBaseRepo
    {
        Task<int> CountImagesForUser(string userId);
        Task<bool> NeedsTraining();
        Task<ImageEntry> GetImageById(int id);
        Task<ImageEntry> GetImageByName(string name);
        Task<List<ImageEntry>> GetImagesForUserId(string userId);
        Task<List<ImageEntry>> GetAllUnprocessedImages(DateTime? before = null);
        Task<ImageEntry> AddImage(ImageEntry image);
        void Update(ImageEntry image);
        void UpdateAll(List<ImageEntry> images);
    }
}