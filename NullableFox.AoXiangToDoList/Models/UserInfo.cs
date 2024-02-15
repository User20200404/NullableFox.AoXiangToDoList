using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.Models
{
    internal class UserInfo
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string NewPassword { get; set; }
        public required string Account { get; set; }
    }
}
