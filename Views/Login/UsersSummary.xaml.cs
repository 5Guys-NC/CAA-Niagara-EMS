using CAA_Event_Management.Data;
using CAA_Event_Management.Models;
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
/******************************
*  Model Created By: Nathan Smith
*  Edited by: Brian Culp
*******************************/

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
namespace CAA_Event_Management
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UsersSummary : Page
    {
        #region Startup - variables. repositories, methods

        IUsersRepository usersRepository;

        public UsersSummary()
        {
            this.InitializeComponent();
            usersRepository = new UsersRepository();
            FillUserList();
        }

        private void FillUserList()
        {
            try
            {
                List<Users> users = usersRepository.GetUsers();

                lvUsers.ItemsSource = users;
            }
            catch (Exception e)
            {
                Jeeves.ShowMessage("Error", e.Message.ToString());
            }
        }

        #endregion

        private void BtnDetails_Click(object sender, RoutedEventArgs e)
        {
            UserParams up = new UserParams();
            if (lvUsers.SelectedItem != null)
            {
                up.selectedUser = (Users)lvUsers.SelectedItem;
                Frame.Navigate(typeof(UserDetails), up);
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var u = lvUsers.SelectedItem;
            usersRepository.DeleteUser((Users)u);
            Frame.GoBack();
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CreateNewUser));
        }
    }
    public class UserParams
    {
        public Users selectedUser { get; set; }
        public bool editMode { get; set; }
    }
}