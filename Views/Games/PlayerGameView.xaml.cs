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
using CAA_Event_Management.Data.Interface_Repos;
using CAA_Event_Management.Data.Repos;
using CAA_Event_Management.Views.EventViews;

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
        int index = 0;
        int questionCount;
        IQuestionRepository questRepo;
        public List<QuestAnsViewModel> display = new List<QuestAnsViewModel>();

        public PlayerGameView()
        {
            this.InitializeComponent();
            questRepo = new QuestionRepository();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            thisEvent = (Event)e.Parameter;            
            gameQuest = questRepo.GetModelQuestions(Convert.ToInt32(thisEvent.QuizID));
            questionCount = gameQuest.Count;
            DisplayQuestion();
        }

        public void IncrementQuestion()
        {
            index++;
            DisplayQuestion();
        }

        public void DisplayQuestion()
        {
            tbkQuestion.Text = gameQuest[index].QuestionText;
            display.Clear();

            var options = gameQuest[index].OptionsText.Split('|');
            //var possibleAnswers = gameQuest[index].AnswerText.Split('|');

            //Loop through possible answers
            for (int i = 0; i < options.Length; i++)
            {
                //place them in holder and set variables 
                QuestAnsViewModel t = new QuestAnsViewModel();
                t.Text = options[i];
                display.Add(t);
            }

            gameplayView.ItemsSource = display;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var select = ((Button)sender);

            if(gameQuest[index].AnswerText.Contains(select.Content.ToString()))
            {
                select.Background = new SolidColorBrush(Windows.UI.Colors.Green);
                txtDisplayResult.Text = "Correct!";
            }

            else
            {
                select.Background = new SolidColorBrush(Windows.UI.Colors.Red);

            }

            btnResultNext.Visibility =  Visibility.Visible;
        }

        private void btnResultNext_Click(object sender, RoutedEventArgs e)
        {

            if (index + 1 == questionCount)
            {
                Frame.Navigate(typeof(GameResult));
            }
            else
            {
                gameplayView.ItemsSource = null;
                txtDisplayResult.Text = "";
                IncrementQuestion();
                
            }
        }
    }
}
