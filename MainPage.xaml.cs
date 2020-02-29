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
using CAA_Event_Management.Views.Games;
using CAA_Event_Management.Views;
using CAA_Event_Management.Views.Event;
//using CAA_Event_Management.Views.Event;
using Windows.System;
using CAA_Event_Management.Models;
using CAA_Event_Management.Data;
//Edited By: Nathan Smith
//Edited By: Brian Culp
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CAA_Event_Management
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        DispatcherTimer Timer = new DispatcherTimer();
        Users currentUser;
        IUsersRepository usersRepository;
        bool s;

        public MainPage()
        {
            this.InitializeComponent();
            usersRepository = new UsersRepository();
            isAuthenticated(out s);
            MyFrame.Navigate(typeof(EventStartView));
            DataContext = this;
            Timer.Tick += Timer_Tick;
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();
        }
        //Clock code adapted from https://stackoverflow.com/questions/38562704/make-clock-uwp-c, posted by Frauke and edited by Mafii
        private void Timer_Tick(object sender, object e)
        {
            Time.Text = DateTime.Now.ToString("h:mm tt");
        }

        //private void btnSignIn_PointerEntered(object sender, PointerRoutedEventArgs e)
        //{
        //    btnSignIn.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 255));
        //}

        //private void btnSignIn_PointerExited(object sender, PointerRoutedEventArgs e)
        //{
        //    btnSignIn.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 0, 0));
        //}

        private void NavView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if (MyFrame.CanGoBack)
            {
                MyFrame.GoBack();
            }
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            string item = args.InvokedItem.ToString();
            if (item != null)
            {
                switch (item)
                {
                    case "Home":
                        MyFrame.Navigate(typeof(EventStartView));
                        break;

                    case "Events":
                        MyFrame.Navigate(typeof(CAAEvents));
                        break;

                    case "Games":
                        MyFrame.Navigate(typeof(GameMenu));
                        break;

                    case "Users":
                        MyFrame.Navigate(typeof(UsersSummary));
                        break;

                    case "Surveys":
                        MyFrame.Navigate(typeof(ItemsView));
                        break;

                    case "Sign Out":
                        SignUserOut();
                        break;
                }
            }
        }

        private void BtnSignIn_Click(object sender, RoutedEventArgs e)
        {
            //if username is empty
            if (txtUserName.Text == "" || txtUserName.Text == null)
            {
                txttest.Text = "Username empty";
                return;
            }
            //if password is empty
            if (txtPassword.Password == "" || txtPassword.Password == null)
            {
                txttest.Text = "Password empty";
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
                        txttest.Text = "Password does not match";
                    }

                }
                else
                {
                    txttest.Text = "User Not Found";
                }
            }
            catch
            {
                txttest.Text = "Error connecting to Database";
                throw;
            }
        }

        //check is user is signed in and authenticated, show or hide the corresponding items
        public void isAuthenticated(out bool status)
        {
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
                MyFrame.Navigate(typeof(UsersSummary));
            }
            else
            {
                status = true;

                //restricted rows visible
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

        private void SignUserOut()
        {
            //set current user to null
            currentUser = null;

            //show sign in button in header, hide welcome message
            btnSignInFlyout.Visibility = Visibility.Visible;
            txtUser.Visibility = Visibility.Collapsed;

            isAuthenticated(out s);
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

            isAuthenticated(out s);
        }
    }
}
