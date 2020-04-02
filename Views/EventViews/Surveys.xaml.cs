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
using CAA_Event_Management.Views.EventViews;
using CAA_Event_Management.Models;
using Windows.UI.Xaml.Media.Animation;
using CAA_Event_Management.Utilities;
using System.Threading.Tasks;
using CAA_Event_Management.Data.Interface_Repos;
using CAA_Event_Management.Data.Repos;
/********************************
* Created By: Jon Yade
* Edited By:
* ******************************/

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
namespace CAA_Event_Management.Views.EventViews
{
    /// <summary>
    /// Frame for Items
    /// </summary>
    public sealed partial class Surveys : Page
    {
        #region Startup - variables, respositories, methods

        Item item;
        Item selectedItem;
        Item startItemEditState;
        int displayChoice = 1;
        int deleteMode = 0;
        int addOrEdit;
        readonly List<DataType> dataList = new List<DataType>();

        IItemRepository itemRespository;
        IModelAuditLineRepository auditLineRepository;

        public Surveys()
        {
            this.InitializeComponent();
            itemRespository = new ItemRepository();
            auditLineRepository = new ModelAuditLineRepository();
            ((Window.Current.Content as Frame).Content as MainPage).ChangeMainPageTitleName("SURVEY QUESTION MANAGEMENT");

            FillDataTypeComboBox();
            FillFields(displayChoice);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null) deleteMode = (int)e.Parameter;
            if (deleteMode == 1) DeleteModeToggle();
        }

        #endregion

        #region Buttons - Add, Edit, Delete Questions

        private void btnAddSurveyQuestion_Click(object sender, RoutedEventArgs e)
        {
            ScreenLockDown();
            addOrEdit = 1;
        }

        private async void btnEditMultipurpose_Click(object sender, RoutedEventArgs e)
        {
            addOrEdit = 2;

            string selected = Convert.ToString(((Button)sender).DataContext);
            selectedItem = itemRespository.GetItem(selected.ToString());
            startItemEditState = itemRespository.GetItem(selected.ToString());
            string warning = "Please exercise caution when editing this question. Do you wish to continue?";

            if (selectedItem != null)
            {
                var result = await Jeeves.ConfirmDialog("Warning", warning);

                if (result == ContentDialogResult.Secondary) //&& btnEditSurvey.Content.ToString() == "#xE74D;"
                {
                    ScreenLockDown();
                    BeginUpdate(selectedItem);
                }
            }
        }

        private void btnSaveQuestion_Click(object sender, RoutedEventArgs e)
        {
            DataType selectedDataType = (DataType)cboDataType.SelectedItem;

            if (addOrEdit == 1)
            {
                item = new Item();
                this.DataContext = item;

                try
                {
                    if (cboDataType.SelectedItem == null)
                    {
                        Jeeves.ShowMessage("Error", "Please select a data type");
                        return;
                    }
                    else if (txtNewSurveyQuestion.Text != "")
                    {

                        App userInfo = (App)Application.Current;
                        item.ItemID = Guid.NewGuid().ToString();
                        item.ItemName = (string)txtNewSurveyQuestion.Text;
                        item.ValueType = selectedDataType.DisplayText;
                        item.CreatedBy = userInfo.userAccountName;
                        item.LastModifiedBy = userInfo.userAccountName;
                        itemRespository.AddItem(item);
                        string thisEventDetails = "Item Question: " + item.ItemName + " Value: " + item.ValueType;
                        WriteNewAuditLineToDatabase(item.LastModifiedBy, "Item Table", item.ItemID, thisEventDetails, item.LastModifiedDate.ToString(),"Create","Item - Creation - No changes");
                    }
                }
                catch (Exception ex)
                {
                    Jeeves.ShowMessage("Error", ex.GetBaseException().Message.ToString());
                }
            }
            else
            {
                selectedItem.ItemName = (string)txtNewSurveyQuestion.Text;
                selectedItem.ValueType = selectedDataType.DisplayText;

                if (!startItemEditState.Equals(selectedItem))
                {
                    try
                    {
                        App userInfo = (App)Application.Current;
                        selectedItem.LastModifiedBy = userInfo.userAccountName;
                        selectedItem.LastModifiedDate = DateTime.Now;
                        itemRespository.UpdateItem(selectedItem);
                        string recordChanges = ShowObjectDifferences();
                        string thisEventDetails = "Item Question: " + selectedItem.ItemName + " Value: " + selectedItem.ValueType;
                        WriteNewAuditLineToDatabase(selectedItem.LastModifiedBy, "Item Table", selectedItem.ItemID, thisEventDetails, selectedItem.LastModifiedDate.ToString(),"Edit",recordChanges);
                    }
                    catch { }
                }
                selectedItem = null;
                startItemEditState = null;
            }
            ScreenUnlock();
            ClearFields();
            FillFields(displayChoice);
            addOrEdit = 0;
        }

        private void btnCancelSave_Click(object sender, RoutedEventArgs e)
        {
            startItemEditState = null;
            selectedItem = null;
            ScreenUnlock();
            ClearFields();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            DeleteModeToggle();
        }

        /// <summary>
        /// This method handles the click event for survey item deletion and updates the audit table when this is done by the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnConfirmRemove_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                string selected = Convert.ToString(((Button)sender).DataContext);
                Item thisSelectedItem = itemRespository.GetItem(selected.ToString());

                App userInfo = (App)Application.Current;
                thisSelectedItem.IsDeleted = true;
                thisSelectedItem.LastModifiedBy = userInfo.userAccountName;
                thisSelectedItem.LastModifiedDate = DateTime.Now;
                itemRespository.DeleteUpdateItem(thisSelectedItem);

                string thisEventDetails = "Item Question: " + thisSelectedItem.ItemName + " Value: " + thisSelectedItem.ValueType;
                WriteNewAuditLineToDatabase(thisSelectedItem.LastModifiedBy, "Item Table", thisSelectedItem.ItemID, thisEventDetails, thisSelectedItem.LastModifiedDate.ToString(), "Delete", "Item - Manual Deletion - 'IsDeleted' to 'true'");
                Frame.Navigate(typeof(Surveys), deleteMode, new SuppressNavigationTransitionInfo());
            }
            catch
            {
                Jeeves.ShowMessage("Error", "Problem with deleting the survey question");
            }
        }

        /// <summary>
        /// This method handles the Cancel button click by the user on the "Delete Survey Item" fly-out
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(Surveys), deleteMode, new SuppressNavigationTransitionInfo());
        }


        private void btnMostUsedQuestions_Click(object sender, RoutedEventArgs e)
        {
            Canvas.SetZIndex(btnMostUsedQuestions, 1);
            Canvas.SetZIndex(btnAlphabeticalQuestions, 0);
            displayChoice = 1;
            FillFields(displayChoice);
        }

        private void btnAlphabeticalQuestions_Click(object sender, RoutedEventArgs e)
        {
            Canvas.SetZIndex(btnAlphabeticalQuestions, 1);
            Canvas.SetZIndex(btnMostUsedQuestions, 0);
            displayChoice = 0;
            FillFields(displayChoice);
        }

        #endregion

        #region Helper Methods - FillFields, FillDataTypeComboBox, SearchField, BeginUpdate, ClearFields, NewAuditLine

        /// <summary>
        /// This method fills the available survey questions in the survey view and displays 
        /// them either by 'Most Used' or 'Alphabetically', based on user preference
        /// </summary>
        /// <param name="itemDisplayOption"></param>
        private void FillFields(int itemDisplayOption)
        {
            try
            {
                List<Item> items = new List<Item>();

                if (itemDisplayOption == 1)
                {
                    items = itemRespository.GetUndeletedItemsByCount();
                }
                else if (itemDisplayOption == 0)
                {
                    items = itemRespository.GetUndeletedItems();
                }
                gvAvailableQuestions.ItemsSource = items;
                gvAvailableQuestionsDeleteMode.ItemsSource = items;
            }
            catch (Exception)
            {
                Jeeves.ShowMessage("Error", "There was an error in retreving the questions");
            }
        }

        /// <summary>
        /// This method fills the survey item dropdown list for creating/editing purposes
        /// </summary>
        private void FillDataTypeComboBox()
        {
            DataType dt1 = new DataType();
            DataType dt2 = new DataType();
            DataType dt3 = new DataType();
            DataType dt4 = new DataType();

            dt1.DisplayText = "Yes-No";
            dt1.Type = "yesNo";

            dt2.DisplayText = "Numbers";
            dt2.Type = "number";

            dt3.DisplayText = "Words";
            dt3.Type = "word";

            dt4.DisplayText = "Dates";
            dt4.Type = "date";

            dataList.Add(dt1);
            dataList.Add(dt2);
            dataList.Add(dt3);
            dataList.Add(dt4);
            cboDataType.ItemsSource = dataList;
        }

        /// <summary>
        /// This method handles the search box text changed event as the user searchs for events, and gives the view the newly selected events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtSearchBox.Text == "") FillFields(displayChoice);
            else
            {
                DataType dataTypes = new DataType();

                try
                {
                    List<Item> items = itemRespository.GetUndeletedItems();
                    List<Item> searchItems = new List<Item>();
                    string searchString = txtSearchBox.Text.ToLower();

                    foreach (var x in items)
                    {
                        if (x.ItemName.ToLower().IndexOf(searchString) > -1)
                        {
                            searchItems.Add(x);
                        }
                    }
                    gvAvailableQuestionsDeleteMode.ItemsSource = searchItems;
                    gvAvailableQuestions.ItemsSource = searchItems;
                }
                catch (Exception)
                {
                    Jeeves.ShowMessage("Error", "There was an error in retreving the questions");
                }
            }
        }

        /// <summary>
        /// This method starts the editing process for a selected Survey item
        /// </summary>
        /// <param name="selectedItem"></param>
        private void BeginUpdate(Item selectedItem)
        {
            DataType selectedDataType = dataList
                .Where(c => c.DisplayText == selectedItem.ValueType)
                .FirstOrDefault();
            cboDataType.SelectedItem = selectedDataType;
            txtNewSurveyQuestion.Text = selectedItem.ItemName;
            ScreenLockDown();
        }

        /// <summary>
        /// This method resets the survey question create/edit box and dropdown
        /// </summary>
        private void ClearFields()
        {
            txtNewSurveyQuestion.Text = "";
            txtSearchBox.Text = "";
            cboDataType.SelectedItem = null;
        }

        /// <summary>
        /// This method locks down all the survey view features that are not directly related to the 
        /// survey item create/edit portion as well as displaying those features that are needed for creating/editing a survey item
        /// </summary>
        private void ScreenLockDown()
        {
            spQuestion.Visibility = Visibility.Visible;
            spDataType.Visibility = Visibility.Visible;
            rpButtons.Visibility = Visibility.Collapsed;
            rpSaveAndCancel.Visibility = Visibility.Visible;
            gvAvailableQuestions.IsEnabled = false;
            gvAvailableQuestionsDeleteMode.IsEnabled = true;
        }

        /// <summary>
        /// This method unlocks the Survey view features and hides those features that are used for creating/editing
        /// </summary>
        private void ScreenUnlock()
        {
            spQuestion.Visibility = Visibility.Collapsed;
            spDataType.Visibility = Visibility.Collapsed;
            rpButtons.Visibility = Visibility.Visible;
            rpSaveAndCancel.Visibility = Visibility.Collapsed;
            gvAvailableQuestions.IsEnabled = true;
            gvAvailableQuestionsDeleteMode.IsEnabled = true;
        }

        /// <summary>
        /// This method is responsible for changing the Survey view for the Delete Mode toggle feature
        /// </summary>
        private void DeleteModeToggle()
        {
            if (btnDelete.Content.ToString() == "Delete Mode (OFF)" || deleteMode == 0)
            {
                btnDelete.Content = "Delete Mode (ON)";
                rpSurvey.Visibility = Visibility.Collapsed;
                rpSurveyDeleteMode.Visibility = Visibility.Visible;
                SolidColorBrush toRed = new SolidColorBrush(Windows.UI.Colors.Red);
                btnDelete.Foreground = toRed;
                btnDelete.BorderBrush = toRed;
                deleteMode = 1;
                FillFields(displayChoice);
            }
            else
            {
                btnDelete.Content = "Delete Mode (OFF)";
                rpSurvey.Visibility = Visibility.Visible;
                rpSurveyDeleteMode.Visibility = Visibility.Collapsed;
                SolidColorBrush toBlue = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 27, 62, 110));
                btnDelete.Foreground = toBlue;
                btnDelete.BorderBrush = toBlue;
                deleteMode = 0;
                FillFields(displayChoice);
            }
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
            if (startItemEditState.ItemName != selectedItem.ItemName)
            {
                differences += "ItemName change: " + startItemEditState.ItemName + " TO: " + selectedItem.ItemName;
            }
            if (startItemEditState.ValueType != selectedItem.ValueType)
            {
                if (differences != "") differences += " | ";
                differences += "ValueType change: " + startItemEditState.ValueType + " TO: " + selectedItem.ValueType;
            }
            return differences;
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

    internal class DataType
    {
        internal int ID { get; set; }
        public string DisplayText { get; set; }
        internal string Type { get; set; }
    }
}
