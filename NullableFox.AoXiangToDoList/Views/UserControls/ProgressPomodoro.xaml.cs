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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NullableFox.AoXiangToDoList.Views.UserControls
{
    public sealed partial class ProgressPomodoro : UserControl
    {
        private static readonly DependencyProperty ProgressProperty = DependencyProperty.Register(nameof(Progress), typeof(double), typeof(ProgressPomodoro), new PropertyMetadata(0, OnProgressPropertyChanged));

        static void OnProgressPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ProgressPomodoro that = (ProgressPomodoro)obj;
            that.UpdateClip();
        }
        public double Progress
        {
            get => Convert.ToDouble(GetValue(ProgressProperty));
            set => SetValue(ProgressProperty, value);
        }


        void UpdateClip()
        {
            double y = (1-Progress) * this.ActualHeight;
            clipRect.Transform = new TranslateTransform() { X = 0, Y = y };
        }
        public ProgressPomodoro()
        {
            this.InitializeComponent();
            UpdateClip();
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateClip();
        }
    }
}
