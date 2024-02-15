using NullableFox.AoXiangToDoList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.Services.Interfaces
{
    internal interface IToDoWorkItemService
    {
        public Task<ToDoWorkItem> QueryToDoWorkItemAsync(int innderId);
        public Task<List<ToDoWorkItem>> PullToDoWorkItemsAsync();
        public Task<int> CreateToDoWorkAsync(ToDoWorkItem toDoWorkItem);
        public Task EditToDoWorkItemAsync(ToDoWorkItem toDoWorkItem);
        public Task DeleteToDoWorkItemAsync(int innerId);
        public event EventHandler<SystemCollectionChangedNotificationArgs> ToDoCollectionChanged;
    }
}
