using CAA_Event_Management.Data;
using CAA_Event_Management.Models;
using CAA_Event_Management.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using CAA_Event_Management.Data.Interface_Repos;
using CAA_Event_Management.Data.Repos;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;
using CAA_Event_Management.Views.EventViews;
using System;
/******************************
*  Created By: Brian Culp
*  Edited by:
*******************************/

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
namespace CAA_Event_Management.Views.Login
{
    /// <summary>
    /// Frame for User Details
    /// </summary>
    public sealed partial class UserDetails : Page
    {
        UserAccount view;
        UserAccount viewOnNav;
        IUserAccountRepository userRepository;
        //get user logged in currently
        App userLoggedIn = (App)Application.Current;
        UserAccount editor = new UserAccount();

        public UserDetails()
        {
            this.InitializeComponent();
            userRepository = new UserAccountRepository();
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Get parameters on navigation and assign to UserParams object
            UserParams userParams = (UserParams)e.Parameter;
            
            //get selected User
            view = userParams.selectedUser;
            viewOnNav = userParams.selectedUser;
            this.DataContext = view;

            editor = userRepository.GetUser(userLoggedIn.userAccountName);

            EditMode(true);
            
            //if user is Admin
            if(view.isAdmin == true)
            {
                chkAdmin.IsChecked = true;
            }
            //if user FirstName not null
            if (view.FirstName != null)
            {
                txtFirstName.Text = view.FirstName;
            }
            //if user LastName not null
            if (view.LastName != null)
            {
                txtLastName.Text = view.LastName;
            }

            //show username
            txtUserName.Text = view.UserName;

            ((Window.Current.Content as Frame).Content as MainPage).ChangeMainPageTitleName("DETAILS OF " + view.UserName.ToUpper());
        }

        /// <summary>
        /// Method to set text boxes to read only if not in edit mode
        /// </summary>
        /// <param name="status"></param>
        private void EditMode(bool status)
        {
            chkAdmin.IsEnabled = status;
            txtFirstName.IsReadOnly = !status;
            txtLastName.IsReadOnly = !status;
            txtUserName.IsReadOnly = !status;
        }

        private async void btnEdit_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            string popupMsg = "Are you sure that you want to make changes to this user?";

            var result = await Jeeves.ConfirmDialog("Edit Existing User", popupMsg);

            if (result == ContentDialogResult.Secondary)
            {
                try
                {
                    view.FirstName = txtFirstName.Text;
                    view.LastName = txtLastName.Text;
                    view.UserName = txtUserName.Text;
                    if (chkAdmin.IsChecked == true)
                    {
                        view.isAdmin = true;
                    }
                    else
                    {
                        view.isAdmin = false;
                    }

                    userRepository.UpdateUser(view);
                }
                catch (Exception ex)
                {
                    //error if adding user was unsuccessful
                    Jeeves.ShowMessage("Error", ex.GetBaseException().Message.ToString());
                }
                finally
                {
                    if (editor.isAdmin)
                    {
                        Frame.Navigate(typeof(UsersSummary), null, new SuppressNavigationTransitionInfo());
                    }
                    else
                    {
                        Frame.Navigate(typeof(CAAEvents), null, new SuppressNavigationTransitionInfo());
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (editor.isAdmin)
            {
                Frame.Navigate(typeof(UsersSummary), null, new SuppressNavigationTransitionInfo());
            }
            else
            {
                Frame.Navigate(typeof(CAAEvents), null, new SuppressNavigationTransitionInfo());
            }
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string popupMsg = "Are you sure that you want to make changes to this user?";

            var result = await Jeeves.ConfirmDialog("Creating a New User", popupMsg);

            if (result == ContentDialogResult.Secondary)
            {
                try
                {
                    view.FirstName = txtFirstName.Text;
                    view.LastName = txtLastName.Text;
                    view.UserName = txtUserName.Text;

                        userRepository.UpdateUser(view);
                    
                }
                catch (Exception ex)
                {
                    //error if adding user was unsuccessful
                    Jeeves.ShowMessage("Error", ex.GetBaseException().Message.ToString());
                }
                finally
                {
                    if (editor.isAdmin)
                    {
                        Frame.Navigate(typeof(UsersSummary), null, new SuppressNavigationTransitionInfo());
                    }
                    else
                    {
                        Frame.Navigate(typeof(CAAEvents), null, new SuppressNavigationTransitionInfo());
                    }
                }
            }
        }
    }
}
