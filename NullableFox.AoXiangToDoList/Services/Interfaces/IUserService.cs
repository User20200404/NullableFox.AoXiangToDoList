using NullableFox.AoXiangToDoList.Models;
using NullableFox.AoXiangToDoList.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.Services.Interfaces
{
    internal interface IUserService
    {
        /// <summary>
        /// 从后端拉取User数据。
        /// </summary>
        /// <returns></returns>
        public Task<User> PullUserModelAsync();
        /// <summary>
        /// 将User信息更新到后端。
        /// </summary>
        /// <param name="userModel"></param>
        public Task PushUserAsync(UserInfo userInfo);
    }
}