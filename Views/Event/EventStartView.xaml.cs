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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CAA_Event_Management.Views.Event
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EventStartView : Page
    {

        IEventRepository eventRepository;



        public EventStartView()
        {
            this.InitializeComponent();
            eventRepository = new EventRepository();
            FillCurrentEvents();
        }

        private void FillCurrentEvents()
        {
            List<Models.Event> eventsList = new List<Models.Event>();
            List<Models.Event> currentEvents = new List<Models.Event>();
            DateTime now = DateTime.Now;

            try
            {
                bool isDeleted = false;
                eventsList = eventRepository.GetEvents(isDeleted);
            }
            catch
            {
                Jeeves.ShowMessage("Error", "The events failed to populate; please reload the program");
            }
            
            if (eventsList.Count > 0)
            {
                currentEvents = eventsList
                    .Where(c => c.EventStart <= now.AddHours(3) && c.EventEnd >= now.AddHours(-2))
                    .OrderBy(c => c.EventName)
                    .ToList();
            }
            gvAvailableEvents.ItemsSource = currentEvents;
        }

        private void btnStartEvent_Click(object sender, RoutedEventArgs e)
        {
            if (gvAvailableEvents.SelectedItem != null)
            {
                Models.Event selectedEvent = (Models.Event)gvAvailableEvents.SelectedItem;
                Frame.Navigate(typeof(EventAttendenceTracking), (Models.Event)selectedEvent);
            }
            else
            {
                Jeeves.ShowMessage("Error", "Please select an active event");
            }
        }
    }
}
