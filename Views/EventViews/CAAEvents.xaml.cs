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
using CAA_Event_Management.Data;
using CAA_Event_Management.Models;
using CAA_Event_Management.Views;
using Windows.UI.Xaml.Media.Animation;
using CAA_Event_Management.Views.EventViews;
using CAA_Event_Management.Utilities;
using CAA_Event_Management.Data.Interface_Repos;
using CAA_Event_Management.Data.Repos;
/******************************
*  Model Created By: Jon Yade
*  Edited by: Nathan Smith
*******************************/

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
namespace CAA_Event_Management.Views.EventViews
{
    /// <summary>
    /// Frame for Events
    /// </summary>
    public sealed partial class CAAEvents : Page
    {
        #region Startup - variables. repositories, methods

        int CurrentOrPast = 1;
        int deleteMode = 0;
        int daysUntilEventAutoDelete = 7;

        IEventRepository eventRepository;
        IModelAuditLineRepository auditLineRepository;

        public CAAEvents()
        {
            this.InitializeComponent();
            eventRepository = new EventRepository();
            auditLineRepository = new ModelAuditLineRepository();
            FillDropDown(1);

            ((Window.Current.Content as Frame).Content as MainPage).ChangeMainPageTitleName("GENERAL EVENT MANAGEMENT");
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                try
                {
                    deleteMode = (int)e.Parameter;
                }
                catch { }
            }
            if (deleteMode == 1) DeleteModeToggle();

            bool deleted = false;
            List<Models.Event> newList = eventRepository.GetEvents(deleted)
                .Where(p => p.IsDeleted == false && p.EventEnd < DateTime.Now.AddDays(-daysUntilEventAutoDelete))
                .ToList();
            if(newList.Count > 0) RemoveOldEvents(newList);
        }

        #endregion

        #region Buttons - CRUD - NewEvent, Event Display Toggle, SelectedEvent(edit)

        private void NewEvent_Click(object sender, RoutedEventArgs e)
        {
            Models.Event newEvent = new Models.Event();
            Frame.Navigate(typeof(EventDetails), newEvent, new SuppressNavigationTransitionInfo());
        }

        private void btnCurrentEvents_Click(object sender, RoutedEventArgs e)
        {
            Canvas.SetZIndex(btnCurrentEvents, 2);
            Canvas.SetZIndex(btnPastEvents, -1);
            CurrentOrPast = 1;
            FillDropDown(1);
        }

        private void btnPastEvents_Click(object sender, RoutedEventArgs e)
        {
            Canvas.SetZIndex(btnPastEvents, 2);
            Canvas.SetZIndex(btnCurrentEvents, -1);
            CurrentOrPast = 2;
            FillDropDown(2);
        }

        private void btnDeleteMode_Click(object sender, RoutedEventArgs e)
        {
            DeleteModeToggle();
        }

        #endregion

        #region Buttons - Adding: Attendees, Items, EventItems; btnChooseWinner

        /// <summary>
        /// This method handles the button click event for the actual starting of an event and changes 
        /// the view to AttendanceTracking for the selected event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRegisterAttendance_Click(object sender, RoutedEventArgs e)
        {
            if (gdvEditEvents.SelectedItem != null)
            {
                Event selectedEvent = (Event)gdvEditEvents.SelectedItem;
                Frame.Navigate(typeof(EventAttendanceTracking), (Event)selectedEvent, new SuppressNavigationTransitionInfo());
            }
            else if (gdvDeleteEvents.SelectedItem != null)
            {
                Event selectedEvent = (Event)gdvDeleteEvents.SelectedItem;
                Frame.Navigate(typeof(EventAttendanceTracking), (Event)selectedEvent, new SuppressNavigationTransitionInfo());
            }
            else
            {
                Jeeves.ShowMessage("Error", "Please select an active event");
            }
        }

        private void BtnCancel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(CAAEvents), deleteMode, new SuppressNavigationTransitionInfo());
        }

        /// <summary>
        /// This method handles the click event for event deletion and updates the audit table when that is done by the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnConfirmRemove_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string selected = (((Button)sender).DataContext).ToString();
            try
            {
                Event selectedEvent = new Event();
                selectedEvent = eventRepository.GetEvent(selected);
                App userInfo = (App)Application.Current;
                selectedEvent.LastModifiedBy = userInfo.userAccountName;
                selectedEvent.LastModifiedDate = DateTime.Now;
                selectedEvent.IsDeleted = true;
                eventRepository.UpdateEvent(selectedEvent);
                string thisEventDetails = selectedEvent.DisplayName + " (" + selectedEvent.AbrevEventname + ") " + selectedEvent.EventStart + " " + selectedEvent.EventEnd + " Members Only:" + selectedEvent.MembersOnly + " QuizID:" + selectedEvent.QuizID;
                WriteNewAuditLineToDatabase(selectedEvent.LastModifiedBy, "Event Table", selectedEvent.EventID, thisEventDetails, selectedEvent.LastModifiedDate.ToString(), "Delete", "Event - Manual Delete - 'IsDeleted' to 'true'");
                Frame.Navigate(typeof(CAAEvents), deleteMode, new SuppressNavigationTransitionInfo());
            }
            catch
            {
                Jeeves.ShowMessage("Error", "There was a problem deleting the selected record");
                //Jeeves.ShowMessage("Error", test);
            }
        }

        private void btnEditEvent_Click(object sender, RoutedEventArgs e)
        {
            string selected = (((Button)sender).DataContext).ToString();
            try
            {
                Event selectedEvent = new Event();
                selectedEvent = eventRepository.GetEvent(selected.ToString());
                Frame.Navigate(typeof(EventDetails), (Models.Event)selectedEvent, new SuppressNavigationTransitionInfo());
            }
            catch
            {
                Jeeves.ShowMessage("Error", "There was a problem editing the selected record");
            }
        }

        private void btnChooseWinner_Click(object sender, RoutedEventArgs e)
        {
            if (gdvEditEvents.SelectedItem != null)
            {
                var selectedEvent = gdvEditEvents.SelectedItem;
                Frame.Navigate(typeof(EventWinner), (Models.Event)selectedEvent);
            }
            else if (gdvDeleteEvents.SelectedItem != null)
            {
                var selectedEvent = gdvEditEvents.SelectedItem;
                Frame.Navigate(typeof(EventWinner), (Models.Event)selectedEvent);
            }
            else Jeeves.ShowMessage("Error", "Please select an event first");
        }

        #endregion

        #region Helper Methods - FillDropDown, SearchBox_TextChanged, DeleteModeToggle

        /// <summary>
        /// This methods fills the CAAEvent view with events based on whether or not the events are in the past or the future
        /// </summary>
        /// <param name="check"></param>
        private void FillDropDown(int check)
        {
            DateTime now = DateTime.Today.AddHours(-5);

            try
            {
                bool deleted = false;
                List<Models.Event> noDeletedEvents = eventRepository.GetEvents(deleted);

                if (check == 1)
                {
                    List<Models.Event> upcomingEvents = noDeletedEvents
                        .Where(c => c.EventEnd >= now)
                        .OrderBy(c => c.EventStart)
                        .ToList();
                    gdvEditEvents.ItemsSource = upcomingEvents;
                    gdvDeleteEvents.ItemsSource = upcomingEvents;
                }
                else
                {
                    List<Models.Event> pastEvents = noDeletedEvents
                        .Where(c => c.EventEnd < now)
                        .OrderByDescending(c => c.EventStart)
                        .ToList();
                    gdvEditEvents.ItemsSource = pastEvents;
                    gdvDeleteEvents.ItemsSource = pastEvents;
                }
            }
            catch (Exception e)
            {
                Jeeves.ShowMessage("Error", e.Message.ToString());
            }
        }

        /// <summary>
        /// This method automatically removes old events and auto updates the audit table in the process
        /// </summary>
        /// <param name="list">This parameter is a list of all old events</param>
        private void RemoveOldEvents(List<Models.Event> list)
        {
            foreach (var x in list)
            {
                try
                {
                    x.LastModifiedBy = "System Automatic Delete";
                    x.LastModifiedDate = DateTime.Now;
                    x.IsDeleted = true;
                    eventRepository.UpdateEvent(x);
                    string thisEventDetails = x.DisplayName + " (" + x.AbrevEventname + ") " + x.EventStart + " " + x.EventEnd + " Members Only:" + x.MembersOnly + " QuizID:" + x.QuizID;
                    WriteNewAuditLineToDatabase(x.LastModifiedBy, "Event Table", x.EventID, thisEventDetails, x.LastModifiedDate.ToString(), "Delete", "Event - System Automatic Delete - 'IsDeleted' to 'true'");
                }
                catch { }
            }
        }

        /// <summary>
        /// This method handles the search box text changed event as the user searchs for events, and gives the view the newly selected events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtSearch.Text == "") FillDropDown(CurrentOrPast);
            else
            {
                try
                {
                    bool deleted = false;
                    List<Event> thisEvent = eventRepository.GetEvents(deleted);
                    List<Event> searchEvents = new List<Event>();
                    string searchString = txtSearch.Text.ToLower();

                    foreach (var x in thisEvent)
                    {
                        if (x.DisplayName.ToLower().IndexOf(searchString) > -1)
                        {
                            searchEvents.Add(x);
                        }
                    }
                    gdvEditEvents.ItemsSource = searchEvents;
                    gdvDeleteEvents.ItemsSource = searchEvents;
                }
                catch (Exception)
                {
                    Jeeves.ShowMessage("Error", "There was an error in retreving the questions");
                }
            }
        }

        /// <summary>
        /// This method is responsible for changing the CAAEvents view for the Delete Mode toggle feature
        /// </summary>
        private void DeleteModeToggle()
        {
            if (btnDeleteMode.Content.ToString() == "DELETE MODE (OFF)")
            {
                gdvEditEvents.Visibility = Visibility.Collapsed;
                gdvDeleteEvents.Visibility = Visibility.Visible;
                btnDeleteMode.Content = "DELETE MODE (ON)";
                SolidColorBrush toRed = new SolidColorBrush(Windows.UI.Colors.Red);
                btnDeleteMode.Foreground = toRed;
                btnDeleteMode.BorderBrush = toRed;
                deleteMode = 1;
                FillDropDown(CurrentOrPast);
            }
            else if (btnDeleteMode.Content.ToString() == "DELETE MODE (ON)")
            {
                gdvEditEvents.Visibility = Visibility.Visible;
                gdvDeleteEvents.Visibility = Visibility.Collapsed;
                btnDeleteMode.Content = "DELETE MODE (OFF)";
                SolidColorBrush toBlue = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 27, 62, 110));
                btnDeleteMode.Foreground = toBlue;
                btnDeleteMode.BorderBrush = toBlue;
                deleteMode = 0;
                FillDropDown(CurrentOrPast);
            }
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