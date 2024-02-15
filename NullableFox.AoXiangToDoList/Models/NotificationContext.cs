using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.Models
{
    internal class NotificationContext
    {
        public string ContextName { get; set; } 
        public int NotificationQueueMaxLength { get; set; }
    }
}
