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
using CAA_Event_Management.Utilities;
using CAA_Event_Management.ViewModels;
using CAA_Event_Management.Data.Interface_Repos;
using CAA_Event_Management.Data.Repos;
using CAA_Event_Management.Views.EventViews;
using System.Threading.Tasks;
using CAA_Event_Management.Converters;
using Windows.UI.Xaml.Media.Imaging;
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
    public sealed partial class PlayerGameView : Page
    {
        Event thisEvent = new Event();
        List<GameModel> gameQuest = new List<GameModel>();
        int index = 0; //used to go through the list of questions
        int questionCount;
        IQuestionRepository questRepo;
        IPictureRepository picRepo;
        ImageConverter imageConverter = new ImageConverter();
        public List<QuestAnsViewModel> display = new List<QuestAnsViewModel>();
        ResultsViewModel resultVM = new ResultsViewModel();

        public PlayerGameView()
        {
            this.InitializeComponent();
            questRepo = new QuestionRepository();
            picRepo = new PictureRepository();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            thisEvent = (Event)e.Parameter;
            SetUpGame();
            DisplayQuestion();
        }

        public void SetUpGame()
        {
            gameQuest = questRepo.GetModelQuestions(Convert.ToInt32(thisEvent.QuizID));
            questionCount = gameQuest.Count;
            resultVM.QuestionCount = gameQuest.Count;
            resultVM.Event = thisEvent;
        }

        public void IncrementQuestion()
        {
            //increases index to move onto next question
            index++;
            DisplayQuestion();
        }

        public void DisplayQuestion()
        {
            //If the question has an image with it, displays it
            if (gameQuest[index].QuestionImageId != null && gameQuest[index].QuestionImageId != "0" && gameQuest[index].QuestionImageId != "")
            {
                imageQuest.Source = null;
                var p = picRepo.GetPicture(Convert.ToInt32(gameQuest[index].QuestionImageId));
                var image = imageConverter.ByteToImage(p.Image);
                imageQuest.Source = image;
            }

            tbkQuestion.Text = gameQuest[index].QuestionText;

            display.Clear();
            var options = gameQuest[index].OptionsText.Split('|');
            var images = gameQuest[index].ImageIDs.Split('|');
            var possibleAnswers = gameQuest[index].AnswerText.Split('|');

            //Loop through possible answers
            for (int i = 0; i < options.Length; i++)
            {
                //place them in holder and set variables 
                QuestAnsViewModel t = new QuestAnsViewModel();
                t.Text = options[i];
                
                var imageElementExists = images.ElementAtOrDefault(i) != null;
                var optionElementExists = options.ElementAtOrDefault(i) != null;
                t.IsTrue = false;

                if (imageElementExists && images[i] != "" && images[i] != "0")
                {
                    var p = picRepo.GetPicture(Convert.ToInt32(images[i]));
                    t.Image = imageConverter.ByteToImage(p.Image);
                    if (possibleAnswers.Contains(images[i])) t.IsTrue = true;
                }

                //if the correct answer is either an image or a text, sets the item to true
                if (optionElementExists && possibleAnswers.Contains(options[i]))
                    t.IsTrue = true;

                display.Add(t);
            }
            gameplayView.ItemsSource = display;
        }

        public async void NextQuestion()
        {
            //pauses for 3 seconds then moves on
            await Task.Delay(3000);
            if (index + 1 == questionCount)
            {
                //If question answered was the last one, goes to the result page
                Frame.Navigate(typeof(GameResult), resultVM, new SuppressNavigationTransitionInfo());
            }
            else
            {
                //Clears view then moves on to next question.
                gameplayView.ItemsSource = null;
                txtDisplayResult.Text = "";
                IncrementQuestion();
            }
        }

        private void btnCancelGame_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(EventAttendanceTracking), thisEvent, new SuppressNavigationTransitionInfo());
        }

        private void gameplayView_ItemClick(object sender, ItemClickEventArgs e)
        {
            //Gets grid item selected and casts as a QuestAnsViewModel
            var selected = e.ClickedItem as QuestAnsViewModel;

            if (selected.IsTrue)
            {
                txtDisplayResult.Text = "Correct!";
                resultVM.CorrectAnswerCount++;
            }
            else
            {
                txtDisplayResult.Text = "Incorrect!";
            }
            NextQuestion();
        }
    }
}