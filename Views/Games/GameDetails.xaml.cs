using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using CAA_Event_Management.Data;
using CAA_Event_Management.Models;
using CAA_Event_Management.Utilities;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using CAA_Event_Management.Views.Games;
using Windows.UI.Xaml.Media.Animation;
/****************************
* Created By: Max Cashmore
* Edited By: Nathan Smith
* Edited By: Brian Culp
* **************************/

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
namespace CAA_Event_Management.Views.Games
{
    /// <summary>
    /// Frame for Game Details
    /// </summary>
    public sealed partial class GameDetails : Page
    {
        #region Startup - variables, repositories, methods

        Game view;
        List<Question> questions;
        IGameRepository gameRepo;
        IQuestionRepository questionRepo;
        IAnswerRepository answerRepo;

        public GameDetails()
        {
            this.InitializeComponent();
            answerRepo = new AnswerRepository();
            gameRepo = new GameRepository();
            questionRepo = new QuestionRepository();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            view = (Game)e.Parameter;
            this.DataContext = view;
            //questions = questionRepo.GetQuestionsByGame(view.ID); 
            questionList.ItemsSource = questions; 
        }
        #endregion

        #region Buttons - Add, Edit, Delete
        private void questionList_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(QuestionDetail), (Question)e.ClickedItem, new DrillInNavigationTransitionInfo());
        }

        private void btnBack_Tapped(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void btnAddQuestion_Tapped(object sender, RoutedEventArgs e)
        {
            Question newQuestion = new Question();
            //newQuestion.GameID = view.ID;
            questionRepo.AddQuestion(newQuestion);
            Frame.Navigate(typeof(QuestionDetail), (newQuestion));
        }

        private void btnSave_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //gameRepo.UpdateGame(view);
            Frame.Navigate(typeof(GameMenu), (view));
        }

        private void btnDelete_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Game game = new Game();
            game = view;
            //gameRepo.DeleteGame(game);
            Frame.Navigate(typeof(GameMenu), null, new SuppressNavigationTransitionInfo());
        }

        private void btnRemove_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            //questionList.ItemsSource = questionRepo.SearchQuestion(txtSearch.Text);
        }

        #endregion

        private void BtnCancel_Tapped(object asender, TappedRoutedEventArgs e)
        {
            return;
            //TODO: Find fix!
        }

        private void BtnConfirmRemove_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int selected = Convert.ToInt32(((Button)sender).DataContext);
            Question question = new Question();
            //question = questionRepo.GetQuestion(selected);
            //questionRepo.DeleteQuestion(question);
            Frame.Navigate(typeof(GameDetails), (view), new SuppressNavigationTransitionInfo());
        }
    }
}
