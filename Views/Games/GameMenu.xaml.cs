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
/******************************
*  Model Created By: Max Cashmore
*  Edited by: Brian Culp
*  Edited By: Nathan Smith
*******************************/
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CAA_Event_Management.Views.Games
{
    /// <summary>
    /// Frame for Game Menu
    /// </summary>
    public sealed partial class GameMenu : Page
    {
        #region Startup - variables, repositories, methods

        IGameRepository gameRepo;
        IQuestionRepository questionRepo;
        IAnswerRepository answerRepo;
        Question view;

        public GameMenu()
        {
            this.InitializeComponent();
            questionRepo = new QuestionRepository();
            answerRepo = new AnswerRepository();
            view = new Question();
            gameRepo = new GameRepository();
            populateGameList();
        }

        #endregion

        #region Buttons - Add, Edit, Delete, Search
        private void BtnCreateConfirm_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Game newGame = new Game();
            newGame.Title = txtCreateNewGame.Text.ToUpper();
            gameRepo.AddGame(newGame);
            Frame.Navigate(typeof(GameDetails), (newGame));
        }

        private void GameList_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(GameDetails),  (Game)e.ClickedItem, new DrillInNavigationTransitionInfo());
        }

        private void BtnCreateCancel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            btnCreateNewGame.Flyout.Hide();
        }

        private void BtnCancel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Flyout FlyDelete = new Flyout();
            FlyDelete.Hide();
        }

        private void BtnConfirmRemove_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int selected = Convert.ToInt32(((Button)sender).DataContext);
            Game game = new Game();
            game = gameRepo.GetAGame(selected);
            gameRepo.DeleteGame(game);
            Frame.Navigate(typeof(GameMenu), null, new SuppressNavigationTransitionInfo());
        }

        private void txtSearch_TextChanged(object sender, RoutedEventArgs e)
        {
            List<Game> g = new List<Game>();
            
            gameList.ItemsSource = gameRepo.SearchGame(txtSearch.Text);
        }

        private void btnRecentGames_Click(object sender, RoutedEventArgs e)
        {
            Canvas.SetZIndex(btnRecentGames, 1);
            Canvas.SetZIndex(btnMostUsedGames, 0);
        }

        private void btnMostUsedGames_Click(object sender, RoutedEventArgs e)
        {
            Canvas.SetZIndex(btnMostUsedGames, 1);
            Canvas.SetZIndex(btnRecentGames, 1);
        }
        #endregion

        #region Helper Methods
        private void populateGameList()
        {
            try
            {
                List<Game> games = gameRepo.GetGames();
                //Bind to the ComboBox
                gameList.ItemsSource = games;
            }
            catch (Exception)
            {
                Jeeves.ShowMessage("Error", "Could not complete operation.");
            }
        }
        #endregion

    }
}

