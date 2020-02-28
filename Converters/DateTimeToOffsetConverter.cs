using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using CAA_Event_Management.Converters;
/******************************
*  Model Created By: Nathan Smith
*  Edited by:
*******************************/
namespace CAA_Event_Management.Converters
{
    /// <summary>
    /// Model to convert DateTime values to DatTimeOffset
    /// </summary>
    public class DateTimeToOffsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                if (value == null) return null;
                DateTime date = (DateTime)value;
                return new DateTimeOffset(date);
            }
            catch (Exception)
            {
                return DateTimeOffset.MinValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            try
            {
                if (value == null) return null;
                DateTimeOffset dto = (DateTimeOffset)value;
                return dto.DateTime;
            }
            catch (Exception)
            {
                return DateTime.MinValue;
            }
        }
    }
}
