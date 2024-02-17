using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using NullableFox.AoXiangToDoList.Services;
using NullableFox.AoXiangToDoList.Services.Interfaces;
using NullableFox.AoXiangToDoList.ViewModels;
using NullableFox.AoXiangToDoList.Views.Windows;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NullableFox.AoXiangToDoList
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        public DispatcherQueue MainDispatcherQueue { get;  set; }

        /// <summary>
        /// Gets the current <see cref="App"/> instance in use
        /// </summary>
        public new static App Current => (App)Application.Current;
        public IServiceProvider ServiceProvider { get; init; }
        internal ApplicationViewModel ApplicationViewModel => ServiceProvider.GetService<ApplicationViewModel>();

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            ServiceProvider = ConfigureServices();
        }

        public IServiceProvider ConfigureServices()
        {
            int socketBackEndport = 20221; //定义后端的网络端口，这里可能需要放到配置服务中去。
            //int httpBackEndPort = 20220; //定义后端的网络端口，这里可能需要放到配置服务中去。

            var services = new ServiceCollection();
            services.AddSingleton<INetworkService,SocketService>(provider => new SocketService(socketBackEndport));
            services.AddSingleton<INotificationService, NotificationService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IToDoWorkItemService, ToDoWorkItemService>();
            services.AddSingleton<IPomodoroRecordService, PomodoroRecordService>();
            services.AddSingleton<IPomodoroService,PomodoroService>();
            services.AddSingleton<IApplicationService, ApplicationService>();
            services.AddSingleton<IConfigurationService, ConfigurationService>();
            services.AddKeyedSingleton<INavigationService, NavigationService>(NavigationServiceKeys.Root);
            services.AddKeyedSingleton<INavigationService, NavigationService>(NavigationServiceKeys.ToDoWork);

            services.AddSingleton<ToDoCollectionViewModel>();
            services.AddTransient<ToDoWorkItemViewModel>();
            services.AddSingleton<PomodoroViewModel>();
            services.AddTransient<PomodoroRecordViewModel>();
            services.AddTransient<PomodoroRecordCollectionViewModel>();
            services.AddSingleton<ApplicationViewModel>();
            services.AddSingleton<AppConfigurationViewModel>();
            services.AddSingleton<MainWindow>();
            services.AddSingleton<FocusViewWindow>();

            return services.BuildServiceProvider();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = ServiceProvider.GetRequiredService<MainWindow>();
            m_window.Activate();
        }

        private Window m_window;
    }
}
