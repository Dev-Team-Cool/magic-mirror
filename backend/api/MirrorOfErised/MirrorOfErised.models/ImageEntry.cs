using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace MirrorOfErised.models
{
    public class ImageEntry
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string ImagePath { get; set; }
        public bool IsProcessed { get; set; }
        public bool IsValid { get; set; } = false;
        public DateTime CreatedAt { get; set; }
    }
}