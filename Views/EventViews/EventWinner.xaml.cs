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
using CAA_Event_Management.Models;
using CAA_Event_Management.Data;
using CAA_Event_Management.Data.Interface_Repos;
using CAA_Event_Management.Data.Repos;
using Windows.UI.Xaml.Media.Animation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CAA_Event_Management.Views.EventViews
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EventWinner : Page
    {
        #region Startup - variables, repositories, methods

        Event view;

        List<AttendanceTracking> allAttendants = new List<AttendanceTracking>();
        List<AttendanceTracking> nonMembersOnly = new List<AttendanceTracking>();
        List<AttendanceTracking> membersOnly = new List<AttendanceTracking>();
        List<AttendanceTracking> allAttendantsWithGames = new List<AttendanceTracking>();
        List<AttendanceTracking> nonMembersOnlyWithGames = new List<AttendanceTracking>();
        List<AttendanceTracking> membersOnlyWithGames = new List<AttendanceTracking>();
        List<AttendanceTracking> winners = new List<AttendanceTracking>();

        List<AttendanceTracking> allAttendantsWinners = new List<AttendanceTracking>();
        List<AttendanceTracking> nonMembersOnlyWinners = new List<AttendanceTracking>();
        List<AttendanceTracking> membersOnlyWinners = new List<AttendanceTracking>();
        List<AttendanceTracking> allAttendantsWithGamesWinners = new List<AttendanceTracking>();
        List<AttendanceTracking> nonMembersOnlyWithGamesWinners = new List<AttendanceTracking>();
        List<AttendanceTracking> membersOnlyWithGamesWinners = new List<AttendanceTracking>();

        IAttendanceTrackingRepository attendanceTrackingRepository;
        IEventGameUserAnswerRepository eventGameUserAnswerRepository;

        public EventWinner()
        {
            this.InitializeComponent();
            attendanceTrackingRepository = new AttendanceTrackingRepository();
            eventGameUserAnswerRepository = new EventGameUserAnswerRepository();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            loadingRing.IsActive = true;
            lstWinnersList.Visibility = Visibility.Collapsed;
            loadingRing.Visibility = Visibility.Visible;

            view = (Event)e.Parameter;
            ((Window.Current.Content as Frame).Content as MainPage).ChangeMainPageTitleName("SELECT EVENT WINNER");
            txtEventName.Text = view.DisplayName;
            FillListsWithEntries();
            FillListsWithGamePlayers();
            FillListOfEventWinners();

            loadingRing.IsActive = false;
            loadingRing.Visibility = Visibility.Collapsed;
            lstWinnersList.Visibility = Visibility.Visible;
        }

        #endregion

        #region Buttons - ChooseWinner, RadioButton Changed, Checkbox Changed

        /// <summary>
        /// This method handles the "Choose a Winner" button click by the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChooseWinner_Click(object sender, RoutedEventArgs e)
        {
            AttendanceTracking person = new AttendanceTracking();
            List<AttendanceTracking> chosenList = new List<AttendanceTracking>();
            int listChoice = 0;

            try
            {
                List<AttendanceTracking> userSelectedList = new List<AttendanceTracking>();

                if (ckbOnlyQuizPlayers.IsChecked == true)
                {
                    if (rdoNonMembers.IsChecked == true)
                    {
                        userSelectedList = nonMembersOnlyWithGames;
                        chosenList = nonMembersOnlyWithGamesWinners;
                        listChoice = 1;
                    }
                    else if (rdoMemberOnly.IsChecked == true)
                    {
                        userSelectedList = membersOnlyWithGames;
                        chosenList = membersOnlyWithGamesWinners;
                        listChoice = 2;
                    }
                    else
                    {
                        userSelectedList = allAttendantsWithGames;
                        chosenList = allAttendantsWithGamesWinners;
                        listChoice = 3;
                    }
                }
                else
                {
                    if (rdoNonMembers.IsChecked == true)
                    {
                        userSelectedList = nonMembersOnly;
                        chosenList = nonMembersOnlyWinners;
                        listChoice = 4;
                    }
                    else if (rdoMemberOnly.IsChecked == true)
                    {
                        userSelectedList = membersOnly;
                        chosenList = membersOnlyWinners;
                        listChoice = 5;
                    }
                    else
                    {
                        userSelectedList = allAttendants;
                        chosenList = allAttendantsWinners;
                        listChoice = 6;
                    }
                }

                if (userSelectedList.Count == 0)
                {
                    Jeeves.ShowMessage("Error", "Please chose different selection options as there are no available entries");
                    return;
                }
                if (userSelectedList.Count <= chosenList.Count)
                {
                    Jeeves.ShowMessage("Error", "No more winners can be selected from the chosen options, as all entries have been selected");
                    return;
                }

                bool check = false;
                while (check == false)
                {
                    int randomNumber = GetRandomNumber(userSelectedList.Count);
                    person = userSelectedList[randomNumber];

                    AttendanceTracking numCheck = winners
                                .Where(c => c.MemberNo == person.MemberNo)
                                .FirstOrDefault();
                    AttendanceTracking phoneCheck = winners
                                .Where(c => c.PhoneNo == person.PhoneNo)
                                .FirstOrDefault();
                    if (numCheck == null || phoneCheck == null) check = true;
                }

                person.IsAnEventWinner = (winners.Count) + 1;
                attendanceTrackingRepository.UpdateAttendanceTracking(person);

                if (listChoice == 1)
                {
                    nonMembersOnlyWithGamesWinners.Add(person);
                }
                else if (listChoice == 2)
                {
                    membersOnlyWithGamesWinners.Add(person);
                }
                else if (listChoice == 3)
                {
                    allAttendantsWithGamesWinners.Add(person);
                }
                else if (listChoice == 4)
                {
                    nonMembersOnlyWinners.Add(person);
                }
                else if (listChoice == 5)
                {
                    membersOnlyWinners.Add(person);
                }
                else if (listChoice == 6)
                {
                    allAttendantsWinners.Add(person);
                }

                txtWinnerInfo.Text = "Winner Information:";
                if (person.FirstName != null)
                {
                    if (person.FirstName != "") txtWinnerInfo.Text += Environment.NewLine + "Name: " + person.FirstName;
                }
                if (person.LastName != null)
                {
                    if (person.LastName != "")
                    {
                        if (txtWinnerInfo.Text.Contains("Name")) txtWinnerInfo.Text += " " + person.LastName;
                        else txtWinnerInfo.Text += Environment.NewLine + "Last Name: " + person.LastName;
                    }
                }
                if (person.MemberNo != null)
                {
                    if (person.MemberNo != "") txtWinnerInfo.Text += Environment.NewLine + "CAA Number: " + person.MemberNo;
                }
                if (person.PhoneNo != null)
                {
                    if (person.PhoneNo != "") txtWinnerInfo.Text += Environment.NewLine + "Phone: " + person.PhoneNo.Substring(0, 3) + "-" + person.PhoneNo.Substring(3, 3) + "-" + person.PhoneNo.Substring(6);
                }

                FillListOfEventWinners();
            }
            catch
            {
                Jeeves.ShowMessage("Error", "The was a problem getting your winner");
            }
        }

        /// <summary>
        /// This method handles the user interaction with any radio button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoNewRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            ShowNumberOfEntriesText();
        }

        /// <summary>
        /// This method handles the user interaction with the "quiz only" checkbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckbOnlyQuizPlayers_Click(object sender, RoutedEventArgs e)
        {
            ShowNumberOfEntriesText();
        }

        /// <summary>
        /// This method handles the user click on the "back to events" button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReturntoEvents_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CAAEvents), null, new SuppressNavigationTransitionInfo());
        }

        #endregion

        #region Helper Methods - FillListsWithEntries, FillListsWithGamePlayers, ShowNumberOfEntriesText, FillListOfEventWinners, GetRandomNumber

        /// <summary>
        /// This method fills the various lists on the startup of the page; the lists provide the attendanceTracking objects 
        /// that the winners are drawn from; this method filters out duplicate entries based on CAA member numbers or 
        /// provided telephone numbers (not by names)
        /// </summary>
        private void FillListsWithEntries()
        {
            try
            {
                List<AttendanceTracking> attendanceTrackings = attendanceTrackingRepository.GetAttendanceTrackingByEvent(view.EventID);

                //Building the nonMember Entry List
                List<AttendanceTracking> tempNonMembers = attendanceTrackings
                                    .Where(c => c.IsMember == "false")
                                    .Distinct()
                                    .ToList();
                foreach (var x in tempNonMembers)
                {
                    List<AttendanceTracking> checkPhone1 = nonMembersOnly
                                                    .Where(c => c.PhoneNo == x.PhoneNo)
                                                    .ToList();
                    if (checkPhone1.Count == 0) nonMembersOnly.Add(x);
                    if (x.IsAnEventWinner != null) nonMembersOnlyWinners.Add(x);
                }

                //Building the memberOnly Entry List
                List<AttendanceTracking> tempMembersOnly = attendanceTrackings
                                    .Where(c => c.IsMember == "true")
                                    .Distinct()
                                    .ToList();
                foreach (var x in tempMembersOnly)
                {
                    List<AttendanceTracking> checkMemNum = membersOnly
                                                    .Where(c => c.MemberNo == x.MemberNo)
                                                    .ToList();
                    List<AttendanceTracking> checkPhone = membersOnly
                                                    .Where(c => c.PhoneNo == x.PhoneNo)
                                                    .ToList();
                    if (checkMemNum.Count == 0 && checkPhone.Count == 0) membersOnly.Add(x);
                    if (x.IsAnEventWinner != null) membersOnlyWinners.Add(x);
                }

                //Building the allAttendance Entries List
                List<AttendanceTracking> tempAllEntries = attendanceTrackings
                                    .Distinct()
                                    .ToList();
                foreach (var x in tempAllEntries)
                {
                    List<AttendanceTracking> checkMemNum = allAttendants
                                                    .Where(c => c.MemberNo == x.MemberNo)
                                                    .ToList();
                    List<AttendanceTracking> checkPhone = allAttendants
                                                    .Where(c => c.PhoneNo == x.PhoneNo)
                                                    .ToList();
                    if (checkMemNum.Count == 0 && checkPhone.Count == 0) allAttendants.Add(x);
                    if (x.IsAnEventWinner != null) allAttendantsWinners.Add(x);
                }
            }
            catch
            {
                Jeeves.ShowMessage("Error", "There was a problem connecting to the database; please exit and restart the program");
            }
            tbkTotalNumberOfEntries.Text = "Total number of entries: " + allAttendants.Count().ToString();
        }

        /// <summary>
        /// This method determines which winner entries (attendanceTracking objects) have played the quiz game; it separates 
        /// them into separate lists determined by what catagory the play falls into
        /// </summary>
        private void FillListsWithGamePlayers()
        {
            try
            {
                List<EventGameUserAnswer> eventGameUserAnswers = eventGameUserAnswerRepository.GetEventGameUserAnswers(view.EventID);

                foreach(var x in allAttendants)
                {
                    List<EventGameUserAnswer> check = eventGameUserAnswers
                                        .Where(p => p.AttendantID == x.MemberAttendanceID)
                                        .ToList();
                    if (check.Count > 0) allAttendantsWithGames.Add(x);
                    if (x.IsAnEventWinner != null) allAttendantsWithGamesWinners.Add(x);
                }

                foreach (var x in nonMembersOnly)
                {
                    List<EventGameUserAnswer> check = eventGameUserAnswers
                                        .Where(p => p.AttendantID == x.MemberAttendanceID)
                                        .ToList();
                    if (check.Count > 0) nonMembersOnlyWithGames.Add(x);
                    if (x.IsAnEventWinner != null) nonMembersOnlyWithGamesWinners.Add(x);
                }

                foreach (var x in membersOnly)
                {
                    List<EventGameUserAnswer> check = eventGameUserAnswers
                                        .Where(p => p.AttendantID == x.MemberAttendanceID)
                                        .ToList();
                    if (check.Count > 0) membersOnlyWithGames.Add(x);
                    if (x.IsAnEventWinner != null) membersOnlyWithGamesWinners.Add(x);
                }
            }
            catch {  }
        }

        /// <summary>
        /// This method determines and displays the total number of available entries for the various radio buttons and quiz 
        /// checkbox as chosen by the user for the selected event; it displays this information in the view
        /// </summary>
        private void ShowNumberOfEntriesText()
        {
            if (ckbOnlyQuizPlayers == null)
            {
                if (rdoAllMembers.IsChecked == true)
                {
                    tbkTotalNumberOfEntries.Text = "Total number of entries: " + allAttendants.Count().ToString();
                }
                else if (rdoMemberOnly.IsChecked == true)
                {
                    tbkTotalNumberOfEntries.Text = "Total number of entries: " + membersOnly.Count().ToString();
                }
                else if (rdoNonMembers.IsChecked == true)
                {
                    tbkTotalNumberOfEntries.Text = "Total number of entries: " + nonMembersOnly.Count().ToString();
                }
            }
            else
            {
                if (ckbOnlyQuizPlayers.IsChecked == true && rdoAllMembers.IsChecked == true)
                {
                    tbkTotalNumberOfEntries.Text = "Total number of entries: " + allAttendantsWithGames.Count().ToString();
                }
                else if (ckbOnlyQuizPlayers.IsChecked == true && rdoMemberOnly.IsChecked == true)
                {
                    tbkTotalNumberOfEntries.Text = "Total number of entries: " + membersOnlyWithGames.Count().ToString();
                }
                else if (ckbOnlyQuizPlayers.IsChecked == true && rdoNonMembers.IsChecked == true)
                {
                    tbkTotalNumberOfEntries.Text = "Total number of entries: " + nonMembersOnlyWithGames.Count().ToString();
                }
                else if (ckbOnlyQuizPlayers.IsChecked == false && rdoAllMembers.IsChecked == true)
                {
                    tbkTotalNumberOfEntries.Text = "Total number of entries: " + allAttendants.Count().ToString();
                }
                else if (ckbOnlyQuizPlayers.IsChecked == false && rdoMemberOnly.IsChecked == true)
                {
                    tbkTotalNumberOfEntries.Text = "Total number of entries: " + membersOnly.Count().ToString();
                }
                else if (ckbOnlyQuizPlayers.IsChecked == false && rdoNonMembers.IsChecked == true)
                {
                    tbkTotalNumberOfEntries.Text = "Total number of entries: " + nonMembersOnly.Count().ToString();
                }
            }
        }

        /// <summary>
        /// This method fills the winner list or determines that no winners have been selected; it displays this 
        /// information in the view
        /// </summary>
        private void FillListOfEventWinners()
        {
            bool check = false;
            List<string> winnerInfoList = new List<string>();
            winners = attendanceTrackingRepository.GetAttendanceTrackingByEvent(view.EventID)
                            .Where(p => p.IsAnEventWinner != null)
                            .OrderBy(p => p.IsAnEventWinner)
                            .ThenBy(p => p.FirstName)
                            .ToList();

            if(winners.Count == 0)
            {
                check = true;
                tbkNumberOfWinners.Text = "Selected Event Winners: " + winnerInfoList.Count;
                winnerInfoList.Add("No Event Winners Selected");
            }
            else
            {
                foreach(var x in winners)
                {
                    string winnerInfo = x.IsAnEventWinner.ToString() + ".";
                    if (x.FirstName != null)
                    {
                        if (x.FirstName != "") winnerInfo += " " + x.FirstName;
                    }
                    if (x.LastName != null) 
                    {
                        if(x.LastName != "") winnerInfo += " " + x.LastName;
                    }
                    if (x.MemberNo != null)
                    {
                        if (x.MemberNo != "") winnerInfo += Environment.NewLine + "CAA Number: " + x.MemberNo;
                    }
                    if (x.PhoneNo != null)
                    {
                        if (x.PhoneNo != "") winnerInfo += Environment.NewLine + "Ph: " + x.PhoneNo.Substring(0, 3) + "-" + x.PhoneNo.Substring(3, 3) + "-" + x.PhoneNo.Substring(6) + "\n";
                    }
                    winnerInfoList.Add(winnerInfo);
                }
            }
            lstWinnersList.ItemsSource = winnerInfoList;
            if (check == false)
            {
                tbkNumberOfWinners.Text = "Selected Event Winners: " + winnerInfoList.Count;
            }
        }

        /// <summary>
        /// This method provides a random number which is used to determine an event winner; the parameter
        /// is the number of entries in a particular drawing pool from an event
        /// </summary>
        /// <param name="count">this parameter is the number of entries in a particular drawing pool</param>
        /// <returns></returns>
        private int GetRandomNumber(int count)
        {
            Random random = new Random();
            int number = random.Next(0, count);
            return number;
        }

        #endregion

    }
}
