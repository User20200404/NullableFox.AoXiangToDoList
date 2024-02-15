using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using NullableFox.AoXiangToDoList.Utilities;
using Windows.ApplicationModel.DataTransfer;
using System.Text.Json;
using System.ComponentModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NullableFox.AoXiangToDoList.Views.UserControls
{
    public sealed partial class JsonPresenter : UserControl
    {
        private static readonly DependencyProperty ObjectProperty = DependencyProperty.Register(nameof(Object), typeof(object), typeof(JsonPresenter), new PropertyMetadata(null, OnTargetObjectPropertyChanged));
        private static readonly DependencyProperty MonitorPropertyChangeProperty = DependencyProperty.Register(nameof(MonitorPropertyChange), typeof(bool), typeof(JsonPresenter), new PropertyMetadata(true, OnMonitorPropertyChangePropertyChanged));


        private static JsonSerializerOptions jsonOptions;
        public object Object
        {
            get => GetValue(ObjectProperty);
            set => SetValue(ObjectProperty, value);
        }

        public bool MonitorPropertyChange
        {
            get => (bool)GetValue(MonitorPropertyChangeProperty);
            set => SetValue(MonitorPropertyChangeProperty, value);
        }

        static void OnTargetObjectPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            JsonPresenter presenter = (JsonPresenter)obj;
            presenter.UpdateDisplay();
            presenter.UnregisterMonitorEvent(args.OldValue); //为旧的对象注销属性变化监测事件
            presenter.RegisterMonitorEvent(args.NewValue); //为新的对象注册属性变化监测事件
        }

        static void OnMonitorPropertyChangePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            JsonPresenter presenter = (JsonPresenter)obj;
            //这里args.NewValue和args.OldValue的值分别为MonitorPropertyChange的新值和旧值。
            if(args.NewValue is true && args.OldValue is false)
            {
                presenter.RegisterMonitorEvent(presenter.Object);
            }
            if(args.NewValue is false && args.OldValue is true)
            {
                presenter.UnregisterMonitorEvent(presenter.Object);
            }
        }

        void RegisterMonitorEvent(object obj)
        {
            if (MonitorPropertyChange && obj is INotifyPropertyChanged observableObject)
            {
                observableObject.PropertyChanged += ObservableObject_PropertyChanged;
            }
        }

        private void ObservableObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateDisplay();
        }

        void UnregisterMonitorEvent(object obj)
        {
            if (!MonitorPropertyChange && obj is INotifyPropertyChanged observableObject)
            {
                observableObject.PropertyChanged -= ObservableObject_PropertyChanged;
            }
        }

        void UpdateDisplay()
        {
            jsonOptions ??= new JsonSerializerOptions(JsonHelper.DefaultIndentedJSONOptions);
            typeTxtBlk.Text = Object.GetType().Name;
            jsonTxtBox.Text = Object.ToJsonString(jsonOptions);
        }

        public JsonPresenter()
        {
            this.InitializeComponent();
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            var package = new DataPackage();
            package.SetText(jsonTxtBox.Text);
            Clipboard.SetContent(package);
        }
    }
}
