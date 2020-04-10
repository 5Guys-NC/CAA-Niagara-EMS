/*************************
 * Created By: Jon Yade
 * **********************/
namespace CAA_Event_Management.ViewModels
{
    /// <summary>
    /// ViewModel for Survey Questions
    /// </summary>
    internal class SurveyQuestion
    {
        internal int ID { get; set; }
        internal string questPhrase { get; set; } = "";
        internal string questDataType { get; set; } = "";
    }
}
