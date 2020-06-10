using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MirrorOfErised.models
{
    public class TrainJob
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public bool IsSuccessful { get; set; } = false;
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime StartedAt { get; set; }
    }
}