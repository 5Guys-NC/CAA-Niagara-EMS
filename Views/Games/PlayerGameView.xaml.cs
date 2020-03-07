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
        #region Startup - variables, repositories, methods

        Models.Event thisEvent;
        List<Question> thisQuizQuestions = new List<Question>();
        List<Answer> answersForThisQuestion = new List<Answer>();
        int questionTotalCount = 0;
        int currentQuestionCount = 0;

        IEventGameUserAnswerRepository eventGameUserAnswerRepository;
        IEventRepository eventRepository;
        IGameRepository gameRepository;
        IQuestionRepository questionRepository;
        IAnswerRepository answerRepository;

        public PlayerGameView()
        {
            this.InitializeComponent();
            eventRepository = new EventRepository();
            gameRepository = new GameRepository();
            questionRepository = new QuestionRepository();
            answerRepository = new AnswerRepository();
            eventGameUserAnswerRepository = new EventGameUserAnswerRepository();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            thisEvent = (Models.Event)e.Parameter;
            BuildQuiz();
            StartQuiz();
        }

        #endregion

        private void btnNextQuestion_Click(object sender, RoutedEventArgs e)
        {
            var choosenAnswer = new Answer();

            if(gvDisplayAnswers.SelectedItem != null)
            {
                choosenAnswer = (Answer)gvDisplayAnswers.SelectedItem;

                //if (choosenAnswer.IsCorrect == true)
                //{
                //    Jeeves.ShowMessage("Correct", "Your answer was correct!");
                //}
                //else Jeeves.ShowMessage("Incorrect", "Your answer was wrong");

                //SaveQuestionAnswer((bool)choosenAnswer.IsCorrect);
                NextQuestion();
            }

        }

        private async void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            var result = await Jeeves.ConfirmDialog("Stop Game", "Do you wish to stop the game?");
            if (result == ContentDialogResult.Secondary)
            {
                Frame.Navigate(typeof(EventAttendanceTracking), (Models.Event)thisEvent);
            }
        }


        #region Helper methods - Quiz Methods

        private void BuildQuiz()
        {
            try
            {
                //thisQuizQuestions = questionRepository.GetQuestionsByGame((int)thisEvent.QuizID);
                questionTotalCount = thisQuizQuestions.Count;
            }
            catch
            {
                Jeeves.ShowMessage("Error", "Questions failed to load correctly");
            }
        }

        private void StartQuiz()
        {
            //tbkcurrentQuestion.Text = thisQuizQuestions[currentQuestionCount].Phrase.ToString();
            GetQuestionAnswers(thisQuizQuestions[currentQuestionCount].ID);

            currentQuestionCount++;

        }

        private void NextQuestion()
        {
            if (currentQuestionCount < questionTotalCount)
            {
                //tbkcurrentQuestion.Text = thisQuizQuestions[currentQuestionCount].Phrase.ToString();
                GetQuestionAnswers(thisQuizQuestions[currentQuestionCount].ID);

                currentQuestionCount++;
            }
            else
            {
                Frame.Navigate(typeof(EventAttendanceTracking), (Models.Event)thisEvent);
            }
        }

        private void GetQuestionAnswers(int QuestionID)
        {
            try
            {
                //answersForThisQuestion = answerRepository.GetAnswersByQuestion(QuestionID);
                gvDisplayAnswers.ItemsSource = answersForThisQuestion;
            }
            catch
            {
                Jeeves.ShowMessage("Error", "There was a problem retrieving the answers");
            }
        }

        private void SaveQuestionAnswer(bool isCorrect)
        {
            try
            {
                EventGameUserAnswer answer = new EventGameUserAnswer();
                answer.ID = Guid.NewGuid().ToString();
                answer.EventID = thisEvent.EventID;
                answer.QuestionID = thisQuizQuestions[currentQuestionCount-1].ID;
                answer.answerWasCorrect = isCorrect;
                eventGameUserAnswerRepository.AddEventGameUserAnswer(answer);
            }
            catch
            {

            }
        }

        #endregion
    }
}
