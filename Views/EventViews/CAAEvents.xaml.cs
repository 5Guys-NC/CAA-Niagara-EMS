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
        IEventRepository eventRepository;
        public CAAEvents()
        {
            this.InitializeComponent();
            eventRepository = new EventRepository();
            FillDropDown(1);

            ((Window.Current.Content as Frame).Content as MainPage).ChangeMainPageTitleName("GENERAL EVENT MANAGEMENT");
        }

        private void FillDropDown(int check)
        {
            DateTime now = DateTime.Today.AddDays(1);

            try
            {
                bool deleted = false;
                List<Models.Event> noDeletedEvents = eventRepository.GetEvents(deleted);
                //List<Event> upcomingEvents = noDeletedEvents
                //    .Where(c => c.EventEnd >= now)
                //    .OrderBy(c => c.EventStart)
                //    .ToList();
                //List<Event> pastEvents = noDeletedEvents
                //    .Where(c => c.EventEnd < now)
                //    .OrderByDescending(c => c.EventStart)
                //    .ToList();
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
                //UpcomingEventList.ItemsSource = pastEvents;
                //UpcomingEventList.ItemsSource = noDeletedEvents;
            }
            catch (Exception e)
            {
                Jeeves.ShowMessage("Error", e.Message.ToString());
            }
        }

        #endregion

        #region Buttons - CRUD - NewEvent, DeleteEvent, SelectedEvent(edit)

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

        //private void btnSelectedEvent_Click(object sender, RoutedEventArgs e)
        //{
        //    if (gdvEditEvents.SelectedItem != null)
        //    {
        //        var selectedEvent = gdvEditEvents.SelectedItem;
        //        Frame.Navigate(typeof(EventDetails), (Models.Event)selectedEvent);
        //    }

        //}

        private void btnDeleteMode_Click(object sender, RoutedEventArgs e)
        {
            if(btnDeleteMode.Content.ToString() == "DELETE MODE (OFF)")
            {
                gdvEditEvents.Visibility = Visibility.Collapsed;
                gdvDeleteEvents.Visibility = Visibility.Visible;
                btnDeleteMode.Content = "DELETE MODE (ON)";
                FillDropDown(CurrentOrPast);
            }
            else if(btnDeleteMode.Content.ToString() == "DELETE MODE (ON)")
            {
                gdvEditEvents.Visibility = Visibility.Visible;
                gdvDeleteEvents.Visibility = Visibility.Collapsed;
                btnDeleteMode.Content = "DELETE MODE (OFF)";
                FillDropDown(CurrentOrPast);
            }
        }


        //private void btnEventDelete_Click(object sender, RoutedEventArgs e)
        //{
        //    //if (ListViewBox.SelectedItem != null)
        //    //{
        //    //    Event selectedEvent = (Event)ListViewBox.SelectedItem;
        //    //    selectedEvent.IsDeleted = true;
        //    //    eventRepository.DeleteEvent(selectedEvent);
        //    //    FillDropDown();
        //    //}

        //}

        #endregion

        #region Buttons - Adding: Attendees, Items, EventItems, 

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
            return;
        }

        //private void btnCreateEvent_Tapped(object sender, TappedRoutedEventArgs e)
        //{
        //    Event newEvent = new Event();
        //    Frame.Navigate(typeof(EventDetails), newEvent);
        //}

        //private void btnCreateSurvey_Tapped(object sender, TappedRoutedEventArgs e)
        //{

        //    //Frame.Navigate(typeof(ItemsView));
        //}

        //private void btnBeginEvent_Tapped(object sender, TappedRoutedEventArgs e)
        //{
        //    return;
        //}

        //private void btnItems_Click(object sender, RoutedEventArgs e)
        //{
        //    Frame.Navigate(typeof(Surveys));
        //}

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
                Frame.Navigate(typeof(CAAEvents), null, new SuppressNavigationTransitionInfo());
            }
            catch
            {
                Jeeves.ShowMessage("Error", "There was a problem deleting the selected record");
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


        #endregion

        #region Helper Methods - SearchBox_TextChanged

        //private void cboCurrentEvents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    ////var selectedEvent = cboCurrentEvents.SelectedItem;
        //    //var selectedEvent = ListViewBox.SelectedItem;
        //}

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
        #endregion

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
    }
}