using CAA_Event_Management.Data;
using CAA_Event_Management.Models;
using CAA_Event_Management.Views.Event;
using CAA_Event_Management.Views.Games;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
/***********************************
* Edited By: Nathan Smith
* Edited By: Brian Culp
***********************************/

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
namespace CAA_Event_Management
{
    /// <summary>
    /// The code behind Main Page (things visible on all pages, with a changing frame)
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region Startup - variables, repositories, constructor
            Users currentUser;
            IUsersRepository usersRepository;
            bool AuthStatus;

            public MainPage()
            {
                this.InitializeComponent();
                usersRepository = new UsersRepository();
                isAuthenticated(out AuthStatus);
                MyFrame.Navigate(typeof(EventStartView));
                DataContext = this;
            }
        #endregion

        #region Button - Click events
            /// <summary>
            /// Functionality for Back Button
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="args"></param>
            private void NavView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
            {
                //go back to previous frame
                if (MyFrame.CanGoBack)
                {
                    MyFrame.GoBack();
                }
            }

            /// <summary>
            /// Functionality for the Navigation menu.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="args"></param>
            private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
            {
                //get the item that invoked the method and convert to string
                string item = args.InvokedItem.ToString();
            
                //if the item is not null
                if (item != null)
                {
                    switch (item)
                    {
                        //For Home Button
                        case "Home":
                            MyFrame.Navigate(typeof(EventStartView));
                            break;

                        //For Events Button
                        case "Events":
                            MyFrame.Navigate(typeof(CAAEvents));
                            break;

                        //For Games Button
                        case "Games":
                            MyFrame.Navigate(typeof(GameMenu));
                            break;

                        //For Users Button
                        case "Users":
                            MyFrame.Navigate(typeof(UsersSummary));
                            break;

                        //For Surveys Button
                        case "Surveys":
                            MyFrame.Navigate(typeof(ItemsView));
                            break;

                        //For Sign Out Button
                        case "Sign Out":
                            SignUserOut();
                            break;
                    }
                }
            }

            /// <summary>
            /// Functionality for Sign In button
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void BtnSignIn_Click(object sender, RoutedEventArgs e)
            {
                //if username is empty
                if (txtUserName.Text == "" || txtUserName.Text == null)
                {
                    Jeeves.ShowMessage("Error", "UserName can not be empty");
                    return;
                }

                //if password is empty
                if (txtPassword.Password == "" || txtPassword.Password == null)
                {
                    Jeeves.ShowMessage("Error", "Password can not be empty");
                    return;
                }

                try
                {
                    //try to find user in db
                    currentUser = usersRepository.GetUser(txtUserName.Text);

                    //if user exists
                    if (currentUser != null)
                    {
                        //if password matches one in db
                        if (currentUser.Password == txtPassword.Password)
                        {
                            //sign user in
                            SignUserIn();
                        }
                        else
                        {
                            //incorrect password
                            Jeeves.ShowMessage("Error", "Password does not match the one on file");
                        }

                    }
                    else
                    {
                        //user doesnt exist
                        Jeeves.ShowMessage("Error", "User could not be found");
                    }
                }
                catch
                {
                    //cant find db
                    Jeeves.ShowMessage("Error", "Could not connect to the Database");
                    throw;
                }
            }
        #endregion

        #region Helper Methods
            /// <summary>
            /// Check is user is signed in and authenticated, show or hide the corresponding items
            /// </summary>
            /// <param name="status"></param>
            public void isAuthenticated(out bool status)
            {
                //if nobody is signed in
                if (currentUser == null)
                {
                    status = false;
                
                    //collapse restricted navs
                    EventsLink.Visibility = Visibility.Collapsed;
                    GamesLink.Visibility = Visibility.Collapsed;
                    SurveysLink.Visibility = Visibility.Collapsed;
                    SignOutLink.Visibility = Visibility.Collapsed;
                    UsersLink.Visibility = Visibility.Collapsed;

                    //navigate to home page
                    MyFrame.Navigate(typeof(CAAEvents));
                }
                else
                {
                    status = true;

                    //restricted rows become visible
                    EventsLink.Visibility = Visibility.Visible;
                    GamesLink.Visibility = Visibility.Visible;
                    SurveysLink.Visibility = Visibility.Visible;
                    SignOutLink.Visibility = Visibility.Visible;

                    //if user has admin rights, show those restricted views
                    if (currentUser.isAdmin == true)
                    {
                        UsersLink.Visibility = Visibility.Visible;
                    }
                }
            }

            /// <summary>
            /// This is called to complete process for signing a user out
            /// </summary>
            private void SignUserOut()
            {
                //set current user to null
                currentUser = null;

                //show sign in button in header, hide welcome message
                btnSignInFlyout.Visibility = Visibility.Visible;
                txtUser.Visibility = Visibility.Collapsed;

                //check authentication of new null user (will return false and restrict to not signed in status)
                isAuthenticated(out AuthStatus);
            }

            private void SignUserIn()
            {
                //hide flyout
                flySignin.Hide();

                //hide sign in button, show welcome message in header
                btnSignInFlyout.Visibility = Visibility.Collapsed;
                txtUser.Visibility = Visibility.Visible;

                //set welcome message text
                txtUser.Text = "Hello, " + currentUser.UserName;

                //check authentication of new user (will return true and show restricted navs)
                isAuthenticated(out AuthStatus);
            }
        #endregion
    }
}
