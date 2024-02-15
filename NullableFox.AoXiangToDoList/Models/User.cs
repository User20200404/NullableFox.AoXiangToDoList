using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.Models
{
    internal class User
    {
        public string UserName { get; set; } 
        public string Account { get; set;}
        public string EncryptedPassword { get; set;}
        public string Token { get; set; }
    }
}
