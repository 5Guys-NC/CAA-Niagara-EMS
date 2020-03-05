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
    public sealed partial class AnswerMenu : Page
    {
        IAnswerRepository answerRepo;
        public AnswerMenu()
        {
            this.InitializeComponent();
            answerRepo = new AnswerRepository();
            PopulateAnswerList();
        }

        public void PopulateAnswerList()
        {
            List<Answer> answers = answerRepo.GetAnswers();
            AnswerList.ItemsSource = answers;
        }

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

        private void btnAddAnswer_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnCreateConfirm_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Answer add = new Answer();
            add.Text = txtCreateNewAnswer.Text;
            answerRepo.AddAnswer(add);
            btnCreateNewQuestion.Flyout.Hide();
            PopulateAnswerList();
        }

        private void BtnCreateCancel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            btnCreateNewQuestion.Flyout.Hide();
        }

        private void BtnConfirmRemove_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int selected = Convert.ToInt32(((Button)sender).DataContext);
            Answer answer = new Answer();
            answer = answerRepo.GetAnswer(selected);
            answerRepo.RemoveAnswer(answer);
            PopulateAnswerList();

            //int selected = Convert.ToInt32(((Button)sender).DataContext);
            //Game game = new Game();
            //game = gameRepo.GetGame(selected);
            //gameRepo.RemoveGame(game);
            //Frame.Navigate(typeof(GameMenu), null, new SuppressNavigationTransitionInfo());
        }

        private void BtnCancel_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }
    }
}
