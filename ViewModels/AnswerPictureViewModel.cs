using Windows.UI.Xaml.Media;
/******************************
*  Model Created By: Max Cashmore
*******************************/
namespace CAA_Event_Management.ViewModels
{
    /// <summary>
    /// Object used to display the answer's text and image in a view list
    /// </summary>
    public class AnswerPictureViewModel
    {
        public int? ID { get; set; }
        public ImageSource Image { get; set; }
        public string Text { get; set; }
    }
}
