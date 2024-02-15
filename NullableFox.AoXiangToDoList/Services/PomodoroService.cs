using NullableFox.AoXiangToDoList.Models;
using NullableFox.AoXiangToDoList.Services.Interfaces;
using NullableFox.AoXiangToDoList.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.Services
{
    internal class PomodoroService : IPomodoroService
    {
        private INetworkService networkService;
        private INotificationService notificationService;
        public PomodoroService(INetworkService service, INotificationService notificationService)
        {
            this.networkService = service;
            this.notificationService = notificationService;
            notificationService.PomodoroStatusChanged += NotificationService_PomodoroStatusChanged;
        }

        private void NotificationService_PomodoroStatusChanged(object sender, PomodoroStatusChangedNotificationArgs e)
        {
            PomodoroStatusChanged?.Invoke(this, e);
        }

        /// <summary>
        /// 番茄钟的状态被改变。
        /// </summary>
        public event EventHandler<PomodoroStatusChangedNotificationArgs> PomodoroStatusChanged;



        public async Task RequestEditAsync((int WorkTime, int RestTime) param)
        {
            //创建匿名对象。不能直接用参数中的param，这样属性名会变成Item1和Item2
            var obj = new
            {
                WorkTime = param.WorkTime,
                RestTime = param.RestTime
            };
            var packet = await networkService.RequestAsync(Transmission.RequestType.EditPomodoro, obj.ToJsonString());
            ExceptionHelper.ThrowOnTransFailure(packet, "编辑番茄钟错误");
        }

        public async Task RequestStartAsync(int toDoInnerId)
        {
            var packet = await networkService.RequestAsync(Transmission.RequestType.StartPomodoro, toDoInnerId.ToString());
            ExceptionHelper.ThrowOnTransFailure(packet, "启动番茄钟错误");
        }

        public async Task RequestStopAsync()
        {
            var packet = await networkService.RequestAsync(Transmission.RequestType.EndPomodoro, string.Empty);
            ExceptionHelper.ThrowOnTransFailure(packet, "停止番茄钟错误");
        }

        public async Task<Pomodoro> RequestPullAsync()
        {
            var packet = await networkService.RequestAsync(Transmission.RequestType.GetPomodoro, string.Empty);
            ExceptionHelper.ThrowOnTransFailure(packet, "启动番茄钟错误");
            return JsonHelper.ObjectFromJsonString<Pomodoro>(packet.Content);
        }
    }
}
