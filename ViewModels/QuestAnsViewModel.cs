using Windows.UI.Xaml.Media.Imaging;
/******************************
*  Model Created By: Max Cashmore
*******************************/
namespace CAA_Event_Management.ViewModels
{
    /// <summary>
    /// Object used to store an answer's text, image, and if it's true in editing
    /// </summary>
    public class QuestAnsViewModel
    {
        public QuestAnsViewModel()
        {
            this.IsTrue = false;
            this.ImageID = "0";
        }

        public string Text { get; set; }
        public bool IsTrue { get; set; }
        public int Index { get; set; }
        public BitmapImage Image { get; set; }
        public string ImageID { get; set; }
    }
}
