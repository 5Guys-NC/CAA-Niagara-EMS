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
*  Model Created By: Brian Culp
*  Edited by:
*******************************/
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
namespace CAA_Event_Management
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserDetails : Page
    {
        Users view;
        private bool editMode = true;

        IUsersRepository userRepository;

        public UserDetails()
        {
            this.InitializeComponent();
            userRepository = new UsersRepository();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            UserParams userParams = (UserParams)e.Parameter;

            view = userParams.selectedUser;

            this.DataContext = view;

            if(view.isAdmin == true)
            {
                chkAdmin.IsChecked = true;
            }
            if (view.FirstName != null)
            {
                txtFirstName.Text = view.FirstName;
            }
            if (view.LastName != null)
            {
                txtLastName.Text = view.LastName;
            }

            txtUserName.Text = view.UserName;
            txtPassword.Text = view.Password;
        }

        private void EditMode(bool status)
        {
            txtFirstName.IsReadOnly = !status;
            txtLastName.IsReadOnly = !status;
            txtPassword.IsReadOnly = !status;
            txtUserName.IsReadOnly = !status;   
        }
    }
}
