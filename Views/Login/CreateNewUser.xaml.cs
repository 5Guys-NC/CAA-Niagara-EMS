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
/***************************************
 * Created By: Brian Culp
 * Edited By:
 * *************************************/
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CAA_Event_Management
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    
    public sealed partial class CreateNewUser : Page
    {
        Users u;

        IUsersRepository userRepository;

        public CreateNewUser()
        {
            this.InitializeComponent();
            userRepository = new UsersRepository();
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            u = new Users();
            this.DataContext = u;

            try
            {
                if (txtFName.Text != "")
                {
                    u.FirstName = txtFName.Text;
                }

                if (txtLName.Text != "")
                {
                    u.LastName = txtLName.Text;
                }

                if (chkAdmin.IsChecked == true)
                {
                    u.isAdmin = true;
                }
                else
                {
                    u.isAdmin = false;
                }

                if (txtPassword.Password == txtConfirmPassword.Password)
                {
                    u.Password = txtPassword.Password;
                }
                else
                {
                    Jeeves.ShowMessage("Error", "Passwords do not match");
                    return;
                }

                u.UserName = txtUserName.Text;

                userRepository.AddUser(u);
                ClearFields();

            }
            catch (Exception ex)
            {
                Jeeves.ShowMessage("Error", ex.GetBaseException().Message.ToString());
            }
        }

        private void ClearFields()
        {
            txtUserName.Text = "";
            txtFName.Text = "";
            txtLName.Text = "";
            txtPassword.Password = "";
            txtConfirmPassword.Password = "";
            chkAdmin.IsChecked = false;

        }

    }



}
     
