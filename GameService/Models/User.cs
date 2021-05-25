using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameService.Models
{
    public class User
    {
        public string GameID { get; set; }
        public string UserID { get; set; }
        public List<string> ConnectionIds { get; set; } = new List<string>();
    }
}
