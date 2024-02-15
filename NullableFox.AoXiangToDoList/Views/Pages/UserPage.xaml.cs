using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using NullableFox.AoXiangToDoList.Services;
using NullableFox.AoXiangToDoList.Services.Interfaces;
using NullableFox.AoXiangToDoList.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NullableFox.AoXiangToDoList.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserPage : Page
    {
        public static Type Type => typeof(UserPage);
        UserViewModel userViewModel = new UserViewModel(App.Current.ServiceProvider.GetRequiredService<IUserService>());
        public UserPage()
        {
            this.InitializeComponent();

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await this.userViewModel.UpdateAsync();
            }
            catch (Exception ex)
            {
                ex.PushExceptionDialog(this.XamlRoot);
            }
        }

        private async void Button2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await this.userViewModel.ModifyUserInfoAsync();
            }catch(Exception ex)
            {
                ex.PushExceptionDialog(this.XamlRoot);
            }
        }
    }
}
