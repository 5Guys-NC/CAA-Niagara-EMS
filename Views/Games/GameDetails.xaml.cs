using CAA_Event_Management.Data;
using CAA_Event_Management.Models;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
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

        IQuestionRepository questRepo;
        IGameRepository gameRepo;
        IAnswerRepository answerRepo;
        Game selected = new Game();
        List<GameModel> view = new List<GameModel>();

        public GameDetails()
        {
            this.InitializeComponent();
            answerRepo = new AnswerRepository();
            gameRepo = new GameRepository();
            questRepo = new QuestionRepository();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            selected = (Game)e.Parameter;
            txtGameTitle.Text = selected.Title;
            PopulateModelQuestList();
        }
        #endregion

        public void PopulateModelQuestList()
        {
            view = questRepo.GetModelQuestions(selected.ID);
            questionList.ItemsSource = view;

            var list = new List<Question>();
            list = questRepo.GetQuestionSelection();
            QuestionSelection.ItemsSource = list;
        }


        #region Buttons - Add, Edit, Delete
        private void questionList_ItemClick(object sender, ItemClickEventArgs e)
        {
            GameModel gm = (GameModel)e.ClickedItem;
            this.Frame.Navigate(typeof(QuestionDetail), gm, new DrillInNavigationTransitionInfo());

        }

        private void btnBack_Tapped(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void btnAddQuestion_Tapped(object sender, RoutedEventArgs e)
        {
            //Question newQuestion = new Question();
            ////newQuestion.GameID = view.ID;
            //questRepo.AddQuestion(newQuestion);
            //Frame.Navigate(typeof(QuestionDetail), (newQuestion));
        }

        private void btnSave_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //gameRepo.UpdateGame(view);
            Frame.Navigate(typeof(GameMenu), (view), new SuppressNavigationTransitionInfo());
        }

        private void btnDelete_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //Removes question from game
            GameModel remove = new GameModel();
            var index = Convert.ToInt32(((Button)sender).DataContext);
            remove = questRepo.GetModelQuestion(index);
            questRepo.RemoveGameModel(remove);
            PopulateModelQuestList();
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
            //Removes question from game
            GameModel remove = new GameModel();
            var index = Convert.ToInt32(((Button)sender).DataContext);
            remove = questRepo.GetModelQuestion(index);
            questRepo.RemoveGameModel(remove);
            PopulateModelQuestList();
        }
        private void QuestionSelection_ItemClick(object sender, ItemClickEventArgs e)
        {
            GameModel gm = new GameModel();

            gm.QuestionText = ((Question)e.ClickedItem).Text;
            gm.GameID = selected.ID;
            gameRepo.SaveGameModel(gm);
            PopulateModelQuestList();
        }

        private void btnCreateConfirm_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GameModel gm = new GameModel();

            gm.QuestionText = txtAddNewQuestion.Text;
            gm.GameID = selected.ID;
            gameRepo.SaveGameModel(gm);
            //btnAddQuestion.Flyout.Hide();
            //PopulateModelQuestList();
            this.Frame.Navigate(typeof(QuestionDetail), gm, new SuppressNavigationTransitionInfo());
        }

        private void btnCreateCancel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            btnAddQuestion.Flyout.Hide();
        }
    }
}
