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
        int displayChoice = 1;
        int deleteMode = 0;
        int addOrEdit;
        readonly List<DataType> dataList = new List<DataType>();

        IItemRepository itemRespository;

        public Surveys()
        {
            this.InitializeComponent();
            itemRespository = new ItemRepository();
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
            string warning = "Please exercise caution when editing this question. Do you wish to continue?";

            if (selectedItem != null)
            {
                var result = await Jeeves.ConfirmDialog("Warning", warning);

                if (result == ContentDialogResult.Secondary) //&& btnEditSurvey.Content.ToString() == "#xE74D;"
                {
                    ScreenLockDown();
                    BeginUpdate(selectedItem);
                }
                else if (result == ContentDialogResult.Secondary)
                {
                    SaveQuestion(selectedItem);
                }
            }
        }

        private void btnSaveQuestion_Click(object sender, RoutedEventArgs e)
        {
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
                        DataType selectedDataType = (DataType)cboDataType.SelectedItem;
                        App userInfo = (App)Application.Current;
                        item.ItemID = Guid.NewGuid().ToString();
                        item.ItemName = (string)txtNewSurveyQuestion.Text;
                        item.ValueType = selectedDataType.DisplayText;
                        item.CreatedBy = userInfo.userAccountName;
                        item.LastModifiedBy = userInfo.userAccountName;
                        itemRespository.AddItem(item);
                        NewAuditLine("Created by:" + userInfo.userAccountName + ", SurveyItem:" + item.ItemID + " " + item.ItemName + " " + item.ValueType + ", On Date: " + item.LastModifiedDate.ToString());
                        ClearFields();
                    }
                }
                catch (Exception ex)
                {
                    Jeeves.ShowMessage("Error", ex.GetBaseException().Message.ToString());
                }
                addOrEdit = 0;
                ScreenUnlock();
                ClearFields();
                FillFields(displayChoice);
            }
            else
            {
                App userInfo = (App)Application.Current;
                selectedItem.LastModifiedBy = userInfo.userAccountName;
                selectedItem.LastModifiedDate = DateTime.Now;
                SaveQuestion(selectedItem);
                NewAuditLine("Modified(Edit) by:" + userInfo.userAccountName + ", SurveyItem:" + selectedItem.ItemID + " To: " + selectedItem.ItemName + " " + selectedItem.ValueType + ", On Date: " + selectedItem.LastModifiedDate.ToString());
                selectedItem = null;
                addOrEdit = 0;
            }
        }

        private void btnCancelSave_Click(object sender, RoutedEventArgs e)
        {
            selectedItem = null;
            ScreenUnlock();
            ClearFields();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            DeleteModeToggle();
        }
    

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
                NewAuditLine("Modified(Delete) by:" + userInfo.userAccountName + ", SurveyItem:" + thisSelectedItem.ItemID + " " + thisSelectedItem.ItemName + ", On Date: " + thisSelectedItem.LastModifiedDate.ToString());
                Frame.Navigate(typeof(Surveys), deleteMode, new SuppressNavigationTransitionInfo());
            }
            catch
            {
                Jeeves.ShowMessage("Error", "Problem with deleting the survey question");
            }
        }

        private void BtnCancel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            return;
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

        #region Helper Methods - FillFields, FillDataTypeComboBox, SearchField, BeginUpdate, SaveQuestion, ClearFields, NewAuditLine

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

        private void BeginUpdate(Item selectedItem)
        {
            DataType selectedDataType = dataList
                .Where(c => c.DisplayText == selectedItem.ValueType)
                .FirstOrDefault();
            cboDataType.SelectedItem = selectedDataType;
            txtNewSurveyQuestion.Text = selectedItem.ItemName;
            ScreenLockDown();
        }

        private void SaveQuestion(Item selectedItem)
        {
            try
            {
                selectedItem.ItemName = txtNewSurveyQuestion.Text;
                DataType selectedDataType = (DataType)cboDataType.SelectedItem;

                selectedItem.ValueType = selectedDataType.DisplayText;
                itemRespository.UpdateItem(selectedItem);

                ScreenUnlock();
                ClearFields();
            }
            catch
            {
                Jeeves.ShowMessage("Error", "Unable to save the question");
            }
            FillFields(displayChoice);
        }

        private void ClearFields()
        {
            txtNewSurveyQuestion.Text = "";
            txtSearchBox.Text = "";
            cboDataType.SelectedItem = null;
        }

        private void ScreenLockDown()
        {
            spQuestion.Visibility = Visibility.Visible;
            spDataType.Visibility = Visibility.Visible;
            rpButtons.Visibility = Visibility.Collapsed;
            rpSaveAndCancel.Visibility = Visibility.Visible;
            gvAvailableQuestions.IsEnabled = false;
            gvAvailableQuestionsDeleteMode.IsEnabled = true;
        }

        private void ScreenUnlock()
        {
            spQuestion.Visibility = Visibility.Collapsed;
            spDataType.Visibility = Visibility.Collapsed;
            rpButtons.Visibility = Visibility.Visible;
            rpSaveAndCancel.Visibility = Visibility.Collapsed;
            gvAvailableQuestions.IsEnabled = true;
            gvAvailableQuestionsDeleteMode.IsEnabled = true;
        }

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

        private void NewAuditLine(string newLine)
        {
            AuditLog line = new AuditLog();
            line.WriteToAuditLog(newLine);
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
