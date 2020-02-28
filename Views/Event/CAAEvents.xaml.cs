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
/******************************
*  Model Created By: Jon Yade
*  Edited by: Nathan Smith
*******************************/

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
namespace CAA_Event_Management
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
        }

        private void FillDropDown(int check)
        {
            DateTime now = DateTime.Today.AddDays(-1);

            try
            {
                bool deleted = false;
                List<Event> noDeletedEvents = eventRepository.GetEvents(deleted);
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

                    List<Event> upcomingEvents = noDeletedEvents
                    .Where(c => c.EventEnd >= now)
                    .OrderBy(c => c.EventStart)
                    .ToList();
                    gdvEvents.ItemsSource = upcomingEvents;
                }
                else
                {
                    List<Event> pastEvents = noDeletedEvents
                    .Where(c => c.EventEnd < now)
                    .OrderByDescending(c => c.EventStart)
                    .ToList();
                    gdvEvents.ItemsSource = pastEvents;
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
            Event newEvent = new Event();
            Frame.Navigate(typeof(EventDetails), newEvent);
        }

        private void btnCurrentEvents_Click(object sender, RoutedEventArgs e)
        {
            CurrentOrPast = 1;
            FillDropDown(1);
        }

        private void btnPastEvents_Click(object sender, RoutedEventArgs e)
        {
            CurrentOrPast = 2;
            FillDropDown(2);
        }

        private void btnSelectedEvent_Click(object sender, RoutedEventArgs e)
        {
            if (gdvEvents.SelectedItem != null)
            {
                var selectedEvent = gdvEvents.SelectedItem;
                Frame.Navigate(typeof(EventDetails), (Event)selectedEvent);
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

        #region Buttons - Adding: Attendees, Items, EventItems

        private void btnRegisterAttendance_Click(object sender, RoutedEventArgs e)
        {
            if (gdvEvents.SelectedItem != null)
            {
                Event selectedEvent = (Event)gdvEvents.SelectedItem;
                Frame.Navigate(typeof(EventAttendenceTracking), (Event)selectedEvent);
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

        private void btnCreateEvent_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Event newEvent = new Event();
            Frame.Navigate(typeof(EventDetails), newEvent);
        }

        private void btnCreateSurvey_Tapped(object sender, TappedRoutedEventArgs e)
        {

            //Frame.Navigate(typeof(ItemsView));
        }

        private void btnRegisterAttendee_Tapped(object sender, TappedRoutedEventArgs e)
        {
            return;
        }

        private void btnItems_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ItemsView));
        }

        private void BtnConfirmRemove_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int selected = Convert.ToInt32(((Button)sender).DataContext);
            Event selectedEvent = new Event();
            selectedEvent = eventRepository.GetEvent(selected.ToString());
            eventRepository.DeleteEvent(selectedEvent);
            Frame.Navigate(typeof(CAAEvents), null, new SuppressNavigationTransitionInfo());
        }


        #endregion

        #region Helper Methods - SearchBox_TextChanged

        private void cboCurrentEvents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ////var selectedEvent = cboCurrentEvents.SelectedItem;
            //var selectedEvent = ListViewBox.SelectedItem;
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
                    gdvEvents.ItemsSource = searchEvents;
                }
                catch (Exception)
                {
                    Jeeves.ShowMessage("Error", "There was an error in retreving the questions");
                }
            }
        }


        #endregion

        //tester window - delete this later
        //private void btnPOSTester_Click(object sender, RoutedEventArgs e)
        //{
        //    Frame.Navigate(typeof(POSTestView));
        //}
    }
}
