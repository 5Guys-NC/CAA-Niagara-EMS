using CAA_Event_Management.Models;
using CAA_Event_Management.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CAA_Event_Management.Views.EventViews
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateSurveyQuestion : Page
    {
        #region Startup - variables, respositories, methods

        Item item;
        readonly List<DataType> dataList = new List<DataType>();
        IItemRepository itemRepo;

        public CreateSurveyQuestion()
        {
            this.InitializeComponent();
            itemRepo = new ItemRepository();

            FillDataTypeComboBox();
            FillFields();
        }

        #endregion

        private void BtnAddSurveyQuestion_Tapped(object sender, TappedRoutedEventArgs e)
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
                    else if (txtSurveyQuestion.Text != "")
                    {
                        DataType selectedDataType = (DataType)cboDataType.SelectedItem;

                        item.ItemID = Guid.NewGuid().ToString();
                        item.ItemName = (string)txtSurveyQuestion.Text;
                        item.ValueType = selectedDataType.DisplayText;
                        itemRepo.AddItem(item);
                        ClearFields();
                    }

                }
                catch (Exception ex)
                {
                    Jeeves.ShowMessage("Error", ex.GetBaseException().Message.ToString());
                }
                FillFields();
        }

        #region Helper Methods - FillFields, FillDataTypeComboBox, SearchField, BeginUpdate, SaveQuestion, ClearFields

        private void FillFields()
        {
            try
            {
                List<Item> items = itemRepo.GetItems();
                lstPreMadeQuestions.ItemsSource = items;
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
            if (txtSearch.Text == "") FillFields();
            else
            {
                DataType dataTypes = new DataType();

                try
                {
                    List<Item> items = itemRepo.GetItems();
                    List<Item> searchItems = new List<Item>();
                    string searchString = txtSearch.Text.ToLower();

                    foreach (var x in items)
                    {
                        if (x.ItemName.ToLower().IndexOf(searchString) > -1)
                        {
                            searchItems.Add(x);
                        }
                    }
                    lstPreMadeQuestions.ItemsSource = searchItems;
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

            btnAddSurveyQuestion.IsEnabled = false;
            btnDeleteQuestion.IsEnabled = false;
            txtSurveyQuestion.Text = selectedItem.ItemName;
            btnEditQuestion.Content = "Save Question";
        }

        private void SaveQuestion(Item selectedItem)
        {
            try
            {
                selectedItem.ItemName = txtSurveyQuestion.Text;
                DataType selectedDataType = (DataType)cboDataType.SelectedItem;

                selectedItem.ValueType = selectedDataType.DisplayText;
                itemRepo.UpdateItem(selectedItem);

                btnEditQuestion.Content = "Edit Question";
                btnAddSurveyQuestion.IsEnabled = true;
                btnDeleteQuestion.IsEnabled = true;
                ClearFields();
            }
            catch
            {
                Jeeves.ShowMessage("Error", "Unable to save the question");
            }
            FillFields();
        }

        private void ClearFields()
        {
            txtSurveyQuestion.Text = "";
            cboDataType.SelectedItem = null;
        }

        #endregion
    }

}




