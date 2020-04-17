using CAA_Event_Management.Data;
using CAA_Event_Management.Data.Interface_Repos;
using CAA_Event_Management.Data.Repos;
using CAA_Event_Management.Models;
using CAA_Event_Management.Utilities;
using CAA_Event_Management.ViewModels;
using CAA_Event_Management.Views.EventViews;
using CAA_Event_Management.Views.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
/***********************************
 * Created By: Jon Yade 95%
 * Edited By: Brian Culp 5%
 * ********************************/

namespace CAA_Event_Management.Views.EventViews
{
    public sealed partial class EventAttendanceTracking : Page
    {
        #region Startup - variables, repositories, methods

        private int questionCount = 0;
        Models.Event currentEvent;
        AttendanceTracking tracker = new AttendanceTracking();
        List<EventItemDetails> ListOfEID = new List<EventItemDetails>();

        private string cardInfo = "g"; //the default value for this is "g"
        MemberNumberCheck MemNumCheck = new MemberNumberCheck();

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
            ((Window.Current.Content as Frame).Content as MainPage).ChangeMainPageTitleName(currentEvent.DisplayName.ToUpper());
            ((Window.Current.Content as Frame).Content as MainPage).HideTheNavBar(false);
            ((Window.Current.Content as Frame).Content as MainPage).HideTheLoginButton();
            this.DataContext = tracker;
            tracker.EventID = currentEvent.EventID;
            tracker.ArrivalTime = DateTime.Now;
            BuildQuestions();
            ShowLastSwipeInfo();
            Window.Current.CoreWindow.CharacterReceived += CoreWindow_CharacterReceived;

        }

        /// <summary>
        /// This method handles either the swiping or scanning of a CAA card by examining each keyboard character recieved.
        /// After recieving a character, it decides how to best use it. It records all entries in the "cardInfo" string
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void CoreWindow_CharacterReceived(CoreWindow sender, CharacterReceivedEventArgs args)
        {
            if (args.KeyCode == 37)
            {
                cardInfo = "%";
            }
            else if (cardInfo.Substring(0, 1) == "%")
            {
                cardInfo += Convert.ToChar(args.KeyCode).ToString();
            }

            if (cardInfo.Substring(0, 1) != "%" && args.KeyCode >= 48 && args.KeyCode <= 57)
            {
                if (cardInfo == "g") cardInfo = Convert.ToChar(args.KeyCode).ToString();
                else cardInfo += Convert.ToChar(args.KeyCode).ToString();
                if (cardInfo.Length == 16)
                {
                    if (MemNumCheck.CheckMemberNumber(cardInfo)) SaveScannedCardNumber();
                    else cardInfo = "g";
                }
            }

            if (cardInfo != "g" && cardInfo.Substring(0, 1) != "%" && args.KeyCode < 48 && args.KeyCode > 57)
            {
                cardInfo = "g";
            }

            if (cardInfo.Length == 18 && cardInfo.Substring(0, 1) == "%" && !MemNumCheck.CheckMemberNumber(cardInfo.Substring(2)))
            {
                cardInfo = "g";
            }

            if (cardInfo.Length > 70 && cardInfo.EndsWith("?"))
            //if (cardInfo.Length > 75 && cardInfo.EndsWith((char)Windows.System.VirtualKey.Enter))
            {
                CardReadDisplay();
            }

            if (cardInfo.Length == 18)
            {
                memberNumTextBox.IsTabStop = false;
                firstNameTextBox.IsTabStop = false;
                lastNameTextBox.IsTabStop = false;
                phoneNumTextBox.IsTabStop = false;
                TextBoxLockDown();
            }
        }

        #endregion

        #region Buttons - Save, Cancel

        /// <summary>
        /// The method handles the "Submit" Button click event by a user in the AttendanceTracking view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckFormForCompletion()) return;
            //if (!CheckAnswersForCompletion()) return;   //This and the attached function should be deleted
            bool refreshScreen = SaveAttendanceTrackingObject();
            if (refreshScreen && currentEvent.QuizID == null) Frame.Navigate(this.GetType(), currentEvent, new SuppressNavigationTransitionInfo());
            else if (refreshScreen) Frame.Navigate(typeof(PlayerGameView), (Event)currentEvent, new SuppressNavigationTransitionInfo());
        }

        /// <summary>
        /// The method handles the "Back" Button click event by a user in the AttendanceTracking view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            ((Window.Current.Content as Frame).Content as MainPage).HideTheNavBar(true);
            App userCheck = (App)Application.Current;

            ((Window.Current.Content as Frame).Content as MainPage).ShowTheLoginButton();
            if (userCheck.userIsLogIn == false) Frame.Navigate(typeof(EventStartView), new SuppressNavigationTransitionInfo());
            else Frame.Navigate(typeof(CAAEvents), new SuppressNavigationTransitionInfo());
        }

        #endregion

        #region Helper Methods - CardReadDisplay, SaveScannedCardNumber, SaveSurveyResponses, SaveAttendanceTrackingObject, TextBoxLockDown, TextBoxUnlock, ClearSurveyFields

        /// <summary>
        /// If a card is swiped and the magnetic strip is read, this method handles the data entry
        /// </summary>
        private void CardReadDisplay()
        {
            try
            {
                tracker.MemberNo = cardInfo.Substring(2, 16);
                tracker.IsMember = "true";

                int firstName = cardInfo.IndexOf("/");
                int lastName = cardInfo.IndexOf("^");

                string firstNamePlusRemainingString = cardInfo.Substring(firstName + 1);
                //inspired by: https://stackoverflow.com/questions/24028945/find-first-character-in-string-that-is-a-letter
                string firstNameLetters = new string(firstNamePlusRemainingString.TakeWhile(Char.IsLetter).ToArray());

                tracker.FirstName = firstNameLetters.Substring(0, 1).ToUpper() + firstNameLetters.Substring(1).ToLower(); ;
                tracker.LastName = cardInfo.Substring(lastName + 1, 1) + cardInfo.Substring(lastName + 2, firstName - lastName - 2).ToLower();

                if (ListOfEID.Count == 0)
                {
                    bool refreshScreen = SaveAttendanceTrackingObject();
                    if (refreshScreen && currentEvent.QuizID == null) Frame.Navigate(this.GetType(), currentEvent, new SuppressNavigationTransitionInfo());
                    else if (refreshScreen) Frame.Navigate(typeof(PlayerGameView), (Event)currentEvent, new SuppressNavigationTransitionInfo());
                }
                else
                {
                    phoneNumTextBox.IsTabStop = true;
                    isMembersCheck.IsChecked = true;
                    memberNumTextBox.Text = tracker.MemberNo;
                    firstNameTextBox.Text = tracker.FirstName;
                    lastNameTextBox.Text = tracker.LastName;

                    cardInfo = "g";
                    phoneNumTextBox.Text = "";
                    ClearSurveyFields();
                    TextBoxUnlock();
                }
            }
            catch
            {
                Jeeves.ShowMessage("Error", "Please re-swip card");
            }
        }

        /// <summary>
        /// This method sets the current AttandenceTracking object's member number to that of the scanned CAA card.
        /// If there are no survey questions, the form will auto submit
        /// </summary>
        /// <param name="cardNumber">This string parameter carries with the scanned card number</param>
        private void SaveScannedCardNumber()
        {
            tracker.MemberNo = cardInfo;
            tracker.IsMember = "true";

            if (ListOfEID.Count == 0)
            {
                bool refreshScreen = SaveAttendanceTrackingObject();
                if (refreshScreen && currentEvent.QuizID == null) Frame.Navigate(this.GetType(), currentEvent, new SuppressNavigationTransitionInfo());
                else if (refreshScreen) Frame.Navigate(typeof(PlayerGameView), (Event)currentEvent, new SuppressNavigationTransitionInfo());
            }
            else
            {
                memberNumTextBox.Text = tracker.MemberNo;
                memberNumTextBox.IsTabStop = false;
                ClearSurveyFields();
                if (firstNameTextBox.Text == tracker.MemberNo) firstNameTextBox.Text = "";
                if (lastNameTextBox.Text == tracker.MemberNo) lastNameTextBox.Text = "";
                if (phoneNumBlock.Text == tracker.MemberNo) phoneNumBlock.Text = "";
                isMembersCheck.IsChecked = true;
                cardInfo = "g";
            }
        }

        /// <summary>
        /// This method saves the AttendanceTracking object to the database. If it is a successful save, the method 
        /// returns a bool of "true", otherwise it returns a "false"
        /// </summary>
        /// <returns>A bool response of "true" if the method successfully saved the response</returns>
        private bool SaveAttendanceTrackingObject()
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

            if (tracker.FirstName == "")
            {
                tracker.FirstName = firstNameTextBox.Text.Trim();
            }
            if (tracker.LastName == "")
            {
                tracker.LastName = lastNameTextBox.Text.Trim();
            }
            if (memberNumTextBox.Text == "")
            {
                tracker.PhoneNo = phoneNumTextBox.Text.Trim();
            }
            if (isMembersCheck.IsChecked == true) tracker.IsMember = "true";
            else tracker.IsMember = "false";

            try
            {
                tracker.MemberAttendanceID = Guid.NewGuid().ToString();
                tracker.ArrivalTime = DateTime.Now;
                List<EventItem> eventItems = eventItemRepository.GetEventItems(currentEvent.EventID);
                if (eventItems.Count == 0) loadSurveyQuestionsView = false;

                attendanceTrackingRepository.AddAttendanceTracking(tracker);
            }
            catch
            {
                Jeeves.ShowMessage("Error", "There was a problem saving this event to the data base; please re-try");
                return false;
            }

            if (loadSurveyQuestionsView == true) SaveSurveyResponses();
            return true;
        }

        /// <summary>
        /// This method saves all the survey question responses to the AttendanceItem table
        /// </summary>
        private void SaveSurveyResponses()
        {
            foreach (var x in ListOfEID)
            {
                var surveyEntry = new AttendanceItem();
                surveyEntry.EventItemID = x.EIDEventItemID;
                surveyEntry.MemberAttendanceID = tracker.MemberAttendanceID;
                surveyEntry.Answer = "";

                if (tbkQuestionOne.Text == x.EIDItemPhrase)
                {
                    if (ckbAnswerOne.Visibility == Visibility.Visible && ckbAnswerOne.IsChecked == true) surveyEntry.Answer = "true";
                    else if (ckbAnswerOne.Visibility == Visibility.Visible && ckbAnswerOne.IsChecked == false) surveyEntry.Answer = "false";
                    else if (dprAnswerOne.Visibility == Visibility.Visible) surveyEntry.Answer = (dprAnswerOne.SelectedDate.ToString()).Split(' ')[0];
                    else surveyEntry.Answer = txtAnswerOne.Text.Trim();
                }
                else if (tbkQuestionTwo.Text == x.EIDItemPhrase)
                {
                    if (ckbAnswerTwo.Visibility == Visibility.Visible && ckbAnswerTwo.IsChecked == true) surveyEntry.Answer = "true";
                    else if (ckbAnswerTwo.Visibility == Visibility.Visible && ckbAnswerTwo.IsChecked == false) surveyEntry.Answer = "false";
                    else if (dprAnswerTwo.Visibility == Visibility.Visible) surveyEntry.Answer = (dprAnswerTwo.SelectedDate.ToString()).Split(' ')[0];
                    else surveyEntry.Answer = txtAnswerTwo.Text.Trim();
                }
                else if (tbkQuestionThree.Text == x.EIDItemPhrase)
                {
                    if (ckbAnswerThree.Visibility == Visibility.Visible && ckbAnswerThree.IsChecked == true) surveyEntry.Answer = "true";
                    else if (ckbAnswerThree.Visibility == Visibility.Visible && ckbAnswerThree.IsChecked == false) surveyEntry.Answer = "false";
                    else if (dprAnswerThree.Visibility == Visibility.Visible) surveyEntry.Answer = (dprAnswerThree.SelectedDate.ToString()).Split(' ')[0];
                    else surveyEntry.Answer = txtAnswerThree.Text.Trim();
                }
                else if (tbkQuestionFour.Text == x.EIDItemPhrase)
                {
                    if (ckbAnswerFour.Visibility == Visibility.Visible && ckbAnswerFour.IsChecked == true) surveyEntry.Answer = "true";
                    else if (ckbAnswerFour.Visibility == Visibility.Visible && ckbAnswerFour.IsChecked == false) surveyEntry.Answer = "false";
                    else if (dprAnswerFour.Visibility == Visibility.Visible) surveyEntry.Answer = (dprAnswerFour.SelectedDate.ToString()).Split(' ')[0];
                    else surveyEntry.Answer = txtAnswerFour.Text.Trim();
                }
                else if (tbkQuestionFive.Text == x.EIDItemPhrase)
                {
                    if (ckbAnswerFive.Visibility == Visibility.Visible && ckbAnswerFive.IsChecked == true) surveyEntry.Answer = "true";
                    else if (ckbAnswerFive.Visibility == Visibility.Visible && ckbAnswerFive.IsChecked == false) surveyEntry.Answer = "false";
                    else if (dprAnswerFive.Visibility == Visibility.Visible) surveyEntry.Answer = (dprAnswerFive.SelectedDate.ToString()).Split(' ')[0];
                    else surveyEntry.Answer = txtAnswerFive.Text.Trim();
                }
                else if (tbkQuestionSix.Text == x.EIDItemPhrase)
                {
                    if (ckbAnswerSix.Visibility == Visibility.Visible && ckbAnswerSix.IsChecked == true) surveyEntry.Answer = "true";
                    else if (ckbAnswerSix.Visibility == Visibility.Visible && ckbAnswerSix.IsChecked == false) surveyEntry.Answer = "false";
                    else if (dprAnswerSix.Visibility == Visibility.Visible) surveyEntry.Answer = (dprAnswerSix.SelectedDate.ToString()).Split(' ')[0];
                    else surveyEntry.Answer = txtAnswerSix.Text.Trim();
                }
                else if (tbkQuestionSeven.Text == x.EIDItemPhrase)
                {
                    if (ckbAnswerSeven.Visibility == Visibility.Visible && ckbAnswerSeven.IsChecked == true) surveyEntry.Answer = "true";
                    else if (ckbAnswerSeven.Visibility == Visibility.Visible && ckbAnswerSeven.IsChecked == false) surveyEntry.Answer = "false";
                    else if (dprAnswerSeven.Visibility == Visibility.Visible) surveyEntry.Answer = (dprAnswerSeven.SelectedDate.ToString()).Split(' ')[0];
                    else surveyEntry.Answer = txtAnswerSeven.Text.Trim();
                }
                else if (tbkQuestionEight.Text == x.EIDItemPhrase)
                {
                    if (ckbAnswerEight.Visibility == Visibility.Visible && ckbAnswerEight.IsChecked == true) surveyEntry.Answer = "true";
                    else if (ckbAnswerEight.Visibility == Visibility.Visible && ckbAnswerEight.IsChecked == false) surveyEntry.Answer = "false";
                    else if (dprAnswerEight.Visibility == Visibility.Visible) surveyEntry.Answer = (dprAnswerEight.SelectedDate.ToString()).Split(' ')[0];
                    else surveyEntry.Answer = txtAnswerEight.Text.Trim();
                }
                else if (tbkQuestionNine.Text == x.EIDItemPhrase)
                {
                    if (ckbAnswerNine.Visibility == Visibility.Visible && ckbAnswerNine.IsChecked == true) surveyEntry.Answer = "true";
                    else if (ckbAnswerNine.Visibility == Visibility.Visible && ckbAnswerNine.IsChecked == false) surveyEntry.Answer = "false";
                    else if (dprAnswerNine.Visibility == Visibility.Visible) surveyEntry.Answer = (dprAnswerNine.SelectedDate.ToString()).Split(' ')[0];
                    else surveyEntry.Answer = txtAnswerNine.Text.Trim();
                }
                else if (tbkQuestionTen.Text == x.EIDItemPhrase)
                {
                    if (ckbAnswerTen.Visibility == Visibility.Visible && ckbAnswerTen.IsChecked == true) surveyEntry.Answer = "true";
                    else if (ckbAnswerTen.Visibility == Visibility.Visible && ckbAnswerTen.IsChecked == false) surveyEntry.Answer = "false";
                    else if (dprAnswerTen.Visibility == Visibility.Visible) surveyEntry.Answer = (dprAnswerTen.SelectedDate.ToString()).Split(' ')[0];
                    else surveyEntry.Answer = txtAnswerTen.Text.Trim();
                }

                try
                {
                    if (surveyEntry.Answer != "")
                    {
                        attendanceItemRepository.AddAttendanceItem(surveyEntry);
                    }
                    else
                    {
                        surveyEntry.Answer = null;
                        attendanceItemRepository.AddAttendanceItem(surveyEntry);
                    }
                }
                catch
                {
                    Jeeves.ShowMessage("Error", "There was a problem saving question " + x.EIDquestionCount.ToString());
                }
            }
        }

        /// <summary>
        /// This method locks down the survey questions as the card is read
        /// </summary>
        private void TextBoxLockDown()
        {
            txtAnswerOne.IsTabStop = false;
            txtAnswerTwo.IsTabStop = false;
            txtAnswerThree.IsTabStop = false;
            txtAnswerFour.IsTabStop = false;
            txtAnswerFive.IsTabStop = false;
            txtAnswerSix.IsTabStop = false;
            txtAnswerSeven.IsTabStop = false;
            txtAnswerEight.IsTabStop = false;
            txtAnswerNine.IsTabStop = false;
            txtAnswerTen.IsTabStop = false;

            dprAnswerOne.IsEnabled = false;
            dprAnswerTwo.IsEnabled = false;
            dprAnswerThree.IsEnabled = false;
            dprAnswerFour.IsEnabled = false;
            dprAnswerFive.IsEnabled = false;
            dprAnswerSix.IsEnabled = false;
            dprAnswerSeven.IsEnabled = false;
            dprAnswerEight.IsEnabled = false;
            dprAnswerNine.IsEnabled = false;
            dprAnswerTen.IsEnabled = false;
        }

        /// <summary>
        /// This method unlocks the survey questions after the card is read
        /// </summary>
        private void TextBoxUnlock()
        {
            txtAnswerOne.IsTabStop = true;
            txtAnswerTwo.IsTabStop = true;
            txtAnswerThree.IsTabStop = true;
            txtAnswerFour.IsTabStop = true;
            txtAnswerFive.IsTabStop = true;
            txtAnswerSix.IsTabStop = true;
            txtAnswerSeven.IsTabStop = true;
            txtAnswerEight.IsTabStop = true;
            txtAnswerNine.IsTabStop = true;
            txtAnswerTen.IsTabStop = true;

            dprAnswerOne.IsEnabled = true;
            dprAnswerTwo.IsEnabled = true;
            dprAnswerThree.IsEnabled = true;
            dprAnswerFour.IsEnabled = true;
            dprAnswerFive.IsEnabled = true;
            dprAnswerSix.IsEnabled = true;
            dprAnswerSeven.IsEnabled = true;
            dprAnswerEight.IsEnabled = true;
            dprAnswerNine.IsEnabled = true;
            dprAnswerTen.IsEnabled = true;
        }

        /// <summary>
        /// This method clears all the Survey Question text boxes in case of accidental text entry
        /// </summary>
        private void ClearSurveyFields()
        {
            txtAnswerOne.Text = "";
            txtAnswerTwo.Text = "";
            txtAnswerThree.Text = "";
            txtAnswerFour.Text = "";
            txtAnswerFive.Text = "";
            txtAnswerSix.Text = "";
            txtAnswerSeven.Text = "";
            txtAnswerEight.Text = "";
            txtAnswerNine.Text = "";
            txtAnswerTen.Text = "";
        }

        #endregion

        #region Helper Methods - BuildQuestions, ShowQuestions, CheckAnswers, ShowLastSwipeInfo, CheckFormForCompletion

        /// <summary>
        /// This method builds the various questions for the event and ties the Item object details to the EventItem objects
        /// </summary>
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

        /// <summary>
        /// This method builds the questions in the Attendance Tracking view by connecting the question text to the 
        /// necessary entry field
        /// </summary>
        /// <param name="count">This parameter contains the current question number</param>
        /// <param name="question">This parameter contains the actual question info that is to built</param>
        private void ShowQuestion(int count, SurveyQuestion question)
        {
            if (count == 1)
            {
                tbkQuestionOne.Visibility = Visibility;
                tbkQuestionOne.Text = question.questPhrase;
                if (question.questDataType.Contains("Yes"))
                {
                    ckbAnswerOne.Visibility = Visibility;
                }
                else if (question.questDataType.Contains("Dates"))
                {
                    dprAnswerOne.Visibility = Visibility;
                }
                else
                {
                    //Inputscope code comes from https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.textbox.inputscope
                    txtAnswerOne.Visibility = Visibility;
                    InputScope scope = new InputScope();
                    InputScopeName scopeName = new InputScopeName();
                    if (question.questDataType.Contains("Numbers"))
                    {
                        scopeName.NameValue = InputScopeNameValue.Number;
                    }
                    else if (question.questDataType.Contains("Email"))
                    {
                        scopeName.NameValue = InputScopeNameValue.EmailSmtpAddress;
                    }
                    else
                    {
                        scopeName.NameValue = InputScopeNameValue.Chat;
                    }
                    scope.Names.Add(scopeName);
                    txtAnswerOne.InputScope = scope;
                }
            }
            else if (count == 2)
            {
                tbkQuestionTwo.Visibility = Visibility;
                tbkQuestionTwo.Text = question.questPhrase;
                if (question.questDataType.Contains("Yes"))
                {
                    ckbAnswerTwo.Visibility = Visibility;
                }
                else if (question.questDataType.Contains("Dates"))
                {
                    dprAnswerTwo.Visibility = Visibility;
                }
                else
                {
                    //Inputscope code comes from https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.textbox.inputscope
                    txtAnswerTwo.Visibility = Visibility;
                    InputScope scope = new InputScope();
                    InputScopeName scopeName = new InputScopeName();
                    if (question.questDataType.Contains("Numbers"))
                    {
                        scopeName.NameValue = InputScopeNameValue.Number;
                    }
                    else if (question.questDataType.Contains("Email"))
                    {
                        scopeName.NameValue = InputScopeNameValue.EmailSmtpAddress;
                    }
                    else
                    {
                        scopeName.NameValue = InputScopeNameValue.Chat;
                    }
                    scope.Names.Add(scopeName);
                    txtAnswerTwo.InputScope = scope;
                };
            }
            else if (count == 3)
            {
                tbkQuestionThree.Visibility = Visibility;
                tbkQuestionThree.Text = question.questPhrase;
                if (question.questDataType.Contains("Yes"))
                {
                    ckbAnswerThree.Visibility = Visibility;
                }
                else if (question.questDataType.Contains("Dates"))
                {
                    dprAnswerThree.Visibility = Visibility;
                }
                else
                {
                    //Inputscope code comes from https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.textbox.inputscope
                    txtAnswerThree.Visibility = Visibility;
                    InputScope scope = new InputScope();
                    InputScopeName scopeName = new InputScopeName();
                    if (question.questDataType.Contains("Numbers"))
                    {
                        scopeName.NameValue = InputScopeNameValue.Number;
                    }
                    else if (question.questDataType.Contains("Email"))
                    {
                        scopeName.NameValue = InputScopeNameValue.EmailSmtpAddress;
                    }
                    else
                    {
                        scopeName.NameValue = InputScopeNameValue.Chat;
                    }
                    scope.Names.Add(scopeName);
                    txtAnswerThree.InputScope = scope;
                }
            }
            else if (count == 4)
            {
                tbkQuestionFour.Visibility = Visibility;
                tbkQuestionFour.Text = question.questPhrase;
                if (question.questDataType.Contains("Yes"))
                {
                    ckbAnswerFour.Visibility = Visibility;
                }
                else if (question.questDataType.Contains("Dates"))
                {
                    dprAnswerFour.Visibility = Visibility;
                }
                else
                {
                    //Inputscope code comes from https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.textbox.inputscope
                    txtAnswerFour.Visibility = Visibility;
                    InputScope scope = new InputScope();
                    InputScopeName scopeName = new InputScopeName();
                    if (question.questDataType.Contains("Numbers"))
                    {
                        scopeName.NameValue = InputScopeNameValue.Number;
                    }
                    else if (question.questDataType.Contains("Email"))
                    {
                        scopeName.NameValue = InputScopeNameValue.EmailSmtpAddress;
                    }
                    else
                    {
                        scopeName.NameValue = InputScopeNameValue.Chat;
                    }
                    scope.Names.Add(scopeName);
                    txtAnswerFour.InputScope = scope;
                }
            }
            else if (count == 5)
            {
                tbkQuestionFive.Visibility = Visibility;
                tbkQuestionFive.Text = question.questPhrase;
                if (question.questDataType.Contains("Yes"))
                {
                    ckbAnswerFive.Visibility = Visibility;
                }
                else if (question.questDataType.Contains("Dates"))
                {
                    dprAnswerFive.Visibility = Visibility;
                }
                else
                {
                    //Inputscope code comes from https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.textbox.inputscope
                    txtAnswerFive.Visibility = Visibility;
                    InputScope scope = new InputScope();
                    InputScopeName scopeName = new InputScopeName();
                    if (question.questDataType.Contains("Numbers"))
                    {
                        scopeName.NameValue = InputScopeNameValue.Number;
                    }
                    else if (question.questDataType.Contains("Email"))
                    {
                        scopeName.NameValue = InputScopeNameValue.EmailSmtpAddress;
                    }
                    else
                    {
                        scopeName.NameValue = InputScopeNameValue.Chat;
                    }
                    scope.Names.Add(scopeName);
                    txtAnswerFive.InputScope = scope;
                }
            }
            else if (count == 6)
            {
                tbkQuestionSix.Visibility = Visibility;
                tbkQuestionSix.Text = question.questPhrase;
                if (question.questDataType.Contains("Yes"))
                {
                    ckbAnswerSix.Visibility = Visibility;
                }
                else if (question.questDataType.Contains("Dates"))
                {
                    dprAnswerSix.Visibility = Visibility;
                }
                else
                {
                    //Inputscope code comes from https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.textbox.inputscope
                    txtAnswerSix.Visibility = Visibility;
                    InputScope scope = new InputScope();
                    InputScopeName scopeName = new InputScopeName();
                    if (question.questDataType.Contains("Numbers"))
                    {
                        scopeName.NameValue = InputScopeNameValue.Number;
                    }
                    else if (question.questDataType.Contains("Email"))
                    {
                        scopeName.NameValue = InputScopeNameValue.EmailSmtpAddress;
                    }
                    else
                    {
                        scopeName.NameValue = InputScopeNameValue.Chat;
                    }
                    scope.Names.Add(scopeName);
                    txtAnswerSix.InputScope = scope;
                }
            }
            else if (count == 7)
            {
                tbkQuestionSeven.Visibility = Visibility;
                tbkQuestionSeven.Text = question.questPhrase;
                if (question.questDataType.Contains("Yes"))
                {
                    ckbAnswerSeven.Visibility = Visibility;
                }
                else if (question.questDataType.Contains("Dates"))
                {
                    dprAnswerSeven.Visibility = Visibility;
                }
                else
                {
                    //Inputscope code comes from https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.textbox.inputscope
                    txtAnswerSeven.Visibility = Visibility;
                    InputScope scope = new InputScope();
                    InputScopeName scopeName = new InputScopeName();
                    if (question.questDataType.Contains("Numbers"))
                    {
                        scopeName.NameValue = InputScopeNameValue.Number;
                    }
                    else if (question.questDataType.Contains("Email"))
                    {
                        scopeName.NameValue = InputScopeNameValue.EmailSmtpAddress;
                    }
                    else
                    {
                        scopeName.NameValue = InputScopeNameValue.Chat;
                    }
                    scope.Names.Add(scopeName);
                    txtAnswerSeven.InputScope = scope;
                }
            }
            else if (count == 8)
            {
                tbkQuestionEight.Visibility = Visibility;
                tbkQuestionEight.Text = question.questPhrase;
                if (question.questDataType.Contains("Yes"))
                {
                    ckbAnswerEight.Visibility = Visibility;
                }
                else if (question.questDataType.Contains("Dates"))
                {
                    dprAnswerEight.Visibility = Visibility;
                }
                else
                {
                    //Inputscope code comes from https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.textbox.inputscope
                    txtAnswerEight.Visibility = Visibility;
                    InputScope scope = new InputScope();
                    InputScopeName scopeName = new InputScopeName();
                    if (question.questDataType.Contains("Numbers"))
                    {
                        scopeName.NameValue = InputScopeNameValue.Number;
                    }
                    else if (question.questDataType.Contains("Email"))
                    {
                        scopeName.NameValue = InputScopeNameValue.EmailSmtpAddress;
                    }
                    else
                    {
                        scopeName.NameValue = InputScopeNameValue.Chat;
                    }
                    scope.Names.Add(scopeName);
                    txtAnswerEight.InputScope = scope;
                }
            }
            else if (count == 9)
            {
                tbkQuestionNine.Visibility = Visibility;
                tbkQuestionNine.Text = question.questPhrase;
                if (question.questDataType.Contains("Yes"))
                {
                    ckbAnswerNine.Visibility = Visibility;
                }
                else if (question.questDataType.Contains("Dates"))
                {
                    dprAnswerNine.Visibility = Visibility;
                }
                else
                {
                    //Inputscope code comes from https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.textbox.inputscope
                    txtAnswerNine.Visibility = Visibility;
                    InputScope scope = new InputScope();
                    InputScopeName scopeName = new InputScopeName();
                    if (question.questDataType.Contains("Numbers"))
                    {
                        scopeName.NameValue = InputScopeNameValue.Number;
                    }
                    else if (question.questDataType.Contains("Email"))
                    {
                        scopeName.NameValue = InputScopeNameValue.EmailSmtpAddress;
                    }
                    else
                    {
                        scopeName.NameValue = InputScopeNameValue.Chat;
                    }
                    scope.Names.Add(scopeName);
                    txtAnswerNine.InputScope = scope;
                }
            }
            else if (count == 10)
            {
                tbkQuestionTen.Visibility = Visibility;
                tbkQuestionTen.Text = question.questPhrase;
                if (question.questDataType.Contains("Yes"))
                {
                    ckbAnswerTen.Visibility = Visibility;
                }
                else if (question.questDataType.Contains("Dates"))
                {
                    dprAnswerOne.Visibility = Visibility;
                }
                else
                {
                    //Inputscope code comes from https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.textbox.inputscope
                    txtAnswerTen.Visibility = Visibility;
                    InputScope scope = new InputScope();
                    InputScopeName scopeName = new InputScopeName();
                    if (question.questDataType.Contains("Numbers"))
                    {
                        scopeName.NameValue = InputScopeNameValue.Number;
                    }
                    else if (question.questDataType.Contains("Email"))
                    {
                        scopeName.NameValue = InputScopeNameValue.EmailSmtpAddress;
                    }
                    else
                    {
                        scopeName.NameValue = InputScopeNameValue.Chat;
                    }
                    scope.Names.Add(scopeName);
                    txtAnswerTen.InputScope = scope;
                }
            }
        }

        /// <summary>
        /// This method ensures that the form (AttendanceTracking view) is submitted only if it is correctly filled out
        /// </summary>
        /// <returns></returns>
        private bool CheckFormForCompletion()
        {
            bool formMemeberNumCheck = false;
            bool formPersonInfoCheck = false;

            if (memberNumTextBox.Text.Trim().Length == 16)
            {
                formMemeberNumCheck = true;
            }
            if (firstNameTextBox.Text.Trim() != "" && lastNameTextBox.Text.Trim() != "" && phoneNumTextBox.Text.Trim().Length > 6)
            {
                formPersonInfoCheck = true;
            }

            if (formPersonInfoCheck == true || formMemeberNumCheck == true)
            {
                return true;
            }
            else
            {
                Jeeves.ShowMessage("Error", "Enter either a CAA member number OR a first and last name along with a phone number");
                return false;
            }
        }

        /// <summary>
        /// This method ensures that the survey answers are completed before the form is submitted; returns "true" if it is
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// This method displays the last user-attendee swipe interaction that was made at a particular event in the view
        /// </summary>
        private void ShowLastSwipeInfo()
        {
            var lastSwipe = attendanceTrackingRepository.GetLastAttendanceTrackingByEvent(currentEvent.EventID);
            string personInfo = "";

            if (lastSwipe == null)
            {
                tbkLastSwipe.Text = "Last Entry: no available data";
            }
            else
            {
                if (lastSwipe.FirstName != "") personInfo += lastSwipe.FirstName;
                if (lastSwipe.LastName != "")
                {
                    if (personInfo == "") personInfo += lastSwipe.LastName;
                    else personInfo += " " + lastSwipe.LastName;
                }
                if (lastSwipe.MemberNo != "")
                {
                    if (personInfo == "") personInfo += "CAA Number: " + lastSwipe.MemberNo;
                    else personInfo += "  CAA Number: " + lastSwipe.MemberNo;
                }
                tbkLastSwipe.Text = "Last Entry:  " + personInfo;
            }
        }

        #endregion

    }
}