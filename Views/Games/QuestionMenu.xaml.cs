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
using CAA_Event_Management.Models;
using CAA_Event_Management.Data;
using Windows.UI.Xaml.Media.Animation;
using System.Threading.Tasks;
/******************************
*  Model Created By: Max Cashmore
*******************************/

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CAA_Event_Management.Views.Games
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class QuestionMenu : Page
    {
        IQuestionRepository questRepo;
        IGameRepository gameRepo;

        public QuestionMenu()
        {
            this.InitializeComponent();
            questRepo = new QuestionRepository();
            gameRepo = new GameRepository();
            PopulateGameList();

            ((Window.Current.Content as Frame).Content as MainPage).ChangeMainPageTitleName("GAME MENU");
        }

        public void PopulateGameList()
        {
            List<Question> quests = questRepo.GetQuestions();
            GameList.ItemsSource = quests;
        }

        private void btnAddQuestion_Click(object sender, RoutedEventArgs e)
        {
            //Question add = new Question();
            //add.Text = txtQuestionAdd.Text;
            //questRepo.AddQuestion(add);
            //PopulateGameList();
        }
        //Navigation
        //Navigation
        private void btnGames_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GameMenu), null, new SuppressNavigationTransitionInfo());
        }

        private void btnQuestion_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(QuestionMenu), null, new SuppressNavigationTransitionInfo());
        }

        private void btnAnswer_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AnswerMenu), null, new SuppressNavigationTransitionInfo());
        }

        private void BtnCreateConfirm_Tapped(object sender, TappedRoutedEventArgs e)
        {
           
            Question newQuest = new Question();

            newQuest.Text = txtCreateNewGame.Text;

            questRepo.AddQuestion(newQuest);
            Frame.Navigate(typeof(QuestionMenu),new SuppressNavigationTransitionInfo());
        }

        private void BtnCreateCancel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            btnCreateGame.Flyout.Hide();
        }

        private void BtnConfirmRemove_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int selected = Convert.ToInt32(((Button)sender).DataContext);
            Question question = new Question();
            question = questRepo.GetQuestion(selected);
            questRepo.RemoveQuestion(question);
            Frame.Navigate(typeof(GameMenu), null, new SuppressNavigationTransitionInfo());
        }

        private async void btnRemove_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var result = await Jeeves.ConfirmDialog("Warning", "Are you sure you want to delete?");

            if (result == ContentDialogResult.Secondary) //&& btnEditSurvey.Content.ToString() == "#xE74D;"
            {
                int selected = Convert.ToInt32(((Button)sender).DataContext);
                Question question = new Question();
                question = questRepo.GetQuestion(selected);
                questRepo.RemoveQuestion(question);
                Frame.Navigate(typeof(QuestionMenu), null, new SuppressNavigationTransitionInfo());
            }
        }
    }
}