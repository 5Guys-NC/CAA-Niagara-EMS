using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using CAA_Event_Management.ViewModels;
using System.Threading.Tasks;
using CAA_Event_Management.Views.EventViews;
using System.Threading;
using Windows.UI.Xaml.Media.Animation;
/******************************
*  Model Created By: Max Cashmore
*******************************/
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CAA_Event_Management.Views.Games
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GameResult : Page
    {
        ResultsViewModel resultVM = new ResultsViewModel();
        public GameResult()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            resultVM = (ResultsViewModel)e.Parameter;

            ShowResult();
        }

        public async void ShowResult()
        {
            decimal score = (Convert.ToDecimal(resultVM.CorrectAnswerCount) / Convert.ToDecimal(resultVM.QuestionCount)) * 100;

            //Depending on the score percentage, use gets feed back
            if (score < 50)
            { tbHeader.Text = "Too bad!"; }
            else if (score >= 50 && score < 80)
            { tbHeader.Text = "Not bad!"; }
            else if (score >= 80 && score < 90)
            { tbHeader.Text = "Good Job!"; }
            else if (score >= 90)
            { tbHeader.Text = "Great Job!"; }
            else if (score == 100)
            { tbHeader.Text = "Perfect!! "; }
            else
            { tbHeader.Text = ""; }

            //Shows the score result
            tbResult.Text = "You scored " + resultVM.CorrectAnswerCount + " out of " + resultVM.QuestionCount + "!";
            await Task.Delay(3000);
            Frame.Navigate(typeof(EventAttendanceTracking), resultVM.Event, new SuppressNavigationTransitionInfo());
        }
    }
}
