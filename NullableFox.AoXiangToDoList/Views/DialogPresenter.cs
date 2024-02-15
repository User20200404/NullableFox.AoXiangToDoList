using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using NullableFox.AoXiangToDoList.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace NullableFox.AoXiangToDoList.Views
{
    /// <summary>
    /// 提供了基于栈的ContentDialog管理功能，保证同一时间只会有一个对话框出现。
    /// </summary>
    internal class DialogPresenter
    {
        static Stack<ContentDialog> dialogStacks = new Stack<ContentDialog>();

        /// <summary>
        /// 给用户推送指定ContentDialog。如果已经有对话框显示，则暂时关闭原先对话框并显示该对话框，当用户关闭此对话框时，再依此显示原先的对话框（栈结构）。
        /// ContentDialog必须提前设置好XamlRoot，且Content属性必须为 UIElement 或 string 类型，本方法会为传入的对话框添加额外信息元素（例如剩余多少对话框）。
        /// 不允许重复添加同一个ContentDialog。
        /// </summary>
        /// <param name="dialog">对话框。</param>
        /// <exception cref="ArgumentNullException">当<paramref name="dialog"/>为null时抛出。</exception>
        /// <exception cref="ArgumentException"></exception>
        public static void Push(ContentDialog dialog)
        {
            if (dialog is null) throw new ArgumentNullException(nameof(dialog));
            if (dialogStacks.Contains(dialog)) throw new ArgumentException("不能重复添加同一个ContentDialog实例。");

            lock (dialogStacks)
            {
                if (dialogStacks.Any())
                {
                    var topDialog = dialogStacks.Peek();
                    topDialog.Closed -= OnTopDialogClosed;
                    topDialog.Hide();
                }
                WrapContentDialog(dialog); //这一条必须在Push前调用，保证显示的剩余条数信息正确。

                dialogStacks.Push(dialog);
                dialog.Closed += OnTopDialogClosed;
                _ = dialog.ShowAsync();
            }
        }

        static void OnTopDialogClosed(ContentDialog dialog, ContentDialogClosedEventArgs args)
        {
            UnwrapContentDialog(dialog); //对该对话框内容进行解包。
            lock (dialogStacks)
            {
                dialogStacks.Pop();
                if (dialogStacks.Any())
                {
                    var topDialog = dialogStacks.Peek();
                    topDialog.Closed += OnTopDialogClosed;
                    _ = topDialog.ShowAsync();
                }
            }
        }

        /// <summary>
        /// 包装一个ContentDialog。
        /// </summary>
        /// <param name="dialog"></param>
        static void WrapContentDialog(ContentDialog dialog)
        {
            if(dialog.Content is not UIElement)
            {
                dialog.Content = new TextBlock() {
                    Text = dialog.Content.ToString(), 
                    Tag = "Wrapped Element",//设置Tag标记这是被包装的文本，解包时使用
                    TextWrapping = TextWrapping.Wrap, //使超长的文本换行。
                }; 
            }

            dialog.Content = BuildGridContainer((UIElement)dialog.Content);
        }

        /// <summary>
        /// 解包一个ContentDialog。
        /// </summary>
        /// <param name="dialog"></param>
        static void UnwrapContentDialog(ContentDialog dialog)
        {
            if (dialog.Content is not Grid grid) return; 
            if (grid.RowDefinitions.Count != 2) return; //预期之外，直接返回。
            object content = grid.Children.FirstOrDefault(ele => (int)ele.GetValue(Grid.RowProperty) == 0);
            if(content is TextBlock block && block.Tag.ToString() == "Wrapped Element")
            {
                //该TextBlock是被包装的文本，进行解包还原
                content = block.Text; 
            }
            dialog.Content = content;
        }

        /// <summary>
        /// 为指定元素创建一个Grid容器，并将该元素放在Grid的首行，剩余信息条数放在Grid的第二行。
        /// </summary>
        /// <param name="baseElement">要包装的元素。</param>
        /// <returns>创建的Grid容器。</returns>
        static Grid BuildGridContainer(UIElement baseElement)
        {
            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) }); // RowDefinition Height = Auto
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto }); // RowDefinition Height = Auto

            //定义文本
            TextBlock textBlock = new TextBlock()
            {
                Text = $"还有 {dialogStacks.Count} 条信息。",
                Foreground = new SolidColorBrush(Color.FromArgb(0xCC, 0, 0xFF, 0xFF)),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                FontSize = 14,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(5),
            };
            textBlock.SetValue(Grid.RowProperty, 1);

            //定义分割线
            //Border border = new Border()
            //{
            //    BorderBrush = ((Color)App.Current.Resources["SystemBaseLowColor"]).ToSolidBrush(),
            //    BorderThickness = new Thickness(1),
            //    Height = 1,
            //    Margin = new Thickness(15, 30, 15, 0),
            //    VerticalAlignment = VerticalAlignment.Bottom, //保证这条线出现在最下方
            //    HorizontalAlignment = HorizontalAlignment.Stretch
            //};
            if (dialogStacks.Count > 0)
                grid.Children.Add(textBlock);
            grid.Children.Add(baseElement);
            //grid.Children.Add(border);
            return grid;
        }
    }

    //class DialogRootPair(ContentDialog dialog, XamlRoot xamlRoot)
    //{
    //    public ContentDialog Dialog => dialog;
    //    public XamlRoot XamlRoot => xamlRoot;
    //    public override bool Equals(object obj)
    //    {
    //        if (obj is DialogRootPair pair)
    //        {
    //            return pair.Dialog == dialog;
    //        }
    //        return false;
    //    }

    //    public override int GetHashCode()
    //    {
    //        return dialog.GetHashCode();
    //    }
    //}
}
