
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CAA_Event_Management.Data;
using CAA_Event_Management.Models;
using CAA_Event_Management.Utilities;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using System.ComponentModel.DataAnnotations;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using CAA_Event_Management.ViewModels;
using System.Threading.Tasks;
using CAA_Event_Management.Data.Interface_Repos;
using CAA_Event_Management.Data.Repos;

/******************************
*  Model Created By: Jon Yade
*  Edited By:  
*******************************/

namespace CAA_Event_Management.Views.EventViews
{
    public sealed partial class EventDetails : Page
    {
        #region Startup - variables, respositories, methods

        //General variables
        Models.Event startView;
        Models.Event view;
        private int questionCount = 0;
        private bool insertMode = true;
        private App userInfo; 

        //Lists for selected EventItems
        List<EventItemDetails> listOfEventItemsDetails = new List<EventItemDetails>();
        List<EventItemDetails> selectedItems = new List<EventItemDetails>();
        List<EventItemDetails> availableItems = new List<EventItemDetails>();

        //Lists of Repositories
        IEventRepository eventRepository;
        IItemRepository itemRepository;
        IEventItemRepository eventItemRepository;
        IGameRepository gameRepository;
        IModelAuditLineRepository auditLineRepository;

        public EventDetails()
        {
            this.InitializeComponent();
            eventRepository = new EventRepository();
            eventItemRepository = new EventItemRepository();
            itemRepository = new ItemRepository();
            gameRepository = new GameRepository();
            auditLineRepository = new ModelAuditLineRepository();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Event object preparation
            view = (Event)e.Parameter;
            startView = eventRepository.GetEvent(view.EventID);
            this.DataContext = view;
            GetUserInfo();

            if (view.EventID == "0")
            {
                //btnDelete.IsEnabled = false;
                insertMode = false;
            }
            else if (view.MembersOnly == true)
            {
                membersOnlyCheck.IsChecked = true;
            }

            //Initial EventItem List Preparation Methods
            FillListOfEventItemDetails();
            InitialDeterminationOfEventItemAssignment();
            SetTimes();
            CheckForSelectedQuiz();

            //General Fill Methods
            FillGameField();
            FillSurveySelectionLists();
        }

        /// <summary>
        /// This method builds the initial start and end times of the selected event and sets them in the XAML display for this view (EventDetails.xaml)
        /// </summary>
        private void SetTimes()
        {
            string[] start = view.EventStart.ToString().Split(" ");
            string[] end = view.EventEnd.ToString().Split(" ");

            string startTime = PrepOpeningTimes(start);
            string endTime = PrepOpeningTimes(end);

            tpEventStart.Time = TimeSpan.Parse(startTime);
            tpEventEnd.Time = TimeSpan.Parse(endTime);
        }

        /// <summary>
        /// This method receives an array of strings from the "SetTimes" method (broken apart by spaces). It then takes the time portions of the array and converts them to XAML time which is returned as a string
        /// </summary>
        /// <param name="timeArray">This parameter contains an array of the dateTime object broken down by spaces (" ") from "SetTimes" method</param>
        /// <returns></returns>
        private string PrepOpeningTimes(string[] timeArray)
        {
            string[] time = timeArray[1].Split(":");
            
            if (timeArray[2] == "PM" && time[0] != "12")
            {
                time[0] = (Convert.ToInt32(time[0]) + 12).ToString();
            }
            else if (timeArray[2] == "AM" && time[0] == "12")
            {
                time[0] = "00";
            }
            return String.Join(":", time[0], time[1]);
        }

        #endregion

        #region Buttons - Event - btnSave, btnCancel, and MemberCheckBox Methods

        /// <summary>
        /// This method saves creates/edits of an event and passes all the changes to the database
        /// </summary>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckForProperDateUsage()) return;

            try
            {
                if(!AddEventDatesAndTimes()) return;  //this must be before BuildNamesForTheEvent()
                if(!BuildNamesForTheEvent()) return;

                if(eventNameTextBox.Text.Length > 150)
                {
                    Jeeves.ShowMessage("Error", "Please shorten the event name as it cannot exceed 150 characters");
                    return;
                }

                if (membersOnlyCheck.IsChecked == true) view.MembersOnly = true;
                else view.MembersOnly = false;

                if (view.EventID == "0")
                {
                    view.EventID = Guid.NewGuid().ToString();
                    view.CreatedBy = userInfo.userAccountName;
                    view.LastModifiedBy = userInfo.userAccountName;
                    eventRepository.AddEvent(view);
                    string thisEventDetails =  view.DisplayName + " (" + view.AbrevEventname + ") " + view.EventStart + " " + view.EventEnd + " Members Only:" + view.MembersOnly + " QuizID:" + view.QuizID;
                    WriteNewAuditLineToDatabase(view.LastModifiedBy, "Event Table", view.EventID, thisEventDetails, view.LastModifiedDate.ToString(),"Create", "Event Creation - No Changes");
                    SaveEventItemsToThisEvent();
                }
                else
                {
                    if(!startView.Equals(view))
                    {
                        view.LastModifiedBy = userInfo.userAccountName;
                        view.LastModifiedDate = DateTime.Now;
                        eventRepository.UpdateEvent(view);
                        string recordChanges = ShowObjectDifferences();
                        string thisEventDetails = view.DisplayName + " (" + view.AbrevEventname + ") " + view.EventStart + " " + view.EventEnd + " Members Only:" + view.MembersOnly + " QuizID:" + view.QuizID;
                        WriteNewAuditLineToDatabase(view.LastModifiedBy, "Event Table", view.EventID, thisEventDetails, view.LastModifiedDate.ToString(),"Edit",recordChanges);
                    }
                    SaveEventItemsToThisEvent();
                }
                Frame.Navigate(typeof(CAAEvents));
            }
            catch (Exception)
            {
                Jeeves.ShowMessage("Error", "Failed to save Event; please try again");
            }
        }

        /// <summary>
        /// This method handles the user Cancel button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CAAEvents));
            //Frame.GoBack();
        }

        /// <summary>
        /// This method handles the clicking of the "Members Only" checkbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void membersOnlyCheck_Checked(object sender, RoutedEventArgs e)
        {
            view.MembersOnly = true;
        }

        /// <summary>
        /// This method handles the unclicking of the "Members Only" checkbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void membersOnlyCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            view.MembersOnly = false;
        }

        #endregion

        #region Buttons - Survey Questions/Quiz - Add, Remove, ClearQuiz, Quiz_SelectionChanged

        private void btnAddQuestion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstAvailableSurveyQuestions != null && questionCount < 10)
                {
                    EventItemDetails selectedItem = (EventItemDetails)lstAvailableSurveyQuestions.SelectedItem;
                    EventItemDetails changeItem = listOfEventItemsDetails
                        .Where(d => d.EIDItemID == selectedItem.EIDItemID)
                        .FirstOrDefault();
                    changeItem.EIDItemAssigned = true;
                    questionCount++;
                    FillSurveySelectionLists();
                }
                else
                {
                    Jeeves.ShowMessage("Error", "You are only allowed 5 questions per event");
                }
            }
            catch
            {
                Jeeves.ShowMessage("Error", "You need to select a survey question to add.");
            }
        }

        private void btnRemoveQuestion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstAvailableSurveyQuestions != null)
                {
                    EventItemDetails selectedItem = (EventItemDetails)lstSelectedSurveyQuestions.SelectedItem;
                    EventItemDetails changeItem = listOfEventItemsDetails
                        .Where(d => d.EIDItemID == selectedItem.EIDItemID)
                        .FirstOrDefault();
                    changeItem.EIDItemAssigned = false;
                    questionCount--;
                    FillSurveySelectionLists();
                }
            }
            catch
            {
                Jeeves.ShowMessage("Error", "You need to select a survey question to remove.");
            }
        }

        private void btnClearQuiz_Click(object sender, RoutedEventArgs e)
        {
            view.QuizID = null;
            CheckForSelectedQuiz();
        }

        private void lstAvailableQuizzes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var choosenGame = (Game)lstAvailableQuizzes.SelectedItem;
            view.QuizID = choosenGame.ID;
            CheckForSelectedQuiz();
        }

        #endregion

        #region Helper Methods - Startup List Building methods and General Fill methods (Fill SurveyList, fill Games)

        /// <summary>
        /// This method creates the EventItemDetails viewModel which ties each EventItem to the Survey item information
        /// related to it (eg. ItemName, ItemID)
        /// </summary>
        private void FillListOfEventItemDetails()
        {
            try
            {
                List<Item> items = itemRepository.GetUndeletedItems();
                foreach (var x in items)
                {
                    EventItemDetails newEventItem = new EventItemDetails();
                    newEventItem.EIDItemID = x.ItemID;
                    newEventItem.EIDItemPhrase = x.ItemName;
                    newEventItem.EIDItemAssigned = false;

                    listOfEventItemsDetails.Add(newEventItem);
                }
            }
            catch (Exception ex)
            {
                Jeeves.ShowMessage("Error", ex.GetBaseException().Message.ToString());
            }
        }

        /// <summary>
        /// This method determines if there is a selected quiz. If there is, it updates the EventDetails view
        /// </summary>
        private void CheckForSelectedQuiz()
        {
            if (view.QuizID == null)
            {
                tbChosenQuiz.Text = "Selected Quiz: None";
                btnClearQuiz.Visibility = Visibility.Collapsed;
            }
            else
            {
                try
                {
                    var quiz = gameRepository.GetGame((int)view.QuizID);
                    tbChosenQuiz.Text = "Selected Quiz: " + quiz.Title;
                    btnClearQuiz.Visibility = Visibility.Visible;
                }
                catch
                {
                    Jeeves.ShowMessage("Error", "The quiz failed to load");
                }
            }
        }

        /// <summary>
        /// When the page is navigated to, this method determines what the survey questions (EventItems) are tied to the event
        /// and what survey questions (EventItems) are not, it also sets the current number of selected survey items in the event
        /// </summary>
        private void InitialDeterminationOfEventItemAssignment()
        {
            try
            {
                List<EventItem> eventItems;

                if (insertMode == true)
                {
                    eventItems = eventItemRepository.GetEventItems(view.EventID);

                    foreach (var x in eventItems)
                    {
                        var selectedItem = listOfEventItemsDetails
                            .Where(d => d.EIDItemID == x.ItemID)
                            .FirstOrDefault();
                        selectedItem.EIDItemAssigned = true;
                        questionCount++;
                    }
                }
            }
            catch (Exception ex)
            {
                Jeeves.ShowMessage("Error", ex.GetBaseException().Message.ToString());
            }
        }

        /// <summary>
        /// This method fills both of the Survey question list boxes by placing selected questions in one list and available quesitons in the other
        /// </summary>
        private void FillSurveySelectionLists()
        {
            selectedItems = listOfEventItemsDetails
                .Where(d => d.EIDItemAssigned == true)
                .ToList();

            availableItems = listOfEventItemsDetails
                .Where(d => d.EIDItemAssigned == false)
                .ToList();

            lstSelectedSurveyQuestions.ItemsSource = selectedItems;
            lstAvailableSurveyQuestions.ItemsSource = availableItems;
        }

        /// <summary>
        /// This method fills the appropriate list box with all of the available quizzes
        /// </summary>
        private void FillGameField()
        {
            try
            {
                List<Game> games = gameRepository.GetGames();
                lstAvailableQuizzes.ItemsSource = games;
            }
            catch
            {
                Jeeves.ShowMessage("Error", "There was a problem retrieving game information");
            }
        }

        #endregion

        #region Helper Methods - SaveEventItems, TextSearchBox, CheckForDatesOnNames

        /// <summary>
        /// This method creates and deletes the EventItem survey questions on the current event.
        /// </summary>
        private void SaveEventItemsToThisEvent()
        {
            if (insertMode == false)
            {
                try
                {
                    foreach (var x in selectedItems)
                    {
                        EventItem eventItemToAdd = new EventItem();
                        eventItemToAdd.EventID = view.EventID;
                        eventItemToAdd.EventItemID = Guid.NewGuid().ToString();
                        eventItemToAdd.ItemID = x.EIDItemID;
                        eventItemToAdd.CreatedBy = userInfo.userAccountName;
                        eventItemToAdd.LastModifiedBy = userInfo.userAccountName;
                        eventItemToAdd.LastModifiedDate = DateTime.Now;
                        itemRepository.UpdateItemCount(x.EIDItemID, 1);
                        eventItemRepository.AddEventItem(eventItemToAdd);
                        WriteNewAuditLineToDatabase(eventItemToAdd.CreatedBy, "EventItem Table", "Survey Question: " + eventItemToAdd.EventItemID, x.EIDItemPhrase, eventItemToAdd.LastModifiedDate.ToString(), "Create", "EventItem - Creation - No Changes");
                    }
                }
                catch
                {
                    Jeeves.ShowMessage("Error", "There was a proble...");
                }
            }
            else if (selectedItems.Count == 0)
            {
                try
                {
                    List<EventItem> currentSelectedItems = eventItemRepository.GetEventItems(view.EventID);
                    foreach (var x in currentSelectedItems)
                    {
                        itemRepository.UpdateItemCount(x.ItemID, -1);
                        string itemPhrase = itemRepository.GetItem(x.ItemID).ItemName;
                        x.LastModifiedBy = userInfo.userAccountName;
                        x.LastModifiedDate = DateTime.Now;
                        eventItemRepository.DeleteEventItem(x);
                        WriteNewAuditLineToDatabase(x.LastModifiedBy,"EventItem Table", "Survey Question: " + x.EventItemID, "Survey Question: " + itemPhrase, x.LastModifiedDate.ToString(),"Delete", "EventItem - Manual Deletion - No Change");
                    }
                }
                catch
                {
                    Jeeves.ShowMessage("Error", "There was a problem...");
                }
            }
            else
            {
                try
                {
                    List<EventItem> currentSelectedItems = eventItemRepository.GetEventItems(view.EventID);

                    foreach (var x in listOfEventItemsDetails)
                    {
                        EventItem checkForItem = currentSelectedItems
                            .Where(c => c.ItemID == x.EIDItemID)
                            .FirstOrDefault();

                        if (selectedItems.Contains(x) && checkForItem == null)
                        {
                            EventItem eventItemToAdd = new EventItem();
                            eventItemToAdd.EventItemID = Guid.NewGuid().ToString();
                            eventItemToAdd.ItemID = x.EIDItemID;
                            eventItemToAdd.EventID = view.EventID;
                            eventItemToAdd.CreatedBy = userInfo.userAccountName;
                            eventItemToAdd.LastModifiedBy = userInfo.userAccountName;
                            eventItemToAdd.LastModifiedDate = DateTime.Now;
                            itemRepository.UpdateItemCount(x.EIDItemID, 1);
                            eventItemRepository.AddEventItem(eventItemToAdd);
                            WriteNewAuditLineToDatabase(eventItemToAdd.CreatedBy, "EventItem Table", eventItemToAdd.EventItemID, "Survey Question: " + x.EIDItemPhrase, eventItemToAdd.LastModifiedDate.ToString(), "Create", "EventItem - Creation - No Changes");
                        }
                        else if (checkForItem != null && !selectedItems.Contains(x))
                        {
                            itemRepository.UpdateItemCount(x.EIDItemID, -1);
                            checkForItem.LastModifiedBy = userInfo.userAccountName;
                            checkForItem.LastModifiedDate = DateTime.Now;
                            eventItemRepository.DeleteEventItem(checkForItem);
                            WriteNewAuditLineToDatabase(checkForItem.LastModifiedBy, "EventItem Table", checkForItem.EventItemID, "Survey Question: " + x.EIDItemPhrase, checkForItem.LastModifiedDate.ToString(), "Delete", "EventItem - Manual Deletion - No Change");
                        }
                    }
                }
                catch
                {
                    Jeeves.ShowMessage("Error", "There was a problem...");
                }
            }
        }

        private void txtSearchIcon_Click(object sender, RoutedEventArgs e)
        {
            if (tbSearch.Visibility == Visibility.Collapsed && txtSearchBox.Visibility == Visibility.Collapsed)
            {
                tbSearch.Visibility = Visibility.Visible;
                txtSearchBox.Visibility = Visibility.Visible;
                txtSearchBox.Focus(FocusState.Programmatic);
            }
            else
            {
                tbSearch.Visibility = Visibility.Collapsed;
                txtSearchBox.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// This method handles the search box text changed event as the user searchs for survey questions, and the list with the newly selected survey questions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtSearchBox.Text == "") FillSurveySelectionLists();
            else
            {
                try
                {
                    List<EventItemDetails> searchResultEventItemDetails = new List<EventItemDetails>();
                    string searchString = txtSearchBox.Text.ToLower();

                    foreach (var x in availableItems)
                    {
                        if (x.EIDItemPhrase.ToLower().IndexOf(searchString) > -1)
                        {
                            searchResultEventItemDetails.Add(x);
                        }
                    }
                    lstAvailableSurveyQuestions.ItemsSource = searchResultEventItemDetails;
                }
                catch (Exception)
                {
                    Jeeves.ShowMessage("Error", "There was an error in retreving the questions");
                }
            }
        }

        /// <summary>
        /// This method ensures that the user has properly set the start and end dates/times of the event.
        /// It checks to make sure that the start date is before the end date and that the start date is not more than 2 days in the past.
        /// It returns a false bool if the dates/times are not properly set, and a true if they are correctly set
        /// </summary>
        /// <returns>If the dates are correctly set by the user, this function returns "true", else it returns "false"</returns>
        private bool CheckForProperDateUsage()
        {
            //var eventStart = Convert.ToDateTime(eventStartDate.Date.ToString());
            //var eventEnd = Convert.ToDateTime(cdpEventEnd.Date.ToString());
            string[] startDate = eventStartDate.Date.ToString().Split(" ");
            var startTime = tpEventStart.Time.ToString();
            DateTime start = Convert.ToDateTime(startDate[0] + " " + startTime);

            string[] endDate = cdpEventEnd.Date.ToString().Split(" ");
            var endTime = tpEventEnd.Time.ToString();
            DateTime end = Convert.ToDateTime(endDate[0] + " " + endTime);

            if (start > end)
            {
                Jeeves.ShowMessage("Error", "Please choose an end date that is after the start date");
                return false;
            }
            else if (start < (DateTime.Now.AddDays(-2)))
            {
                Jeeves.ShowMessage("Error", "Please choose a start date in the future");
                return false;
            }
            return true;
        }

        #endregion

        #region Helper Methods - BuildNamesForTheEvent, AddDatesAndTimes, NewAuditLine, WriteNewAuditLineToDatabase

        /// <summary>
        /// This method takes the event name provided by the user (the DisplayName) and make sure that the name provided 
        /// has all of the words in capital case, with the year attached to the end (if not initially added by the user)
        /// It also builds the EventName and the AbreviatedName for the database. It also ensures that the AbreviatedName
        /// is less than 20 characters per database requirements
        /// </summary>
        /// <returns>Returns a "true" if all the names are valid and of an exceptable length; "false" if they are not valid</returns>
        private bool BuildNamesForTheEvent()
        {
            string[] eventNameArray = eventNameTextBox.Text.Trim().Split(' ');

            string tempEventNameString = "";
            for (int x = 0; x < eventNameArray.Length; x++)
            {
                if (eventNameArray[x] != "")
                {
                    if (eventNameArray[x].ToUpper() == "CAA" || eventNameArray[x].ToUpper() == "BBQ")
                    {
                        tempEventNameString += eventNameArray[x].ToUpper() + " ";
                    }
                    else
                    {
                        tempEventNameString += eventNameArray[x].Substring(0, 1).ToUpper() + eventNameArray[x].Substring(1).ToLower() + " ";
                    }
                }
            }
            tempEventNameString = tempEventNameString.Trim();

            var startString = string.Format("{0:yyyy-MM-dd}", view.EventStart);

            if (tempEventNameString.Substring(tempEventNameString.Length - 4) != startString.Substring(0, 4))
            {
                tempEventNameString += " " + startString.Substring(0, 4);
            }
            view.DisplayName = tempEventNameString;

            string[] eventDisplayNameArray = tempEventNameString.Split(' ');
            view.EventName = string.Join("", eventDisplayNameArray);

            string eventAbbreviateName = "";
            foreach (string x in eventDisplayNameArray) eventAbbreviateName += x.Substring(0, 1).ToUpper();
            eventAbbreviateName = eventAbbreviateName.Remove(eventAbbreviateName.Length - 1, 1);
            eventAbbreviateName += startString.Substring(5, 2) + startString.Substring(0, 4);
            if (eventAbbreviateName.Length > 20)
            {
                Jeeves.ShowMessage("Error", "Please reduce the number of words or hyphens in your event name");
                return false;
            }
            view.AbrevEventname = eventAbbreviateName;
            return true;
        }

        /// <summary>
        /// This function actually sets the EventStart and EventEnd dates/times to a particular event it returns a false 
        /// if there is a problem setting them
        /// </summary>
        /// <returns>Returns a bool based on the success of the method; "true" represents successful method completion</returns>
        private bool AddEventDatesAndTimes()
        {
            string[] startDate = eventStartDate.Date.ToString().Split(" ");
            var startTime = tpEventStart.Time.ToString();
            DateTime start = Convert.ToDateTime(startDate[0] + " " + startTime);

            string[] endDate = cdpEventEnd.Date.ToString().Split(" ");
            var endTime = tpEventEnd.Time.ToString();
            DateTime end = Convert.ToDateTime(endDate[0] + " " + endTime);

            view.EventStart = start;
            view.EventEnd = end;
            return true;
        }

        /// <summary>
        /// This method runs on start-up and gives an instance of the Application object (App.xaml) to the userInfo object which is then available to all the methods throughout the class
        /// </summary>
        private void GetUserInfo()
        {
            userInfo = (App)Application.Current;
        }

        /// <summary>
        /// This method is part of the record auditing and determines what changes have been made to the record by the user.
        /// It then assembles a string of both the initial value and the new, updated value which is returned back to the 
        /// calling method.
        /// </summary>
        /// <returns>A string of all the changes to a record</returns>
        private string ShowObjectDifferences()
        {
            string differences = "";

            if (startView.DisplayName != view.DisplayName)
            {
                differences += "DisplayName change: " + startView.DisplayName + " TO: " + view.DisplayName;
            }
            if (startView.EventStart != view.EventStart)
            {
                if (differences != "") differences += " | ";
                differences += "EventSart change: " + startView.EventStart.ToString() + " TO: " + view.EventStart.ToString();
            }
            if (startView.EventEnd != view.EventEnd)
            {
                if (differences != "") differences += " | ";
                differences += "EventEnd change: " + startView.EventEnd.ToString() + " TO: " + view.EventEnd.ToString();
            }
            if (startView.MembersOnly != view.MembersOnly)
            {
                if (differences != "") differences += " | ";
                differences += "MembersOnly change: " + startView.MembersOnly.ToString() + " TO: " + view.MembersOnly.ToString();
            }
            if (startView.QuizID != view.QuizID)
            {
                if (differences != "") differences += " | ";
                differences += "Quiz(ID) change: " + startView.QuizID + " TO: " + view.QuizID;
            }
            return differences;
        }

        private async Task NewAuditLine(string newLine)
        {
            AuditLog line = new AuditLog();
            await line.WriteToAuditLog(newLine);
        }

        /// <summary>
        /// This method builds a ModelAuditLine Object and passes it on, via the respository, to be written in the ModelAuditLine database table
        /// </summary>
        /// <param name="userName">This parameter is the name of the logged-in user who made the changes to the record</param>
        /// <param name="objectTable">This parameter is the table in which the change was made</param>
        /// <param name="typeID">This parameter is the unique ID of the object. It is a global unique GUID and the ModelAuditLine table can be searched for it in order to find all of the occurances of this object</param>
        /// <param name="newTypeInfo">This parameter carries a complete record of the new (current) state of the oject that got changed</param>
        /// <param name="changeDate">This parameter carries the date and time of the changes to the object</param>
        /// <param name="changeType">This parameter describes the nature of the change itself, whether it is a Create, an Edit, or a Delete</param>
        /// <param name="changeInfo">This paramter records each of the specific changes to the object (where applicable), and shows both the original property information as well as the new changed property information of the object</param>
        private void WriteNewAuditLineToDatabase(string userName, string objectTable, string typeID, string newTypeInfo, string changeDate, string changeType, string changeInfo)
        {
            try
            {
                var newLine = new ModelAuditLine()
                {
                    ID = Guid.NewGuid().ToString(),
                    AuditorName = userName,
                    ObjectTable = objectTable,
                    ObjectID = typeID,
                    NewObjectInfo = newTypeInfo,
                    DateTimeOfChange = changeDate,
                    TypeOfChange = changeType,
                    ChangedFieldValues = changeInfo
                };
                auditLineRepository.AddModelAuditLine(newLine);
            }
            catch
            {
                Jeeves.ShowMessage("Error", "Failure to update audit log; please contact adminstrator");
            }
        }

        #endregion
    }
}