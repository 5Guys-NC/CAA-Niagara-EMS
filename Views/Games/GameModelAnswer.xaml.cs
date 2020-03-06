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
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CAA_Event_Management.Views.Games
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GameModelAnswer : Page
    {
        GameModel selected = new GameModel();
        IAnswerRepository ansRepo;
        public List<QuestAnsViewModel> display = new List<QuestAnsViewModel>();

        public GameModelAnswer()
        {
            this.InitializeComponent();
            ansRepo = new AnswerRepository();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            selected = (GameModel)e.Parameter;
            txtQuestionHeader.Text = selected.QuestionText;
            PopulateModelAnswerList();
        }

        public void PopulateModelAnswerList()
        {
            display.Clear();
            //Seperate Question's possible answers
            //then seperate correct answers
            if (selected.OptionsText != null && selected.AnswerText != null)
            {
                var options = selected.OptionsText.Split('|');
                var possibleAnswers = selected.AnswerText.Split('|');

                //Loop through possible answers
                for (int i = 0; i < options.Length; i++)
                {
                    //place them in holder and set variables 
                    QuestAnsViewModel t = new QuestAnsViewModel();
                    t.Text = options[i];
                    t.Index = i;
                    //If possible answer is in correct answer, checkbox to true
                    if (possibleAnswers.Contains(options[i]))
                    { t.IsTrue = true; }
                    //Add holder into list<> display
                    display.Add(t);
                }
            }
            ModelAnswerList.ItemsSource = display;
            var list = new List<Answer>();
            list = ansRepo.GetAnswerSelection();
            AnswerSelectionList.ItemsSource = list;
        }
        //!!!Important!!!
        //Updates the VIEW
        public void UpdateChanges()
        {
            List<string> option = new List<string>();
            List<string> answer = new List<string>();
            foreach (var d in display)
            {
                //Loops through the view and adds each item to string
                option.Add(d.Text);
                //If checked, adds text to correct answer list
                if (d.IsTrue) { answer.Add(d.Text); }
            }

            //joins all strings together
            selected.OptionsText = string.Join("|", option);
            selected.AnswerText = string.Join("|", answer);
        }

        //!!!Important!!!
        //Updates/saves the MODEL to the database
        private void UpdateGM(object sender, RoutedEventArgs e)
        {
            UpdateChanges();
            ansRepo.UpdateGM(selected);

            //Goes back to the list of questions for the game
            IGameRepository gr = new GameRepository();
            Game back = new Game();
            back = gr.GetGame(selected.GameID);
            Frame.Navigate(typeof(GameModelQuestion), back);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var index = Convert.ToInt32(((Button)sender).DataContext);
            display.RemoveAt(index);
            UpdateChanges();
            Frame.Navigate(typeof(GameModelAnswer), selected);
        }

        private void AnswerSelectionList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var s = (Answer)e.ClickedItem;

            QuestAnsViewModel t = new QuestAnsViewModel();
            t.Text = s.Text;
            t.Index = display.Count + 1;
            //If possible answer is in correct answer, checkbox to true

            //Add holder into list<> display
            display.Add(t);
            UpdateChanges();
            Frame.Navigate(typeof(GameModelAnswer), selected);
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
