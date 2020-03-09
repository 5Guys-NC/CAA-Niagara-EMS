
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
            view = (Event)e.Parameter;
            this.DataContext = view;

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
            var start = view.EventStart.ToString().Substring(view.EventStart.ToString().IndexOf(" ")+1);
            var end = view.EventEnd.ToString().Substring(view.EventStart.ToString().IndexOf(" ")+1);

            var start2 = start.Substring(4, 1);

            if (start.Substring(start.IndexOf(" ") + 1) == "PM")
            {
                int newTime = Convert.ToInt32(start.Substring(0, start.IndexOf(":")));
                if (newTime > 12) newTime += 12;

                if (newTime == 24) start = "00" + start.Substring(start.IndexOf(":"), 3);
                else start = newTime.ToString() + start.Substring(start.IndexOf(":"), 3);
            }
            else if (start.Substring(0, 2) == "12") start = "00" + start.Substring(start.IndexOf(":"), 3);
            else
            {
                if (start.Substring(4,1) == ":") start = start.Substring(0, 4);
                else start = start.Substring(0, 5);
            }

            if (end.Substring(end.IndexOf(" ")+1) == "PM")
            {
                int newTime = Convert.ToInt32(end.Substring(0, end.IndexOf(":")));
                if (newTime > 12) newTime += 12;
                
                if (newTime == 24) end = "00" + end.Substring(end.IndexOf(":") , 3);
                else end = newTime.ToString() + end.Substring(end.IndexOf(":"), 3);
            }
            else if (end.Substring(0, 2) == "12") end = "00" + end.Substring(end.IndexOf(":"), 3);
            else
            {
                if (end.Substring(4,1) == ":") end = end.Substring(0, 4);
                else end = end.Substring(0, 5);
            }

            tpEventStart.Time = TimeSpan.Parse(start);
            tpEventEnd.Time = TimeSpan.Parse(end);
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
                    App userInfo = (App)Application.Current;
                    view.CreatedBy = userInfo.userAccountName;
                    view.LastModifiedBy = userInfo.userAccountName;
                    eventRepository.AddEvent(view);
                    SaveEventItemsToThisEvent();
                }
                else
                {
                    App userInfo = (App)Application.Current;
                    view.LastModifiedBy = userInfo.userAccountName;
                    view.LastModifiedDate = DateTime.Now;
                    eventRepository.UpdateEvent(view);
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
            //if (newEvent == true)
            //{
            //    List<EventItem> itemsToDelete = eventItemRepository.GetEventItems(view.EventID);
            //    foreach (var x in itemsToDelete) eventItemRepository.DeleteEventItem(x);
            //    eventRepository.DeleteEventPermanent(view);
            //}
            Frame.GoBack();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)   //This should be removed later as it will serve no prupose
        {
            try
            {
                App userInfo = (App)Application.Current;
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

        #region Buttons - Survey Questions - Add, Remove

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
                //else
                //{
                //    Jeeves.ShowMessage("Error", "You are only allowed 5 questions per event");
                //}
            }
            catch
            {
                Jeeves.ShowMessage("Error", "You need to select a survey question to remove.");
            }
        }

        #endregion

        #region Helper Methods - Startup List Building methods and General Fill methods (Fill SurveyList, fill Games)

        private void FillListOfEventItemDetails()
        {
            try
            {
                List<Item> items = itemRepository.GetItems();
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
            if (insertMode == false)
            {
                try
                {
                    foreach (var x in selectedItems)
                    {
                        App userInfo = (App)Application.Current;
                        EventItem eventItemToAdd = new EventItem();
                        eventItemToAdd.EventID = view.EventID;
                        eventItemToAdd.EventItemID = Guid.NewGuid().ToString();
                        eventItemToAdd.ItemID = x.EIDItemID;
                        eventItemToAdd.CreatedBy = userInfo.userAccountName;
                        eventItemToAdd.LastModifiedBy = userInfo.userAccountName;
                        itemRepository.UpdateItemCount(x.EIDItemID, 1);
                        eventItemRepository.AddEventItem(eventItemToAdd);
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
                            App userInfo = (App)Application.Current;
                            EventItem eventItemToAdd = new EventItem();
                            eventItemToAdd.EventItemID = Guid.NewGuid().ToString();
                            eventItemToAdd.ItemID = x.EIDItemID;
                            eventItemToAdd.EventID = view.EventID;
                            eventItemToAdd.CreatedBy = userInfo.userAccountName;
                            eventItemToAdd.LastModifiedBy = userInfo.userAccountName;
                            itemRepository.UpdateItemCount(x.EIDItemID, 1);
                            eventItemRepository.AddEventItem(eventItemToAdd);
                        }
                        else if (checkForItem != null && !selectedItems.Contains(x))
                        {
                            itemRepository.UpdateItemCount(x.EIDItemID, -1);
                            eventItemRepository.DeleteEventItem(checkForItem);
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


        private void CheckForDatesOnNames()
        {
            string eventDate = view.EventStart.HasValue ? view.EventStart.Value.Year.ToString() : "0000";
            string viewEnd = view.DisplayName.Substring(view.DisplayName.Length - 4, 4);

            if (eventDate != viewEnd)
            {
                view.EventName = view.EventName + eventDate;
                view.AbrevEventname = view.AbrevEventname + eventDate;
            }
        }


        private bool AddEventDatesAndTimes()
        {
            var startDate = eventStartDate.Date.ToString().Substring(0, eventStartDate.Date.ToString().IndexOf(" "));
            var startTime = tpEventStart.Time.ToString();

            var endDate = cdpEventEnd.Date.ToString().Substring(0, cdpEventEnd.Date.ToString().IndexOf(" "));
            var endTime = tpEventEnd.Time.ToString();

            DateTime start = Convert.ToDateTime(startDate + " " + startTime);
            DateTime end = Convert.ToDateTime(endDate + " " + endTime);

            if (start > end)
            {
                Jeeves.ShowMessage("Error", "Please set a start date/time that is before the end date/time");
                return false;
            }

            view.EventStart = start;
            view.EventEnd = end;

            return true;
        }


        private void lstAvailableQuizzes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var choosenGame = (Game)lstAvailableQuizzes.SelectedItem;
            view.QuizID = choosenGame.ID;
            CheckForSelectedQuiz();
        }

        private void btnClearQuiz_Click(object sender, RoutedEventArgs e)
        {
            view.QuizID = null;
            CheckForSelectedQuiz();
        }
    }
}