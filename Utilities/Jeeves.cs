using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
/******************************
*  Model Created By: Jon Yade
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


