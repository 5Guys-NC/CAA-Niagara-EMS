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

        IEventRepository eventRepository;
        public CAAEvents()
        {
            this.InitializeComponent();
            eventRepository = new EventRepository();
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

        private void BtnConfirmRemove_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //string test = "";
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
                
                //AuditLog line = new AuditLog();
                //string newLine = "Modified(Delete) by:" + userInfo.userAccountName + ", Event:" + selectedEvent.EventID + " " + selectedEvent.AbrevEventname + ", On Date: " + selectedEvent.LastModifiedDate.ToString();
                //line.WriteToAuditLog(newLine);
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

        private void WriteNewAuditLineToDatabase(string userName, string objectTable, string typeID, string newTypeInfo, string changeDate, string changeType, string changeInfo)
        {
            AuditLog line = new AuditLog();
            line.WriteAuditLineToDatabase(userName, objectTable, typeID, newTypeInfo, changeDate, changeType, changeInfo);
        }

        #endregion
    }
}