using CAA_Event_Management.Data;
using CAA_Event_Management.Models;
using CAA_Event_Management.Views.EventViews;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
/*********************************
* Created By: Jon Yade
* *******************************/

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
namespace CAA_Event_Management.Views.EventViews
{
    /// <summary>
    /// Frame for EventStartView
    /// </summary>
    public sealed partial class EventStartView : Page
    {

        IEventRepository eventRepository;

        public EventStartView()
        {
            this.InitializeComponent();
            eventRepository = new EventRepository();
            FillCurrentEvents();

            if (((Window.Current.Content as Frame).Content as MainPage) != null)
            {
                ((Window.Current.Content as Frame).Content as MainPage).ChangeMainPageTitleName("CAA CURRENT EVENTS");
            }
        }

        private void FillCurrentEvents()
        {
            List<Models.Event> eventsList = new List<Models.Event>();
            DateTime now = DateTime.Now;

            try
            {
                bool isDeleted = false;
                eventsList = eventRepository.GetEvents(isDeleted)
                    .Where(c => c.EventStart <= now.AddHours(3) && c.EventEnd >= now.AddHours(-5))
                    .OrderBy(c => c.EventName)
                    .ToList();
            }
            catch
            {
                Jeeves.ShowMessage("Error", "The events failed to populate; please reload the program");
            }
            
            if (eventsList.Count > 0)
            {
                gvAvailableEvents.ItemsSource = eventsList;
            }
            else
            {
                tbkEventInfo.Visibility = Visibility.Visible;
                btnStartEvent.Visibility = Visibility.Collapsed;
            }
        }

        private void btnStartEvent_Click(object sender, RoutedEventArgs e)
        {
            if (gvAvailableEvents.SelectedItem != null)
            {
                Models.Event selectedEvent = (Models.Event)gvAvailableEvents.SelectedItem;
                Frame.Navigate(typeof(EventAttendanceTracking), (Models.Event)selectedEvent, new SuppressNavigationTransitionInfo());
            }
            else
            {
                Jeeves.ShowMessage("Error", "Please select an active event");
            }
        }
    }
}
