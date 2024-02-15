using NullableFox.AoXiangToDoList.Models;
using NullableFox.AoXiangToDoList.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.Services.Interfaces
{
    class PomodoroRecordService : IPomodoroRecordService
    {
        private INetworkService networkService;

        public event EventHandler<SystemCollectionChangedNotificationArgs> PomodoroRecordCollectionChanged;


        public async Task<List<PomodoroRecord>> PullPomodoroRecordsAsync()
        {
            var response = await networkService.RequestAsync(Transmission.RequestType.EnumeratePomodoroRecords, string.Empty);
            ExceptionHelper.ThrowOnTransFailure(response, "拉取番茄钟列表错误");
            return JsonHelper.ObjectFromJsonString<List<PomodoroRecord>>(response.Content);
        }

        public async Task<PomodoroRecord> QueryPomodoroRecord(int innerId)
        {
            var response = await networkService.RequestAsync(Transmission.RequestType.QueryPomodoroRecord, innerId.ToString());
            ExceptionHelper.ThrowOnTransFailure(response, "拉取番茄钟记录错误");
            return JsonHelper.ObjectFromJsonString<PomodoroRecord>(response.Content);
        }

        public PomodoroRecordService(INetworkService networkService, INotificationService notificationService)
        {
            this.networkService = networkService;
            notificationService.PomodoroRecordCollectionChanged += (s, e) => PomodoroRecordCollectionChanged?.Invoke(this, e);
        }
    }
}
