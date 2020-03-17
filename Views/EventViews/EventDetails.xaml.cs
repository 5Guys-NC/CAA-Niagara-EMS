
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
/******************************
*  Model Created By: Jon Yade
*  Edited By:  
*******************************/

namespace CAA_Event_Management.Views.EventViews
{
    public sealed partial class EventDetails : Page
    {
        #region Startup - variables, respositories, methods

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

        public EventDetails()
        {
            this.InitializeComponent();
            eventRepository = new EventRepository();
            eventItemRepository = new EventItemRepository();
            itemRepository = new ItemRepository();
            gameRepository = new GameRepository();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Event object preparation
            view = (Event)e.Parameter;
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

        private void SetTimes()
        {
            string[] start = view.EventStart.ToString().Split(" ");
            string[] end = view.EventEnd.ToString().Split(" ");

            string startTime = PrepOpeningTimes(start);
            string endTime = PrepOpeningTimes(end);

            tpEventStart.Time = TimeSpan.Parse(startTime);
            tpEventEnd.Time = TimeSpan.Parse(endTime);
        }

        private string PrepOpeningTimes(string[] timeArray)
        {
            string[] time = timeArray[1].Split(":");
            
            if (timeArray[2] == "PM" && time[0] != "12")
            {
                time[0] = (Convert.ToInt32(time[0]) + 12).ToString();
            }
            else if (timeArray[2] == "AM" && time[0] == "AM")
            {
                time[0] = "00";
            }
            return String.Join(":", time[0], time[1]);
        }

        #endregion

        #region Buttons - Event - Save, Delete, Cancel, and MemberCheckBox Methods

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckForProperDateUsage()) return;

            try
            {
                if(!AddEventDatesAndTimes()) return;  //this must be before BuildNamesForTheEvent()
                if(!BuildNamesForTheEvent()) return;

                if (membersOnlyCheck.IsChecked == true) view.MembersOnly = true;
                else view.MembersOnly = false;

                if (view.EventID == "0")
                {
                    view.EventID = Guid.NewGuid().ToString();
                    view.CreatedBy = userInfo.userAccountName;
                    view.LastModifiedBy = userInfo.userAccountName;
                    eventRepository.AddEvent(view);
                    NewAuditLine("Created by:" + userInfo.userAccountName + " Event:" + view.EventID + " " + view.AbrevEventname + " On Date: " + view.LastModifiedDate.ToString()
                                   + "To:" + view.DisplayName + " " + view.EventStart + " " + view.EventEnd + " Members Only:" + view.MembersOnly + " QuizID:" + view.QuizID);
                    SaveEventItemsToThisEvent();
                }
                else
                {
                    view.LastModifiedBy = userInfo.userAccountName;
                    view.LastModifiedDate = DateTime.Now;
                    eventRepository.UpdateEvent(view);
                    NewAuditLine("Modified(Edit) by:" + userInfo.userAccountName + " Event:" + view.EventID + " " + view.AbrevEventname + " On Date:" + view.LastModifiedDate.ToString()
                                    + " To:" + view.DisplayName + " " + view.EventStart + " " + view.EventEnd + " Members Only:" + view.MembersOnly + " QuizID:" + view.QuizID);
                    SaveEventItemsToThisEvent();
                }
                Frame.GoBack();
            }
            catch (Exception)
            {
                Jeeves.ShowMessage("Error", "Failed to save Event; please try again");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)   //This should be removed later as it will serve no prupose
        {
            try
            {
                view.LastModifiedBy = userInfo.userAccountName;
                view.LastModifiedDate = DateTime.Now;
                eventRepository.DeleteEvent(view);
                Frame.GoBack();
            }
            catch
            {
                Jeeves.ShowMessage("Error", "Failure to delete record; please try again");
            }
        }

        private void membersOnlyCheck_Checked(object sender, RoutedEventArgs e)
        {
            view.MembersOnly = true;
        }

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
                if (lstAvailableSurveyQuestions != null && questionCount < 5)
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

        private void SaveEventItemsToThisEvent()
        {
            string auditLine = "";
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
                        itemRepository.UpdateItemCount(x.EIDItemID, 1);
                        eventItemRepository.AddEventItem(eventItemToAdd);
                        if (auditLine == "")
                        {
                            auditLine = "Created by:" + userInfo.userAccountName + " EventItem:" + eventItemToAdd.EventItemID + ", " + x.EIDItemPhrase + " To:" + eventItemToAdd.EventID + " " + view.DisplayName + " On Date:" + view.LastModifiedDate;
                        }
                        else
                        {
                            auditLine += "\nCreated by:" + userInfo.userAccountName + " EventItem:" + eventItemToAdd.EventItemID + ", " + x.EIDItemPhrase + " To:" + eventItemToAdd.EventID + " " + view.DisplayName + " On Date:" + view.LastModifiedDate;
                        }
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
                        eventItemRepository.DeleteEventItem(x);
                        if (auditLine == "")
                        {
                            auditLine = "Deleted by:" + userInfo.userAccountName + " EventItem:" + x.EventItemID + " From:" + view.EventID + " " + view.DisplayName + " On Date:" + view.LastModifiedDate;
                        }
                        else
                        {
                            auditLine += "\nDeleted by:" + userInfo.userAccountName + " EventItem:" + x.EventItemID + " From:" + view.EventID + " " + view.DisplayName + " On Date:" + view.LastModifiedDate;
                        }
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
                            itemRepository.UpdateItemCount(x.EIDItemID, 1);
                            eventItemRepository.AddEventItem(eventItemToAdd);
                            if (auditLine == "")
                            {
                                auditLine = "Created by:" + userInfo.userAccountName + " EventItem:" + eventItemToAdd.EventItemID + ", " + x.EIDItemPhrase + " To:" + eventItemToAdd.EventID + " " + view.DisplayName + " On Date:" + view.LastModifiedDate;
                            }
                            else
                            {
                                auditLine += "\nCreated by:" + userInfo.userAccountName + " EventItem:" + eventItemToAdd.EventItemID + ", " + x.EIDItemPhrase + " To:" + eventItemToAdd.EventID + " " + view.DisplayName + " On Date:" + view.LastModifiedDate;
                            }
                        }
                        else if (checkForItem != null && !selectedItems.Contains(x))
                        {
                            itemRepository.UpdateItemCount(x.EIDItemID, -1);
                            eventItemRepository.DeleteEventItem(checkForItem);
                            if (auditLine == "")
                            {
                                auditLine = "Deleted by:" + userInfo.userAccountName + " EventItem:" + checkForItem.EventItemID + " " + x.EIDItemPhrase + " From:" + view.EventID + " " + view.DisplayName + " On Date:" + view.LastModifiedDate;
                            }
                            else
                            {
                                auditLine += "\nDeleted by:" + userInfo.userAccountName + " EventItem:" + checkForItem.EventItemID + " " + x.EIDItemPhrase + " From:" + view.EventID + " " + view.DisplayName + " On Date:" + view.LastModifiedDate;
                            }
                        }
                    }
                }
                catch
                {
                    Jeeves.ShowMessage("Error", "There was a problem...");
                }
            }
            NewAuditLine(auditLine);
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



        private bool CheckForProperDateUsage()
        {
            var eventStart = Convert.ToDateTime(eventStartDate.Date.ToString());
            var eventEnd = Convert.ToDateTime(cdpEventEnd.Date.ToString());
            if (eventStart > eventEnd)
            {
                Jeeves.ShowMessage("Error", "Please choose an end date that is after the start date");
                return false;
            }
            else if (eventStart < (DateTime.Now.AddDays(-2)))
            {
                Jeeves.ShowMessage("Error", "Please choose a start date in the future");
                return false;
            }
            return true;
        }

        #endregion

        #region Helper Methods - BuildNamesForTheEvent, AddDatesAndTimes, NewAuditLine

        private bool BuildNamesForTheEvent()
        {
            string[] eventNameArray = eventNameTextBox.Text.Trim().Split(' ');

            string tempEventNameString = "";
            for (int x = 0; x < eventNameArray.Length; x++)
            {
                if (eventNameArray[x] != "")
                {
                    tempEventNameString += eventNameArray[x].Substring(0, 1).ToUpper() + eventNameArray[x].Substring(1).ToLower() + " ";
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

        private bool AddEventDatesAndTimes()
        {
            string[] startDate = eventStartDate.Date.ToString().Split(" ");
            var startTime = tpEventStart.Time.ToString();
            DateTime start = Convert.ToDateTime(startDate[0] + " " + startTime);

            string[] endDate = cdpEventEnd.Date.ToString().Split(" ");
            var endTime = tpEventEnd.Time.ToString();
            DateTime end = Convert.ToDateTime(endDate[0] + " " + endTime);

            if (start > end)
            {
                Jeeves.ShowMessage("Error", "Please set a start date/time that is before the end date/time");
                return false;
            }

            view.EventStart = start;
            view.EventEnd = end;
            return true;
        }

        private void GetUserInfo()
        {
            userInfo = (App)Application.Current;
        }

        private void NewAuditLine(string newLine)
        {
            AuditLog line = new AuditLog();
            line.WriteToAuditLog(newLine);
        }

        #endregion
    }
}