using CAA_Event_Management.Data;
using CAA_Event_Management.Models;
using CAA_Event_Management.ViewModels;
using CAA_Event_Management.Views.EventViews;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
/**************************
 * Created By: Jon Yade
 * Edited By:
 **************************/

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
namespace CAA_Event_Management.Views.EventViews
{
    /// <summary>
    /// Frame for Event Items
    /// </summary>
    public sealed partial class EventItemView : Page
    {
        #region Startup - variables, repositories, methods

        Event currentEvent;
        EventItem eventItem;
        private int questionCount = 0;

        IEventItemRepository eventItemRepository;
        IItemRepository itemRepository;

        public EventItemView()
        {
            this.InitializeComponent();
            eventItemRepository = new EventItemRepository();
            itemRepository = new ItemRepository();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            currentEvent = (Event)e.Parameter;
            tbkCurrentEventName.Text = currentEvent.EventName;
            FillFields();
        }

        #endregion

        #region Buttons - Add Question, Remove Question

        private void btnAddSurveyQuestion_Click(object sender, RoutedEventArgs e)
        {
            Item selectedItem;

            if (cboAvailableItems != null && questionCount < 5)
            {
                selectedItem = (Item)cboAvailableItems.SelectedItem;

                try
                {
                    this.DataContext = eventItem;

                    eventItem = new EventItem();
                    eventItem.ItemID = selectedItem.ItemID;
                    eventItem.EventID = currentEvent.EventID;
                    eventItemRepository.AddEventItem(eventItem);
                }
                catch (Exception ex)
                {
                    Jeeves.ShowMessage("Error", ex.Message.ToString());
                }
            }
            else
            {
                Jeeves.ShowMessage("Error", "You are only allowed 5 questions per event");
            }
            FillFields();
        }

        private void btnDeleteSurveyQuestion_Click(object sender, RoutedEventArgs e)
        {
            EventItemDetails selectedEID = new EventItemDetails();

            if (lstCurrentSurveyQuestions.SelectedItem != null) selectedEID = (EventItemDetails)lstCurrentSurveyQuestions.SelectedItem;
            try
            {
                EventItem eventItem = eventItemRepository.GetEventItem(selectedEID.EIDEventItemID);  //make the getEvents more specific by only selecting events that match the ID of the current event
                eventItemRepository.DeleteEventItem(eventItem);
                FillFields();
            }
            catch
            {
                Jeeves.ShowMessage("Error", "Failure; please try again");
            }
        }

        #endregion

        #region Helper Methods - FillFields, SearchBox

        private void FillFields()
        {
            try
            {
                List<Item> items = itemRepository.GetItems();
                List<EventItem> eventItems = eventItemRepository.GetEventItems(currentEvent.EventID);

                List<Item> displayedItems = new List<Item>();
                List<EventItem> assignedItems = new List<EventItem>();
                List<EventItemDetails> testAssignedItems = new List<EventItemDetails>();

                foreach (var x in eventItems)
                {
                    EventItemDetails eventItemDetails = new EventItemDetails();
                    eventItemDetails.EIDEventItemID = x.EventItemID;

                    if (x.EventID == currentEvent.EventID)
                    {
                        eventItemDetails.EIDEventID = currentEvent.EventID;
                        eventItemDetails.EIDEventDisplayName = currentEvent.EventName;
                    }

                    var currentItem = itemRepository.GetItem(x.ItemID);
                    eventItemDetails.EIDItemID = currentItem.ItemID;
                    eventItemDetails.EIDItemPhrase = currentItem.ItemName;
                    eventItemDetails.EIDItemAssigned = true;

                    testAssignedItems.Add(eventItemDetails);
                }

                foreach (var a in items)
                {
                    bool add = true;

                    foreach (var b in testAssignedItems)
                    {
                        if (b.EIDItemID == a.ItemID) add = false;
                    }

                    if (add == true) displayedItems.Add(a);
                }

                lstCurrentSurveyQuestions.ItemsSource = testAssignedItems;
                cboAvailableItems.ItemsSource = displayedItems;
                questionCount = testAssignedItems.Count();
            }
            catch (Exception)
            {
                Jeeves.ShowMessage("Error", "Unable to fill the fields");
            }
        }

        private void txtSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtSearchBox.Text == "") FillFields();
            else
            {
                try
                {
                    List<Item> items = itemRepository.GetItems();
                    List<Item> searchItems = new List<Item>();
                    string searchString = txtSearchBox.Text.ToLower();

                    foreach (var x in items)
                    {
                        EventItem checkItem = eventItemRepository.GetEventItem(currentEvent.EventID, x.ItemID);

                        if (checkItem == null && x.ItemName.ToLower().IndexOf(searchString) > -1)
                        {
                            searchItems.Add(x);
                        }
                    }
                    cboAvailableItems.ItemsSource = searchItems;
                }
                catch (Exception)
                {
                    Jeeves.ShowMessage("Error", "There was an error in retreving the questions");
                }
            }
        }

        #endregion
    }
}
