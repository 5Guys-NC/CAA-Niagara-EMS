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
using CAA_Event_Management.Views.Games;
using System.Threading.Tasks;
using CAA_Event_Management.ViewModels;
/***********************************
* Created By: Jon Yade
* Edited By: Brian Culp, Jon Yade
* ********************************/

namespace CAA_Event_Management.Views.EventViews
{
    public sealed partial class EventAttendanceTracking : Page
    {
        #region Startup - variables, repositories, methods

        private int questionCount = 0;
        Event currentEvent;
        AttendanceTracking tracker = new AttendanceTracking();
        AttendanceItem survey = new AttendanceItem(); //may be able to delete this after testing
        List<EventItemDetails> ListOfEID = new List<EventItemDetails>();

        IAttendanceItemRepository attendanceItemRepository;
        IAttendanceTrackingRepository attendanceTrackingRepository;
        IEventItemRepository eventItemRepository;
        IItemRepository itemRepository;

        public EventAttendanceTracking()
        {
            this.InitializeComponent();
            attendanceItemRepository = new AttendanceItemRepository();
            attendanceTrackingRepository = new AttendanceTrackingRepository();
            eventItemRepository = new EventItemRepository();
            itemRepository = new ItemRepository();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            currentEvent = (Event)e.Parameter;
            this.DataContext = tracker;
            tracker.EventID = currentEvent.EventID;
            tracker.ArrivalTime = DateTime.Now;
            BuildQuestions();
        }

        #endregion

        #region Buttons - Save, Cancel

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckFormForCompletion()) return;
            //if (!CheckAnswersForCompletion()) return;   //This and the attached function should be deleted
            bool refreshScreen = SaveAttendenceItem();

            //maybe add a bool return for saveSurveyResponses
            if (refreshScreen && currentEvent.QuizID == null) Frame.Navigate(this.GetType(), currentEvent);
            else if (refreshScreen) Frame.Navigate(typeof(PlayerGameView), (Event)currentEvent);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            //I need an if statement here tied to the login to determine what view to return to:

            //if () Frame.Navigate(typeof(EventStartView));
            //else 

            Frame.Navigate(typeof(CAAEvents));
        }

        #endregion

        #region Helper Methods - SaveSurveyResponses, Card Reading

        private bool SaveAttendenceItem()
        {
            bool loadSurveyQuestionsView = true;

            var x = new MemberNumberCheck();
            bool check = true;

            if (memberNumTextBox.Text != "") check = x.CheckMemberNumber(memberNumTextBox.Text);

            if (!check)
            {
                numberError.Visibility = Visibility;
                return false;
            }

            try
            {
                if (isMembersCheck.IsChecked == true) tracker.IsMember = "true";
                else tracker.IsMember = "false";

                tracker.MemberAttendanceID = Guid.NewGuid().ToString();
                tracker.ArrivalTime = DateTime.Now;
                List<EventItem> eventItems = eventItemRepository.GetEventItems(currentEvent.EventID);
                if (eventItems.Count == 0) loadSurveyQuestionsView = false;
                attendanceTrackingRepository.AddAttendanceTracking(tracker);
            }
            catch (Exception exc)
            {
                Jeeves.ShowMessage("Error", exc.InnerException.ToString());
                return false;
            }

            if (loadSurveyQuestionsView == true) SaveSurveyResponses();
            return true;
        }

        private void SaveSurveyResponses()
        {
            foreach (var x in ListOfEID)
            {
                var surveyEntry = new AttendanceItem();
                surveyEntry.EventItemID = x.EIDEventItemID;
                surveyEntry.MemberAttendanceID = tracker.MemberAttendanceID;

                if (tbkQuestionOne.Text == x.EIDItemPhrase)
                {
                    if (ckbAnswerOne.Visibility == Visibility.Visible && ckbAnswerOne.IsChecked == true) surveyEntry.Answer = "true";
                    else if (ckbAnswerOne.Visibility == Visibility.Visible && ckbAnswerOne.IsChecked == false) surveyEntry.Answer = "false";
                    else surveyEntry.Answer = txtAnswerOne.Text;
                }
                else if (tbkQuestionTwo.Text == x.EIDItemPhrase)
                {
                    if (ckbAnswerTwo.Visibility == Visibility.Visible && ckbAnswerTwo.IsChecked == true) surveyEntry.Answer = "true";
                    else if (ckbAnswerTwo.Visibility == Visibility.Visible && ckbAnswerTwo.IsChecked == false) surveyEntry.Answer = "false";
                    else surveyEntry.Answer = txtAnswerTwo.Text;
                }
                else if (tbkQuestionThree.Text == x.EIDItemPhrase)
                {
                    if (ckbAnswerThree.Visibility == Visibility.Visible && ckbAnswerThree.IsChecked == true) surveyEntry.Answer = "true";
                    else if (ckbAnswerThree.Visibility == Visibility.Visible && ckbAnswerThree.IsChecked == false) surveyEntry.Answer = "false";
                    else surveyEntry.Answer = txtAnswerThree.Text;
                }
                else if (tbkQuestionFour.Text == x.EIDItemPhrase)
                {
                    if (ckbAnswerFour.Visibility == Visibility.Visible && ckbAnswerFour.IsChecked == true) surveyEntry.Answer = "true";
                    else if (ckbAnswerFour.Visibility == Visibility.Visible && ckbAnswerFour.IsChecked == false) surveyEntry.Answer = "false";
                    else surveyEntry.Answer = txtAnswerFour.Text;
                }
                else if (tbkQuestionFive.Text == x.EIDItemPhrase)
                {
                    if (ckbAnswerFive.Visibility == Visibility.Visible && ckbAnswerFive.IsChecked == true) surveyEntry.Answer = "true";
                    else if (ckbAnswerFive.Visibility == Visibility.Visible && ckbAnswerFive.IsChecked == false) surveyEntry.Answer = "false";
                    else surveyEntry.Answer = txtAnswerFive.Text;
                }

                try
                {
                    attendanceItemRepository.AddAttendanceItem(surveyEntry);
                }
                catch
                {
                    Jeeves.ShowMessage("Error", "There was a problem entering question " + x.EIDquestionCount.ToString());
                }
            }
        }

        private void cardEntryTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string entry = memberNumTextBox.Text;
            if (entry.StartsWith(" ")) memberNumTextBox.Text = entry.Trim();

            if (entry.Length > 76 && entry.EndsWith("?") && entry.IndexOf("%") == 0)
            {
                memberNumTextBox.Text = entry.Substring(2, 16);
                //int firstName = entry.IndexOf("/");
                //int lastName = entry.IndexOf("^");

                //string firstNamePlusRemainingString = entry.Substring(firstName + 1);
                ////inspired by: https://stackoverflow.com/questions/24028945/find-first-character-in-string-that-is-a-letter
                //string firstNameLetters = new string(firstNamePlusRemainingString.TakeWhile(Char.IsLetter).ToArray());

                //firstNameTextBox.Text = firstNameLetters.Substring(0, 1).ToUpper() + firstNameLetters.Substring(1).ToLower(); ;
                //lastNameTextBox.Text = entry.Substring(lastName + 1, 1) + entry.Substring(lastName + 2, firstName - lastName - 2).ToLower();
                //isMembersCheck.IsChecked = true;

                if (ListOfEID.Count == 0)
                {
                    SaveAttendenceItem();
                    Frame.Navigate(this.GetType(), currentEvent);
                }
            }
        }

        #endregion

        #region Helper Methods - BuildQuestions, ShowQuestions, CheckAnswers

        private void BuildQuestions()
        {
            try
            {
                List<Item> surveyItems = itemRepository.GetItems();
                List<EventItem> eventQuestions = eventItemRepository.GetEventItems(currentEvent.EventID);

                foreach (var x in eventQuestions)
                {
                    SurveyQuestion question = new SurveyQuestion();
                    questionCount += 1;

                    Item thisItem = surveyItems
                        .Where(c => c.ItemID == x.ItemID)
                        .FirstOrDefault();
                    question.questPhrase = thisItem.ItemName;
                    question.questDataType = thisItem.ValueType;

                    //Not sure if I still need all this code here - don't delete without testing via commenting out code first
                    var currentEID = new EventItemDetails();
                    currentEID.EIDEventItemID = x.EventItemID;
                    currentEID.EIDEventID = currentEvent.EventID;
                    currentEID.EIDItemID = x.ItemID;
                    currentEID.EIDItemPhrase = question.questPhrase;
                    currentEID.EIDDataType = question.questDataType;
                    currentEID.EIDquestionCount = questionCount;
                    ListOfEID.Add(currentEID);

                    ShowQuestion(questionCount, question);
                }
            }
            catch
            {
                Jeeves.ShowMessage("Error", "There was an error creating the questions.");
            }
        }

        private void ShowQuestion(int count, SurveyQuestion question)
        {
            if (count == 1)
            {
                tbkQuestionOne.Visibility = Visibility;

                tbkQuestionOne.Text = question.questPhrase;
                if (question.questDataType.Contains("Yes")) ckbAnswerOne.Visibility = Visibility;
                else txtAnswerOne.Visibility = Visibility;
            }
            else if (count == 2)
            {
                tbkQuestionTwo.Visibility = Visibility;
                tbkQuestionTwo.Text = question.questPhrase;
                if (question.questDataType.Contains("Yes")) ckbAnswerTwo.Visibility = Visibility;
                else txtAnswerTwo.Visibility = Visibility;
            }
            else if (count == 3)
            {
                tbkQuestionThree.Visibility = Visibility;
                tbkQuestionThree.Text = question.questPhrase;
                if (question.questDataType.Contains("Yes")) ckbAnswerThree.Visibility = Visibility;
                else txtAnswerThree.Visibility = Visibility;
            }
            else if (count == 4)
            {
                tbkQuestionFour.Visibility = Visibility;
                tbkQuestionFour.Text = question.questPhrase;
                if (question.questDataType.Contains("Yes")) ckbAnswerFour.Visibility = Visibility;
                else txtAnswerFour.Visibility = Visibility;
            }
            else if (count == 5)
            {
                tbkQuestionFive.Visibility = Visibility;
                tbkQuestionFive.Text = question.questPhrase;
                if (question.questDataType.Contains("Yes")) ckbAnswerFive.Visibility = Visibility;
                else txtAnswerFive.Visibility = Visibility;
            }
        }

        private bool CheckFormForCompletion()
        {
            bool formFilledIn = true;
            if (memberNumTextBox.Text.Trim() == "")
            {
                if (firstNameTextBox.Text.Trim() == "" || lastNameTextBox.Text.Trim() == "") formFilledIn = false;
            }

            if (formFilledIn == false)
            {
                Jeeves.ShowMessage("Error", "You must fill in either the member number or a first and last name");
                return false;
            }

            return formFilledIn;
        }

        private bool CheckAnswersForCompletion()
        {
            bool answersFilledIn = true;

            if (txtAnswerOne.Visibility == Visibility && txtAnswerOne.Text.Trim() == "") answersFilledIn = false;
            else if (txtAnswerTwo.Visibility == Visibility && txtAnswerTwo.Text.Trim() == "") answersFilledIn = false;
            else if (txtAnswerThree.Visibility == Visibility && txtAnswerThree.Text.Trim() == "") answersFilledIn = false;
            else if (txtAnswerFour.Visibility == Visibility && txtAnswerFour.Text.Trim() == "") answersFilledIn = false;
            else if (txtAnswerFive.Visibility == Visibility && txtAnswerFive.Text.Trim() == "") answersFilledIn = false;

            if (answersFilledIn == false)
            {
                Jeeves.ShowMessage("Error", "Please fill in all of the survey question boxes");
                return false;
            }
            return true;
        }

        #endregion
    }
    #region  Survey Question Class

    internal class SurveyQuestion
    {
        internal int ID { get; set; }
        internal string questPhrase { get; set; } = "";
        internal string questDataType { get; set; } = "";
    }

    #endregion
}
