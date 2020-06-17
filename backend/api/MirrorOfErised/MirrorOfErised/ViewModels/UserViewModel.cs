using System.Collections.Generic;
using IronPython.Runtime;
using MirrorOfErised.models;

namespace MirrorOfErised.ViewModels
{
    public class UserViewModel
    {
        public List<User> Users { get; set; }
        public User changedUser { get; set; }
    }
}