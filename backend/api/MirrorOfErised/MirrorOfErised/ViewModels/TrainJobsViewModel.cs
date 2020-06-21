using System.Collections.Generic;
using MirrorOfErised.models;

namespace MirrorOfErised.ViewModels
{
    public class TrainJobsViewModel
    {
        public bool IsTrainable { get; set; }
        public List<TrainJob> Jobs { get; set; }
    }
}