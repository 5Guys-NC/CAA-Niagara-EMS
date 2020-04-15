using CAA_Event_Management.Data;
using CAA_Event_Management.Data.Interface_Repos;
using CAA_Event_Management.Data.Repos;
using CAA_Event_Management.Models;
using CAA_Event_Management.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
/******************************
*  Model Created By: Nathan Smith
*  Edited by: Brian Culp
*******************************/

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
namespace CAA_Event_Management.Views.Login
{ 
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
public sealed partial class UsersSummary : Page
    {
        #region Startup - variables. repositories, methods

        IUserAccountRepository usersRepository;
        //all=false, admin=true
        bool filter = false;

        public UsersSummary()
        {
            this.InitializeComponent();
            usersRepository = new UserAccountRepository();
            
            //Fill list of Users
            FillUserList(filter);

            ((Window.Current.Content as Frame).Content as MainPage).ChangeMainPageTitleName("USER SUMMARY");
        }

        /// <summary>
        /// Method to create a list of all Users in database
        /// </summary>
        private void FillUserList(bool filter)
        {
            try
            {
                //get users
                List<UserAccount> users = usersRepository.GetUsers();

                //if only admin
                if (filter == true)
                {
                    users = users.Where(u => u.isAdmin).OrderByDescending(u => u.UserName).ToList();
                }

                //set source for the userList to the list of users
                userList.ItemsSource = users;
            }
            catch (Exception e)
            {
                //could not get list of users
                Jeeves.ShowMessage("Error", e.Message.ToString());
            }
        }

        #endregion


        /// <summary>
        /// Create Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            //Navigate to CreateNewUser
            Frame.Navigate(typeof(CreateNewUser), null, new SuppressNavigationTransitionInfo());
        }

        private void btnAllUsers_Click(object sender, RoutedEventArgs e)
        {
            Canvas.SetZIndex(btnAllUsers, 2);
            Canvas.SetZIndex(btnAdminUsers, -1);
            filter = false;
            FillUserList(filter);
        }

        private void btnAdminUsers_Click(object sender, RoutedEventArgs e)
        {
            Canvas.SetZIndex(btnAdminUsers, 2);
            Canvas.SetZIndex(btnAllUsers, -1);
            filter = true;
            FillUserList(filter);
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtSearch.Text == "") FillUserList(filter);
            else
            {
                try
                {
                    List<UserAccount> users = usersRepository.GetUsers();
                    List<UserAccount> searchUsers = new List<UserAccount>();
                    string searchString = txtSearch.Text.ToLower();

                    foreach (var x in users)
                    {
                        if (x.UserName.ToLower().IndexOf(searchString) > -1)
                        {
                            searchUsers.Add(x);
                        }
                    }
                    userList.ItemsSource = searchUsers;
                }
                catch (Exception)
                {
                    Jeeves.ShowMessage("Error", "There was an error in retreving the users");
                }
            }
        }

        private void userList_ItemClick(object sender, ItemClickEventArgs e)
        {
            UserParams up = new UserParams();

            //put selected user data in UserParams object
            up.selectedUser = (UserAccount)e.ClickedItem;
            //navigate to corresponding frame
            Frame.Navigate(typeof(UserDetails), up, new SuppressNavigationTransitionInfo());
        }
    }
    
}