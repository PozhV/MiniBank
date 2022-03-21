using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniBank.Core.Domains.Users
{
    public class User
    {
        public string UserId { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
    }
}
