using System;
using Windows.UI.Xaml.Data;
/******************************
*  Model Created By: Nathan Smith
*    >>> with code from David Stovell
*******************************/

namespace CAA_Event_Management.Converters
{
    /// <summary>
    /// Model to convert DateTime values to DatTimeOffset
    /// </summary>
    public class DateTimeToOffsetConverter : IValueConverter
    {
        /// <summary>
        /// Converts DateTime to DateTimeOffset
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns>DateTimeOffset</returns>
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

        /// <summary>
        /// Convert DateTimeOffset to DateTime
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns>DatTime</returns>
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
