using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.Models
{
    /// <summary>
    /// 表示待办事项列表变化时的通知参数。
    /// </summary>
    internal class SystemCollectionChangedNotificationArgs : EventArgs
    {
        public int? AddedItemInnerId { get; init; } 
        public int? RemovedItemInnerId { get; init; } 
        public int? ModifiedItemInnerId { get; init; }
        public CollectionOperationType OperationType { get; init; } = CollectionOperationType.NotIndicated;
    }

    /// <summary>
    /// 集合修改的操作类型。
    /// </summary>
    internal enum CollectionOperationType
    {
        NotIndicated,
        ItemRemoved,
        ItemAdded,
        CollectionCleared,
        ItemPropertyChanged
    }
}
