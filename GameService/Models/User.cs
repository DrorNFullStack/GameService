using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameService.Models
{
    public class User
    {
        public string Username { get; set; }
        public Guid GameID { get; set; }
    }
}
