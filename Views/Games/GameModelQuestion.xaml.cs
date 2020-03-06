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
    public sealed partial class GameModelQuestion : Page
    {
        IQuestionRepository questRepo;
        IGameRepository gameRepo;
        Game selected = new Game();
        List<GameModel> view = new List<GameModel>();
        public GameModelQuestion()
        {
            this.InitializeComponent();
            questRepo = new QuestionRepository();
            gameRepo = new GameRepository();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            selected = (Game)e.Parameter;

            PopulateModelQuestList();
        }

        public void PopulateModelQuestList()
        {
            view = questRepo.GetModelQuestions(selected.ID);
            ModelQuestList.ItemsSource = view;

            var list = new List<Question>();
            list = questRepo.GetQuestionSelection();
            QuestionSelection.ItemsSource = list;
        }


        private void ModelQuestList_ItemClick(object sender, ItemClickEventArgs e)
        {
            //Gets the selected question in game and dos to it's detail page for answers
            GameModel gm = (GameModel)e.ClickedItem;
            this.Frame.Navigate(typeof(GameModelAnswer), gm);
        }

        private void QuestionSelection_ItemClick(object sender, ItemClickEventArgs e)
        {
            //Adds question from selction list to game
            GameModel gm = new GameModel();

            gm.QuestionText = ((Question)e.ClickedItem).Text;
            gm.GameID = selected.ID;
            gameRepo.SaveGameModel(gm);
            PopulateModelQuestList();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            //Removes question from game
            GameModel remove = new GameModel();
            var index = Convert.ToInt32(((Button)sender).DataContext);
            remove = questRepo.GetModelQuestion(index);
            questRepo.RemoveGameModel(remove);
            PopulateModelQuestList();
        }

        private void btnViewQuestion_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(QuestionMenu));
        }

        private void btnViewGame_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(GameMenu));
        }

        private void btnViewAnswer_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AnswerMenu));
        }

    }
}
