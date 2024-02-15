using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace NullableFox.AoXiangToDoList.ViewModels
{

    public class BooleanInversionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool b) return !b;
            else return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class BasicCalculateConverter : IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, string language)
        {
            string para = parameter as string;
            char op = para[0];
            double rightVal = double.Parse(para[1..]);
            double leftVal = System.Convert.ToDouble(value);
            return op switch
            {
                '+' => leftVal + rightVal,
                '-' => leftVal - rightVal,
                '*' => leftVal * rightVal,
                '/' => leftVal / rightVal,
                '%' => leftVal % rightVal,
                _ => throw new InvalidDataException($"{nameof(BasicCalculateConverter)}: Invalid Number Or Operator.")
            };
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return Convert(value, targetType, parameter, language);
        }
    }

    /// <summary>
    /// 提供颜色插值转换器，输入的格式必须为 (A1,R1,G1,B1),(A2,R2,G2,B2),S,E
    /// 也可以使用ThemeResources资源名称替代括号内的ARGB，此时转换器会从App的资源中搜索该名称。
    /// 其中前一个括号中为起始颜色，第二个括号为终止颜色。S为起始值，E为终止值。
    /// value传入值，应当介于S和E之间。
    /// A可以省略，此时按照255计算。
    /// 本转换器根据targetType自动将颜色转换为Windows.UI.Color或SolidColorBrush.
    /// </summary>
    public class ColorInterpolationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (parameter is not string str) throw new ArgumentException($"value必须为string，当前为{parameter.GetType()}", nameof(parameter));
            string[] ss = str.Replace(" ", "").Split("),");
            if (ss.Length != 3) throw new ArgumentException($"value必须有三个参数，当前有 {ss.Length} 个参数", nameof(parameter));
            string[] sargb1 = ss[0][1..].Split(',');
            string[] sargb2 = ss[1][1..].Split(',');
            string[] sse = ss[2].Split(',');

            byte a1, r1, g1, b1, a2, r2, g2, b2;
            float p, s, e;

            (a1, r1, g1, b1) = sargb1.Length switch
            {
                1 => GetColorResource(sargb1[0]),
                3 => (byte.MaxValue, byte.Parse(sargb1[0]), byte.Parse(sargb1[1]), byte.Parse(sargb1[2])),
                4 => (byte.Parse(sargb1[0]), byte.Parse(sargb1[1]), byte.Parse(sargb1[2]), byte.Parse(sargb1[3])),
                _ => throw new ArgumentException($"颜色参数必须能够被分为1个、3个或4个参数，而当前起始颜色参数个数为 {sargb1.Length}")
            };

            (a2, r2, g2, b2) = sargb2.Length switch
            {
                1 => GetColorResource(sargb2[0]),
                3 => (byte.MaxValue, byte.Parse(sargb2[0]), byte.Parse(sargb2[1]), byte.Parse(sargb2[2])),
                4 => (byte.Parse(sargb2[0]), byte.Parse(sargb2[1]), byte.Parse(sargb2[2]), byte.Parse(sargb2[3])),
                _ => throw new ArgumentException($"颜色参数必须能够被分为1个、3个或4个参数，而终止颜色参数个数为 {sargb2.Length}")
            };
            s = float.Parse(sse[0]);
            e = float.Parse(sse[1]);
            p = (float.Parse(value.ToString()) - s) / (e - s);
            var argb = Algorithm.Interpolation.ArgbBetween((a1, r1, g1, b1), (a2, r2, g2, b2), p);
            var color = Color.FromArgb(argb.Item1, argb.Item2, argb.Item3, argb.Item4);
            if (targetType.IsAssignableTo(typeof(Brush))) return new SolidColorBrush(color);
            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        (byte, byte, byte, byte) GetColorResource(string resourceName)
        {
            var res = App.Current.Resources[resourceName];
            if (res is Color color)
                return (color.A, color.R, color.G, color.B);
            else return (255, 127, 127, 127); //如果不存在资源，返回中间颜色。
        }
    }

    /// <summary>
    /// 提供bool到Visibility的转换。
    /// 本转换器将true映射到Visibility.Visible，false映射到Visibility.Collapsed。
    /// 如果<para>parameter</para>为inverse，则将true映射到Visibility.Collapsed，false映射到Visibility.Visible。
    /// 在反向转换时，按照上述映射进行反向映射。
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool b = (bool)value;
            if (parameter is not null && parameter.ToString().ToLower() == "inverse")
                b = !b;

            return b ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            Visibility v = (Visibility)value;
            if (parameter is not null && parameter.ToString().ToLower() == "inverse")
                return v != Visibility.Visible;
            else return v == Visibility.Visible;
        }
    }

    /// <summary>
    /// 提供Boolean到Opacity的映射。当value为true时，返回1d，否则返回0d。
    /// parameter为inverse时，提供相反的映射。
    /// </summary>
    public class BooleanToOpacityVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool b = (bool)value;
            if (parameter is not null && parameter.ToString().ToLower() == "inverse")
                b = !b;
            
            return b ? 1d : 0d;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            double opacity = (double)value;
            if (parameter is not null && parameter.ToString().ToLower() == "inverse")
                return opacity == 0d;
            else return opacity > 0d;
        }
    }
}
