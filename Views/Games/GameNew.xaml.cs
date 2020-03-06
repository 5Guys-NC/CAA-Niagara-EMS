using System;
using System.Collections.Generic;
using System.ComponentModel;
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
/********************************
 * Created By: Max Cashmore
 * Edited By: Nathan Smith
 * Edited By: Brian Culp
 * ******************************/

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
namespace CAA_Event_Management.Views.Games
{
    /// <summary>
    /// Frame for New Games
    /// </summary>
    public sealed partial class GameNew : Page
    {
        #region Startup - variables, respositories, methods

        IQuestionRepository questionRepository;
        IGameRepository gameRepo;
        IAnswerRepository answerRepository;
        private int displayedQuestion = 6;
        CheckBox chkSelect3 = new CheckBox();
        CheckBox chkSelect4 = new CheckBox();
        CheckBox chkSelect5 = new CheckBox();
        CheckBox chkSelect6 = new CheckBox();
        Question view;

        public GameNew()
        {
            this.InitializeComponent();
            questionRepository = new QuestionRepository();
            gameRepo = new GameRepository();
            answerRepository = new AnswerRepository();
            view = new Question();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var gameTitle = (string)e.Parameter;
            txtGameTitle.Text = gameTitle;
        }
        #endregion

        #region Buttons - Add, Edit, Delete
        private void btnAddAnswer_Tapped(object sender, RoutedEventArgs e)
        {
            ShowNextQuestion();
            Grid.SetRow(rpButtons, displayedQuestion);
        }

        private void btnRemoveAnswer_Tapped(object sender, TappedRoutedEventArgs e)
        {
            HideNextQuestion();
            Grid.SetRow(rpButtons, displayedQuestion);

        }

        private void BtnAddAnswer_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Game newGame = new Game();
            gameRepo.AddGame(newGame);
            Frame.Navigate(typeof(GameNew), (newGame));
        }

        private void btnCreateConfirm_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }
        #endregion

        #region Helper Methods

        private void ShowNextQuestion()
        {
            if (displayedQuestion == 6)
            {
                AnswerThreeBlock.Visibility = Visibility.Visible;
                //txtAnswerThree.Visibility = Visibility.Visible;
                btnRemoveAnswer.Visibility = Visibility.Visible;
                rpButtons.Margin = new Thickness(350, 0, 0, 0);
                displayedQuestion += 2;

                chkSelect3.Name = "chkSelect3";
                chkSelect3.Content = "Correct Answer";
                chkSelect3.HorizontalAlignment = HorizontalAlignment.Center;
                chkSelect3.Margin = new Thickness(570, 0, 0, 0);
                Grid.SetRow(chkSelect3, displayedQuestion);
                GameGrid.Children.Add(chkSelect3);

            }
            else if (displayedQuestion == 8)
            {
                AnswerFourBlock.Visibility = Visibility.Visible;
                //txtAnswerFour.Visibility = Visibility.Visible;
                displayedQuestion += 2;

                chkSelect4.Name = "chkSelect4";
                chkSelect4.Content = "Correct Answer";
                chkSelect4.HorizontalAlignment = HorizontalAlignment.Center;
                chkSelect4.Margin = new Thickness(570, 0, 0, 0);
                Grid.SetRow(chkSelect4, displayedQuestion);
                GameGrid.Children.Add(chkSelect4);
            }
            else if (displayedQuestion == 10)
            {
                AnswerFiveBlock.Visibility = Visibility.Visible;
                //txtAnswerFive.Visibility = Visibility.Visible;
                displayedQuestion += 2;

                chkSelect5.Name = "chkSelect5";
                chkSelect5.Content = "Correct Answer";
                chkSelect5.HorizontalAlignment = HorizontalAlignment.Center;
                chkSelect5.Margin = new Thickness(570, 0, 0, 0);
                Grid.SetRow(chkSelect5, displayedQuestion);
                GameGrid.Children.Add(chkSelect5);
            }
            else if (displayedQuestion == 12)
            {
                AnswerSixBlock.Visibility = Visibility.Visible;
                //txtAnswerSix.Visibility = Visibility.Visible;
                btnAddAnswer.Visibility = Visibility.Collapsed;
                rpButtons.Margin = new Thickness(350, 0, 0, 0);
                displayedQuestion += 2;

                chkSelect6.Name = "chkSelect6";
                chkSelect6.Content = "Correct Answer";
                chkSelect6.HorizontalAlignment = HorizontalAlignment.Center;
                chkSelect6.Margin = new Thickness(570, 0, 0, 0);
                Grid.SetRow(chkSelect6, displayedQuestion);
                GameGrid.Children.Add(chkSelect6);
            }
            if (displayedQuestion > 14)
                displayedQuestion = 14;
        }

        private void HideNextQuestion()
        {
            if (displayedQuestion == 14)
            {
                AnswerSixBlock.Visibility = Visibility.Collapsed;
                //txtAnswerSix.Visibility = Visibility.Collapsed;
                btnAddAnswer.Visibility = Visibility.Visible;
                rpButtons.Margin = new Thickness(350, 0, 0, 0);
                displayedQuestion -= 2;

                if (GameGrid.Children.Contains(chkSelect6))
                    GameGrid.Children.Remove(chkSelect6);
                
            }
            else if (displayedQuestion == 12)
            {
                AnswerFiveBlock.Visibility = Visibility.Collapsed;
                //txtAnswerFive.Visibility = Visibility.Collapsed;
                displayedQuestion -= 2;

                if (GameGrid.Children.Contains(chkSelect5))
                    GameGrid.Children.Remove(chkSelect5);

            }
            else if (displayedQuestion == 10)
            {
                AnswerFourBlock.Visibility = Visibility.Collapsed;
                //txtAnswerFour.Visibility = Visibility.Collapsed;
                displayedQuestion -= 2;

                if (GameGrid.Children.Contains(chkSelect4))
                    GameGrid.Children.Remove(chkSelect4);

            }
            else if (displayedQuestion == 8)
            {
                AnswerThreeBlock.Visibility = Visibility.Collapsed;
                //txtAnswerThree.Visibility = Visibility.Collapsed;
                btnRemoveAnswer.Visibility = Visibility.Collapsed;
                rpButtons.Margin = new Thickness(310, 0, 0, 0);
                displayedQuestion -= 2;

                if (GameGrid.Children.Contains(chkSelect3))
                    GameGrid.Children.Remove(chkSelect3);
            }
            if (displayedQuestion < 6)
                displayedQuestion = 6;
        }
        #endregion
    }
}
