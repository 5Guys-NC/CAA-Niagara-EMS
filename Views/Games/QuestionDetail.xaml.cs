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
using CAA_Event_Management.ViewModels;
using CAA_Event_Management.Utilities;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
/******************************
*  Model Created By: Max Cashmore
*  Edited by: Brian Culp
*******************************/

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
namespace CAA_Event_Management.Views.Games
{
    /// <summary>
    /// Frame for Question Details
    /// </summary>
    public sealed partial class QuestionDetail : Page
    {
        #region Startup - variables, respositories, methods

        GameModel selected = new GameModel();
        IAnswerRepository ansRepo;
        IPictureRepository picRepo;
        public List<QuestAnsViewModel> display = new List<QuestAnsViewModel>();
        ImageConverter imageConverter = new ImageConverter();
        StorageFile file;

        public QuestionDetail()
        {
            this.InitializeComponent();
            ansRepo = new AnswerRepository();
            picRepo = new PictureRepository();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            selected = (GameModel)e.Parameter;
            tbQuestion.Text = selected.QuestionText;
            PopulateModelAnswerList();
        }

        public void PopulateModelAnswerList()
        {
            //if question has images
            if (selected.OptionsText != null && selected.AnswerText != null && selected.ImageIDs != "")
            {
                //Splits question's details in their own list
                var options = selected.OptionsText.Split('|');
                var possibleAnswers = selected.AnswerText.Split('|');
                var images = selected.ImageIDs.Split('|');

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

                    if (images[i] != "0")
                    {

                        //gets the image for the question and puts it in the view model
                        Picture pic = picRepo.GetPicture(Convert.ToInt32(images[i]));
                        t.Image = imageConverter.ByteToImage(pic.Image);
                        t.ImageID = images[i];
                    }

                    display.Add(t);
                }
            }

            //if question doesn't have images
            else if (selected.OptionsText != null && selected.AnswerText != null)
            {
                //Splits question's details in their own list
                var options = selected.OptionsText.Split('|');
                var possibleAnswers = selected.AnswerText.Split('|');
                var images = selected.ImageIDs.Split('|');

                //Loop through possible answers
                for (int i = 0; i < options.Length; i++)
                {
                    //place them in holder and set variables 
                    QuestAnsViewModel t = new QuestAnsViewModel();
                    t.Text = options[i];
                    t.Index = i;
                    //If possible answer is in correct answer, checkbox to true
                    if (possibleAnswers.Contains(options[i])
                        || possibleAnswers.Contains(images[i]))
                    { t.IsTrue = true; }
                }
            }

            if (selected.QuestionImageId != null)
            {
                imgQuestionImage.Source = null;
                var q = new Image();
                var p = new Picture();
                p = picRepo.GetPicture(Convert.ToInt32(selected.QuestionImageId));
                q.Source = imageConverter.ByteToImage(p.Image);
                imgQuestionImage.Source = q.Source;
            }

            AnswerList.ItemsSource = display;
            var list = new List<Answer>();

            list = ansRepo.GetAnswerSelection();
            AnswerSelectionList.ItemsSource = list;
        }

        public async void UpdateChanges()
        {
            List<string> option = new List<string>();
            List<string> answer = new List<string>();
            List<string> imageID = new List<string>();
            Picture questPic = new Picture();

            foreach (var d in display)
            {
                //Loops through the view and adds each item to string
                option.Add(d.Text);
                //If checked, adds text to correct answer list
                if (d.IsTrue)
                {
                    //Checks if text is empty. Sets the id to be true
                    if (d.Text == "")
                        answer.Add(d.ImageID);
                    else
                        answer.Add(d.Text);
                }

                //if the view model has an image it sets it's ID
                if (d.ImageID != "0")
                { imageID.Add(d.ImageID.ToString()); }
                //if not, gives it's default ID of 0
                else
                    imageID.Add("0");
            }

            //Checks to see if image was uploaded for answer
            if (file != null)
            {
                questPic.Image = await imageConverter.ImageToByte(file);
                picRepo.AddPicture(questPic);
                selected.QuestionImageId = questPic.ID.ToString();
            }


            //joins all strings together
            selected.OptionsText = string.Join("|", option);
            selected.AnswerText = string.Join("|", answer);
            selected.ImageIDs = string.Join("|", imageID);

        }

        //public void UpdateChanges()
        //{
        //    List<string> option = new List<string>();
        //    List<string> answer = new List<string>();
        //    foreach (var d in display)
        //    {
        //        //Loops through the view and adds each item to string
        //        option.Add(d.Text);
        //        //If checked, adds text to correct answer list
        //        if (d.IsTrue) { answer.Add(d.Text); }
        //    }

        //    //joins all strings together
        //    selected.OptionsText = string.Join("|", option);
        //    selected.AnswerText = string.Join("|", answer);
        //}


        #endregion

        #region Buttons - Add, Edit, Delete
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private void btnSave_Tapped(object sender, TappedRoutedEventArgs e)
        {
            UpdateChanges();
            ansRepo.UpdateGM(selected);

            //Goes back to the list of questions for the game
            IGameRepository gr = new GameRepository();
            Game back = new Game();
            back = gr.GetGame(selected.GameID);
            Frame.Navigate(typeof(GameDetails), back);

            //questionRepo.UpdateQuestion(view);
            //answerRepo.UpdateAnswers(answers);

            //gameRepo = new GameRepository();
            //Game game = new Game();
            //game = gameRepo.GetAGame(view.GameID);
            //Frame.Navigate(typeof(GameDetails), (game));
        }

        private void btnAdd_Tapped(object sender, RoutedEventArgs e)
        {
            //answerRepo.UpdateAnswers(answers);
            //Answer newAnswer = new Answer();
            //newAnswer.QuestionID = view.ID;
            //answerRepo.AddAnswer(newAnswer);
            //Frame.Navigate(typeof(QuestionDetail), (view), new SuppressNavigationTransitionInfo());
            //Frame.BackStack.Remove(this.Frame.BackStack.Last());
        }

        private void btnRemove_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var index = Convert.ToInt32(((Button)sender).DataContext);
            display.RemoveAt(index);
            UpdateChanges();
            Frame.Navigate(typeof(QuestionDetail), selected);

            //answerRepo.UpdateAnswers(answers);
            //int selected = Convert.ToInt32(((Button)sender).DataContext);
            //Answer answer = new Answer();
            //answer = answerRepo.GetAnswer(selected);
            //answerRepo.DeleteAnswer(answer);
            //Frame.Navigate(typeof(QuestionDetail), (view), new SuppressNavigationTransitionInfo());
            //Frame.BackStack.Remove(this.Frame.BackStack.Last());        
        }

        private void btnDeleteQuestion_Tapped(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        private void AnswerSelectionList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var s = (Answer)e.ClickedItem;

            QuestAnsViewModel t = new QuestAnsViewModel();
            t.Text = s.Text;
            t.Index = display.Count + 1;

            //If selected answer has an image.
            if (s.AnswerPictures.Count() != 0)
            {
                //get the image and adds it to the view model
                var p = s.AnswerPictures.ToList();
                Picture picture = picRepo.GetPicture(p[0].PictureID);
                t.Image = imageConverter.ByteToImage(picture.Image);
                t.ImageID = picture.ID.ToString();
            }

            //Add holder into list<> display
            display.Add(t);
            UpdateChanges();
            Frame.Navigate(typeof(QuestionDetail), selected);
        }

        private void btnAddAnswer_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCreateConfirm_Tapped(object sender, TappedRoutedEventArgs e)
        {


            QuestAnsViewModel t = new QuestAnsViewModel();
            t.Text = txtAddNewAnswer.Text; ;
            t.Index = display.Count + 1;
            //If possible answer is in correct answer, checkbox to true

            //Add holder into list<> display
            display.Add(t);
            UpdateChanges();
            Frame.Navigate(typeof(QuestionDetail), selected);
        }

        private void btnCreateCancel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            btnAddAnswer.Flyout.Hide();
        }

        private async void btnImageUpload_Click(object sender, RoutedEventArgs e)
        {
            /// <summary>
            /// File picker code source:
            /// https://docs.microsoft.com/en-us/windows/uwp/files/quickstart-using-file-and-folder-pickers
            /// </summary>

            file = null;
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".png");
            file = await openPicker.PickSingleFileAsync();

            UpdateChanges();
            Frame.Navigate(typeof(QuestionDetail), selected);
        }
    }
}