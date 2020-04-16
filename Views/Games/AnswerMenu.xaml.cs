using CAA_Event_Management.Converters;
using CAA_Event_Management.Data;
using CAA_Event_Management.Models;
using CAA_Event_Management.Utilities;
using CAA_Event_Management.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
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
    public sealed partial class AnswerMenu : Page
    {
        IAnswerRepository answerRepo;
        IPictureRepository picRepo;
        ImageConverter imageConverter = new ImageConverter();
        StorageFile file;

        public AnswerMenu()
        {
            this.InitializeComponent();
            answerRepo = new AnswerRepository();
            picRepo = new PictureRepository();
            PopulateAnswerList();
        }

        public void PopulateAnswerList()
        {
            var APVM = new List<AnswerPictureViewModel>();
            List<Answer> answers = answerRepo.GetAnswers();

            foreach (var a in answers)
            {
                var add = new AnswerPictureViewModel();
                add.Text = a.Text;

                //if answer has an image
                if (a.AnswerPictures.Count != 0)
                {
                    //gets image and adds it to the view model
                    var p = a.AnswerPictures.ToList();
                    Picture picture = picRepo.GetPicture(p[0].PictureID);
                    add.Image = imageConverter.ByteToImage(picture.Image);
                }
                else
                    add.Image = null;
                APVM.Add(add);
            }
            AnswerList.ItemsSource = APVM;
        }

        //Navigation
        private void btnGames_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GameMenu), null, new SuppressNavigationTransitionInfo());
        }

        private void btnQuestion_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(QuestionMenu), null, new SuppressNavigationTransitionInfo());
        }

        private void btnAnswer_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AnswerMenu), null, new SuppressNavigationTransitionInfo());
        }

        private void btnAddAnswer_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void BtnCreateConfirm_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Answer add = new Answer();
            Picture pic = new Picture();

            //If there was an image selected with the create new
            if (file != null)
            {
                //converts image to byte and saves to database
                pic.Image = await imageConverter.ImageToByte(file); ;
                picRepo.AddPicture(pic);
                //If there is text to image adds it to new answer
                if (txtCreateNewAnswer.Text != "")
                    add.Text = txtCreateNewAnswer.Text;

                answerRepo.AddAnswer(add);

                //joins answer to picture in joining table
                var answerPic = new AnswerPicture { AnswerID = add.ID, PictureID = pic.ID };
                answerRepo.AddAnswerPicture(answerPic);
                add.AnswerPictures.Add(answerPic);

                answerRepo.UpdateAnswer(add);
                btnCreateNewAnswer.Flyout.Hide();
                PopulateAnswerList();
            }
            else
            {
                //if no image is added, inserts new answer with text
                add.Text = txtCreateNewAnswer.Text;
                answerRepo.AddAnswer(add);
                btnCreateNewAnswer.Flyout.Hide();
                PopulateAnswerList();
            }
        }

        private void BtnCreateCancel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            btnCreateNewAnswer.Flyout.Hide();
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

            //when image is selected reopens flyout
            btnCreateNewAnswer.Flyout.ShowAt(btnCreateNewAnswer);
            //with file name to confirm selection
            txbImageFile.Text = file.Name;
        }

        private async void btnRemove_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var result = await Jeeves.ConfirmDialog("Warning", "Are you sure you want to delete?");

            if (result == ContentDialogResult.Secondary) //&& btnEditSurvey.Content.ToString() == "#xE74D;"
            {
                int selected = Convert.ToInt32(((Button)sender).DataContext);
                Answer answer = new Answer();
                answer = answerRepo.GetAnswer(selected);
                answerRepo.RemoveAnswer(answer);
                Frame.Navigate(typeof(AnswerMenu), null, new SuppressNavigationTransitionInfo());
            }
        }
    }
}
