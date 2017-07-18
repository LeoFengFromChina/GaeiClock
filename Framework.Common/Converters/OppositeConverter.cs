using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Framework.Common.Converters
{
    /// <summary>
    /// 取相反值的转换器
    /// </summary>
    public class OppositeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool state = System.Convert.ToBoolean(value);
            return !state;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool state = System.Convert.ToBoolean(value);
            return !state;
        }
    }
}
