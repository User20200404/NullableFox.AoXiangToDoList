using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using NullableFox.AoXiangToDoList.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.Views
{
    /// <summary>
    /// 提供了异常信息显示的扩展方法。
    /// </summary>
    internal static class ExceptionPresenter
    {
        public static async Task ShowByDialogAsync(this Exception exception, XamlRoot root)
        {
            ContentDialog contentDialog = BuildContentDialogFor(exception, root);
            await contentDialog.ShowAsync();
        }

        /// <summary>
        /// 向用户推送异常对话框。如果前台已经有异常对话框，则暂时隐藏已经显示的对话框，直到本对话框关闭。
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="root"></param>
        public static void PushExceptionDialog(this Exception exception, XamlRoot root)
        {
            DialogPresenter.Push(BuildContentDialogFor(exception, root));
        }

        /// <summary>
        /// 为指定异常构造ContentDialog。
        /// </summary>
        /// <param name="exception">异常对象。</param>
        /// <param name="root">XamlRoot</param>
        /// <returns>构造的ContentDialog。</returns>
        static ContentDialog BuildContentDialogFor(Exception exception,XamlRoot root)
        {
            string title, content;
            ContentDialog dialog = new ContentDialog()
            {
                XamlRoot = root,
                IsSecondaryButtonEnabled=false,
                PrimaryButtonText = "关闭",
                DefaultButton = ContentDialogButton.Primary
            };

            if(exception is ApplicationShowableException showableException)
            {
                title = showableException.Title;
                content = showableException.Description;
            }
            else
            {
                title = "发生内部错误";
                content = $"调试信息：{exception.Message}";
            }
            dialog.Title = title;
            dialog.Content = content;
            return dialog;
        }
    }
}
