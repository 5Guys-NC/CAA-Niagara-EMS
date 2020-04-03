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
            view = (Event)e.Parameter;
            ((Window.Current.Content as Frame).Content as MainPage).ChangeMainPageTitleName("SELECT EVENT WINNER");
            txtEventName.Text = view.DisplayName;
            FillListsWithEntries();
            FillListsWithGamePlayers();
            FillListOfEventWinners();
        }

        #endregion

        #region Buttons - ChooseWinner, RadioButton Changed, Checkbox Changed

        private void btnChooseWinner_Click(object sender, RoutedEventArgs e)
        {
            AttendanceTracking person = new AttendanceTracking();
            
            try
            {
                List<AttendanceTracking> userSelectedList = new List<AttendanceTracking>();

                if (ckbOnlyQuizPlayers.IsChecked == true)
                {
                    if (rdoNonMembers.IsChecked == true)
                    {
                        userSelectedList = nonMembersOnlyWithGames;
                    }
                    else if (rdoMemberOnly.IsChecked == true)
                    {
                        userSelectedList = membersOnlyWithGames;
                    }
                    else
                    {
                        userSelectedList = allAttendantsWithGames;
                    }
                }
                else
                {
                    if (rdoNonMembers.IsChecked == true)
                    {
                        userSelectedList = nonMembersOnly;
                    }
                    else if (rdoMemberOnly.IsChecked == true)
                    {
                        userSelectedList = membersOnly;
                    }
                    else
                    {
                        userSelectedList = allAttendants;
                    }
                }

                if (userSelectedList.Count == 0)
                {
                    Jeeves.ShowMessage("Error", "Please chose different selection options as there are no available entries");
                    return;
                }
                if (userSelectedList.Count <= winners.Count)
                {
                    Jeeves.ShowMessage("Error", "No extra winners can be selected for this event, as all entries have been selected");
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

                    //List<AttendanceTracking> winnerListCheck = winners
                    //                .Where(p => p.MemberAttendanceID == person.MemberAttendanceID)
                    //                .ToList();
                    //if (winnerListCheck.Count == 0) check = true;
                }

                person.IsAnEventWinner = true;
                attendanceTrackingRepository.UpdateAttendanceTracking(person);

                txtWinnerInfo.Text = "Member Number: " + person.MemberNo +
                                   "\nPerson Name: " + person.FirstName + " " + person.LastName +
                                   "\nPerson Phone: " + person.PhoneNo;
                FillListOfEventWinners();
            }
            catch
            {
                Jeeves.ShowMessage("Error", "The was a problem getting your winner");
            }
        }

        private void rdoNewRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            ShowNumberOfEntriesText();
        }

        private void ckbOnlyQuizPlayers_Click(object sender, RoutedEventArgs e)
        {
            ShowNumberOfEntriesText();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CAAEvents));
        }

        #endregion

        #region Helper Methods - FillListsWithEntries, FillListsWithGamePlayers, ShowNumberOfEntriesText, FillListOfEventWinners, GetRandomNumber

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
                }
            }
            catch
            {
                Jeeves.ShowMessage("Error", "There was a problem connecting to the database; please exit and restart the program");
            }
            tbkTotalNumberOfEnteries.Text = "Total number of enteries: " + allAttendants.Count().ToString();
        }

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
                }

                foreach (var x in nonMembersOnly)
                {
                    List<EventGameUserAnswer> check = eventGameUserAnswers
                                        .Where(p => p.AttendantID == x.MemberAttendanceID)
                                        .ToList();
                    if (check.Count > 0) nonMembersOnlyWithGames.Add(x);
                }

                foreach (var x in membersOnly)
                {
                    List<EventGameUserAnswer> check = eventGameUserAnswers
                                        .Where(p => p.AttendantID == x.MemberAttendanceID)
                                        .ToList();
                    if (check.Count > 0) membersOnlyWithGames.Add(x);
                }
            }
            catch {  }
        }

        private void ShowNumberOfEntriesText()
        {
            if (ckbOnlyQuizPlayers == null)
            {
                if (rdoAllMembers.IsChecked == true)
                {
                    tbkTotalNumberOfEnteries.Text = "Total number of enteries: " + allAttendants.Count().ToString();
                }
                else if (rdoMemberOnly.IsChecked == true)
                {
                    tbkTotalNumberOfEnteries.Text = "Total number of enteries: " + membersOnly.Count().ToString();
                }
                else if (rdoNonMembers.IsChecked == true)
                {
                    tbkTotalNumberOfEnteries.Text = "Total number of enteries: " + nonMembersOnly.Count().ToString();
                }
            }
            else
            {
                if (ckbOnlyQuizPlayers.IsChecked == true && rdoAllMembers.IsChecked == true)
                {
                    tbkTotalNumberOfEnteries.Text = "Total number of enteries: " + allAttendantsWithGames.Count().ToString();
                }
                else if (ckbOnlyQuizPlayers.IsChecked == true && rdoMemberOnly.IsChecked == true)
                {
                    tbkTotalNumberOfEnteries.Text = "Total number of enteries: " + membersOnlyWithGames.Count().ToString();
                }
                else if (ckbOnlyQuizPlayers.IsChecked == true && rdoNonMembers.IsChecked == true)
                {
                    tbkTotalNumberOfEnteries.Text = "Total number of enteries: " + nonMembersOnlyWithGames.Count().ToString();
                }
                else if (ckbOnlyQuizPlayers.IsChecked == false && rdoAllMembers.IsChecked == true)
                {
                    tbkTotalNumberOfEnteries.Text = "Total number of enteries: " + allAttendants.Count().ToString();
                }
                else if (ckbOnlyQuizPlayers.IsChecked == false && rdoMemberOnly.IsChecked == true)
                {
                    tbkTotalNumberOfEnteries.Text = "Total number of enteries: " + membersOnly.Count().ToString();
                }
                else if (ckbOnlyQuizPlayers.IsChecked == false && rdoNonMembers.IsChecked == true)
                {
                    tbkTotalNumberOfEnteries.Text = "Total number of enteries: " + nonMembersOnly.Count().ToString();
                }
            }
        }

        private void FillListOfEventWinners()
        {
            List<string> winnerInfoList = new List<string>();
            winners = attendanceTrackingRepository.GetAttendanceTrackingByEvent(view.EventID)
                            .Where(p => p.IsAnEventWinner == true)
                            .ToList();
            if(winners.Count == 0)
            {
                winnerInfoList.Add("No Event Winners Selected");
            }
            else
            {
                foreach(var x in winners)
                {
                    string winnerInfo = "";
                    if (x.FirstName != "")
                    {
                        winnerInfo += x.FirstName;
                    }
                    if (x.LastName != "") 
                    {
                        if (winnerInfo == "") winnerInfo += x.LastName;
                        else winnerInfo += " " + x.LastName;
                    }
                    if (x.MemberNo != "")
                    {
                        if (winnerInfo == "") winnerInfo += "CAA Number: " + x.MemberNo;
                        else winnerInfo += "  -  CAA Number: " + x.MemberNo;
                    }
                    if (x.PhoneNo != "")
                    {
                        if (winnerInfo == "") winnerInfo += "Ph: " + x.PhoneNo;
                        else winnerInfo += "  -  Ph: " + x.PhoneNo;
                    }
                    winnerInfoList.Add(winnerInfo);
                }
            }
            lstWinnersList.ItemsSource = winnerInfoList;
            tbkNumberOfWinners.Text = "Selected Event Winners: " + winnerInfoList.Count;
        }

        private int GetRandomNumber(int count)
        {
            Random random = new Random();
            int number = random.Next(0, count);
            return number;
        }

        #endregion

    }
}
