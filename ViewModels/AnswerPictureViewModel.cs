using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace CAA_Event_Management.ViewModels
{
    /// <summary>
    /// Object used to display the answer's text and image in a view list
    /// </summary>
    public class AnswerPictureViewModel
    {
        public ImageSource Image { get; set; }
        public string Text { get; set; }
    }
}
