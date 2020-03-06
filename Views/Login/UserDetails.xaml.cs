using CAA_Event_Management.Data;
using CAA_Event_Management.Models;
using CAA_Event_Management.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using CAA_Event_Management.Data.Interface_Repos;
using CAA_Event_Management.Data.Repos;
/******************************
*  Created By: Brian Culp
*  Edited by:
*******************************/

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
namespace CAA_Event_Management
{
    /// <summary>
    /// Frame for User Details
    /// </summary>
    public sealed partial class UserDetails : Page
    {
        UserAccount view;
        private bool editMode = true;

        IUserAccountRepository userRepository;

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

            this.DataContext = view;

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

            //show username and password
            txtUserName.Text = view.UserName;
            txtPassword.Text = view.Password;
        }

        /// <summary>
        /// Method to set text boxes to read only if not in edit mode
        /// </summary>
        /// <param name="status"></param>
        private void EditMode(bool status)
        {
            txtFirstName.IsReadOnly = !status;
            txtLastName.IsReadOnly = !status;
            txtPassword.IsReadOnly = !status;
            txtUserName.IsReadOnly = !status;   
        }
    }
}
