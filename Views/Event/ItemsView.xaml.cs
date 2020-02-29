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
using Windows.UI.Xaml.Media.Animation;
/********************************
* Created By: Jon Yade
* Edited By:
* ******************************/

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
namespace CAA_Event_Management
{
    /// <summary>
    /// Frame for Items
    /// </summary>
    public sealed partial class ItemsView : Page
    {
        #region Startup - variables, respositories, methods

        Item item;
        Item selectedItem;
        int addOrEdit;
        readonly List<DataType> dataList = new List<DataType>();

        IItemRepository itemRespository;

        public ItemsView()
        {
            this.InitializeComponent();
            itemRespository = new ItemRepository();
            FillDataTypeComboBox();
            FillFields();
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
            string warning = "Please exercise caution when editing this question. Do you wish to continue?";
            selectedItem = (Item)gvAvailableQuestions.SelectedItem;

            if (selectedItem != null)
            {
                var result = await Jeeves.ConfirmDialog("Warning", warning);

                if (result == ContentDialogResult.Secondary)
                {
                    addOrEdit = 2;
                    BeginUpdate(selectedItem);
                    ScreenLockDown();
                }
            }
        }

        private void btnSaveQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (addOrEdit == 1)
            {
                SaveCreate();
            }
            else if (addOrEdit == 2)
            {
                SaveUpdate(selectedItem);
                selectedItem = null;
            }
            else Jeeves.ShowMessage("Error", "There was a problem saving the question; please try again");
            ScreenUnlock();
            ClearFields();
            FillFields();
        }

        private void btnCancelSave_Click(object sender, RoutedEventArgs e)
        {
            selectedItem = null;
            ScreenUnlock();
            ClearFields();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (btnDelete.Content.ToString() == "Delete Mode (OFF)")
            {
                btnDelete.Content = "Delete Mode (ON)";
                gvAvailableQuestions.Visibility = Visibility.Collapsed;
                gvAvailableQuestionsDeleteMode.Visibility = Visibility.Visible;
                FillFields();
            }
            else
            {
                btnDelete.Content = "Delete Mode (OFF)";
                gvAvailableQuestions.Visibility = Visibility.Visible;
                gvAvailableQuestionsDeleteMode.Visibility = Visibility.Collapsed;
                FillFields();
            }
        }

        private void BtnConfirmRemove_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int selected = Convert.ToInt32(((Button)sender).DataContext);
            try
            {
                Item thisSelectedItem = itemRespository.GetItem(selected.ToString());
                itemRespository.DeleteItem(thisSelectedItem);
            }
            catch
            {
                Jeeves.ShowMessage("Error", "The was an error in deleting this question");
            }
            Frame.Navigate(typeof(CAAEvents), null, new SuppressNavigationTransitionInfo());
        }

        private void BtnCancel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            return;
        }

        #endregion

        #region Helper Methods - FillFields, FillDataTypeComboBox, SearchField

        private void FillFields()
        {
            try
            {
                List<Item> items = itemRespository.GetItems();
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

            dt1.DisplayText = "Yes-No";
            dt1.Type = "yesNo";

            dt2.DisplayText = "Numbers";
            dt2.Type = "number";

            dt3.DisplayText = "Words";
            dt3.Type = "word";

            dataList.Add(dt1);
            dataList.Add(dt2);
            dataList.Add(dt3);
            cboDataType.ItemsSource = dataList;
        }

        private void txtSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtSearchBox.Text == "") FillFields();
            else
            {
                DataType dataTypes = new DataType();

                try
                {
                    List<Item> items = itemRespository.GetItems();
                    List<Item> searchItems = new List<Item>();
                    string searchString = txtSearchBox.Text.ToLower();

                    foreach (var x in items)
                    {
                        if (x.ItemName.ToLower().IndexOf(searchString) > -1)
                        {
                            searchItems.Add(x);
                        }
                    }
                    //lstPreMadeQuestions.ItemsSource = searchItems;
                    gvAvailableQuestions.ItemsSource = searchItems;
                }
                catch (Exception)
                {
                    Jeeves.ShowMessage("Error", "There was an error in retreving the questions");
                }
            }
        }

        #endregion

        #region Helper Methods - Creating, Updating, Clearing Screen, Locking/Unlocking Screen

        private void SaveCreate()
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
                else if (txtNewSurveyQuestionText.Text != "")
                {
                    DataType selectedDataType = (DataType)cboDataType.SelectedItem;

                    item.ItemID = Guid.NewGuid().ToString();
                    item.ItemName = (string)txtNewSurveyQuestionText.Text;
                    item.ValueType = selectedDataType.DisplayText;
                    itemRespository.AddItem(item);
                }
            }
            catch (Exception ex)
            {
                Jeeves.ShowMessage("Error", ex.GetBaseException().Message.ToString());
            }
        }

        private void BeginUpdate(Item selectedItem)
        {
            DataType selectedDataType = dataList
                .Where(c => c.DisplayText == selectedItem.ValueType)
                .FirstOrDefault();
            cboDataType.SelectedItem = selectedDataType;
            txtNewSurveyQuestionText.Text = selectedItem.ItemName;
        }

        private void SaveUpdate(Item selectedItem)
        {
            try
            {
                selectedItem.ItemName = txtNewSurveyQuestionText.Text;
                DataType selectedDataType = (DataType)cboDataType.SelectedItem;

                selectedItem.ValueType = selectedDataType.DisplayText;
                itemRespository.UpdateItem(selectedItem);
            }
            catch
            {
                Jeeves.ShowMessage("Error", "Unable to save the question");
            }
        }

        private void ClearFields()
        {
            txtNewSurveyQuestionText.Text = "";
            txtSearchBox.Text = "";
            cboDataType.SelectedItem = null;
        }

        private void ScreenLockDown()
        {
            panelManageQuestions.Visibility = Visibility.Visible;
            btnEditQuestion.IsEnabled = false;
            btnAddSurveyQuestion.IsEnabled = false;
            //btnDelete.IsEnabled = false;
            gvAvailableQuestions.IsEnabled = false;
        }

        private void ScreenUnlock()
        {
            addOrEdit = 0;
            panelManageQuestions.Visibility = Visibility.Collapsed;
            btnAddSurveyQuestion.IsEnabled = true;
            //btnDelete.IsEnabled = true;
            btnEditQuestion.IsEnabled = true;
            gvAvailableQuestions.IsEnabled = true;
        }

        #endregion
    }

    internal class DataType
    {
        internal int ID {get;set;}
        public string DisplayText { get; set; }
        internal string Type { get; set; }
    }
}
