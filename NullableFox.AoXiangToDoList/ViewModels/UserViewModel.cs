using CommunityToolkit.Mvvm.ComponentModel;
using NullableFox.AoXiangToDoList.Models;
using NullableFox.AoXiangToDoList.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.ViewModels
{
    internal class UserViewModel : MultiThreadBindableBase
    {
        User user = new User();
        IUserService service;
        public UserViewModel(IUserService userService)
        {
            this.service = userService;
        }

        public async Task UpdateAsync()
        {
            User model = await service.PullUserModelAsync();
            this.UserName = model.UserName;
            this.Account = model.Account;
            this.EncryptedPassword = model.EncryptedPassword;
            this.Token = model.Token;
        }

        public async Task ModifyUserInfoAsync()
        {
            await service.PushUserAsync(new UserInfo { Account = string.Empty, UserName = this.UserName, Password = "jvavisthebestlanguage1!", NewPassword = "Rrawgb773@" });
        }

        public string UserName
        {
            get => user.UserName;
            set
            {
                if (user.UserName != value)
                {
                    user.UserName = value;
                    OnPropertyChanged(nameof(UserName));
                }
            }
        }
        public string Account
        {
            get => user.Account;
            set
            {
                if (user.Account != value)
                {
                    user.Account = value;
                    OnPropertyChanged(nameof(Account));
                }
            }
        }
        public string EncryptedPassword
        {
            get => user.EncryptedPassword;
            set
            {
                if (user.EncryptedPassword != value)
                {
                    user.EncryptedPassword = value;
                    OnPropertyChanged(nameof(EncryptedPassword));
                }
            }
        }
        public string Token
        {
            get => user.Token;
            set
            {
                if (user.Token != value)
                {
                    user.Token = value;
                    OnPropertyChanged(nameof(Token));
                }
            }
        }
    }
}

