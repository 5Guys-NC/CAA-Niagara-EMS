using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
/******************************
*  Created By: Jon Yade
*  using code by David Stovell
*  Edited by:
*******************************/
namespace CAA_Event_Management.Models
{
    /// <summary>
    /// Jeeves is the all encompassing Error Handling butler.
    /// He will serve error messages when the errors arrive.
    /// </summary>
    public static class Jeeves
    {
        /// <summary>
        /// Show Message in Popup Window
        /// </summary>
        /// <param name="strTitle"></param>
        /// <param name="Msg"></param>
        internal async static void ShowMessage(string strTitle, string Msg)
        {
            ContentDialog diag = new ContentDialog()
            {
                Title = strTitle,
                Content = Msg,
                PrimaryButtonText = "Ok"
            };
            await diag.ShowAsync();
        }

        /// <summary>
        /// Popup that allows for user to select Yes or No
        /// </summary>
        /// <param name="strTitle"></param>
        /// <param name="Msg"></param>
        /// <returns>result of click on Popup</returns>
        internal async static Task<ContentDialogResult> ConfirmDialog(string strTitle, string Msg)
        {
            ContentDialog diag = new ContentDialog()
            {
                Title = strTitle,
                Content = Msg,
                PrimaryButtonText = "No",
                SecondaryButtonText = "Yes"
            };
            ContentDialogResult result = await diag.ShowAsync();
            return result;
        }
    }
}


