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
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media.Animation;
/******************************
*  Model Created By: Max Cashmore
*  Edited by: Brian Culp
*******************************/

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
namespace CAA_Event_Management.Views.Games
{
    /// <summary>
    /// Frame for Question Details
    /// </summary>
    public sealed partial class QuestionDetail : Page
    {
        #region Startup - variables, respositories, methods

        Question view;
        List<Answer> answers;
        IGameRepository gameRepo;
        IAnswerRepository answerRepo;
        IQuestionRepository questionRepo;

        public QuestionDetail()
        {
            this.InitializeComponent();
            answerRepo = new AnswerRepository();
            questionRepo = new QuestionRepository();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            view = (Question)e.Parameter;
            this.DataContext = view;

            answers = answerRepo.GetAnswersByQuestion(view.ID);
            AnswerList.ItemsSource = answers;
        }
        #endregion

        #region Buttons - Add, Edit, Delete
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private void btnSave_Tapped(object sender, TappedRoutedEventArgs e)
        {
            questionRepo.UpdateQuestion(view);
            answerRepo.UpdateAnswers(answers);

            gameRepo = new GameRepository();
            Game game = new Game();
            game = gameRepo.GetAGame(view.GameID);
            Frame.Navigate(typeof(GameDetails), (game));
        }

        private void btnAdd_Tapped(object sender, RoutedEventArgs e)
        {
            answerRepo.UpdateAnswers(answers);
            Answer newAnswer = new Answer();
            newAnswer.QuestionID = view.ID;
            answerRepo.AddAnswer(newAnswer);
            Frame.Navigate(typeof(QuestionDetail), (view), new SuppressNavigationTransitionInfo());
            Frame.BackStack.Remove(this.Frame.BackStack.Last());
        }

        private void btnRemove_Tapped(object sender, TappedRoutedEventArgs e)
        {
            answerRepo.UpdateAnswers(answers);
            int selected = Convert.ToInt32(((Button)sender).DataContext);
            Answer answer = new Answer();
            answer = answerRepo.GetAnswer(selected);
            answerRepo.DeleteAnswer(answer);
            Frame.Navigate(typeof(QuestionDetail), (view), new SuppressNavigationTransitionInfo());
            Frame.BackStack.Remove(this.Frame.BackStack.Last());        }

        private void btnDeleteQuestion_Tapped(object sender, RoutedEventArgs e)
        {

        }
        #endregion
    }
}
