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
            Frame.Navigate(typeof(GameMenu));
        }

        private void btnQuestion_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(QuestionMenu));
        }

        private void btnAnswer_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AnswerMenu));
        }

        private void BtnCreateConfirm_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Game newGame = new Game();

            newGame.Title = txtCreateNewGame.Text;
            gameRepo.AddGame(newGame);
            Frame.Navigate(typeof(GameDetails), (newGame));
        }

        private void BtnCreateCancel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            btnCreateGame.Flyout.Hide();
        }

        private void BtnConfirmRemove_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void BtnCancel_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }
    }
}