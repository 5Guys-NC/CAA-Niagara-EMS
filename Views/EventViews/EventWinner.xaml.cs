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
        Event view;

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
        }

        private void btnChooseWinner_Click(object sender, RoutedEventArgs e)
        {
            List<AttendanceTracking> currentListOfPeople = new List<AttendanceTracking>();
            AttendanceTracking person = new AttendanceTracking();

            try
            {
                List<AttendanceTracking> attendanceTrackings = attendanceTrackingRepository.GetAttendanceTrackingByEvent(view.EventID);
                List<EventGameUserAnswer> eventGameUserAnswers = eventGameUserAnswerRepository.GetEventGameUserAnswers(view.EventID);

                if (attendanceTrackings.Count == 0)
                {
                    Jeeves.ShowMessage("Error", "Please chose a different event; things event has no entered data");
                    return;
                }
                else if (ckbOnlyQuizPlayers.IsChecked == true && eventGameUserAnswers.Count == 0)
                {
                    Jeeves.ShowMessage("Error", "Please unselect 'Only Include Quiz Players as this event has no quiz data");
                    return;
                }

                if (rdoNonMembers.IsChecked == true)
                {
                    List<AttendanceTracking> temp = attendanceTrackings
                                            .Where(c => c.IsMember == "false")
                                            .Distinct()
                                            .ToList();
                    foreach (var x in temp)
                    {
                        List<AttendanceTracking> checkPhone = currentListOfPeople
                                                        .Where(c => c.PhoneNo == x.PhoneNo)
                                                        .ToList();
                        if (checkPhone.Count == 0) currentListOfPeople.Add(x);
                    }

                }
                else if (rdoMemberOnly.IsChecked == true)
                {
                    List<AttendanceTracking> temp = attendanceTrackings
                        .Where(c => c.IsMember == "true")
                        .Distinct()
                        .ToList();
                 
                    foreach (var x in temp)
                    {
                        List<AttendanceTracking> checkMemNum = currentListOfPeople
                                                        .Where(c => c.MemberNo == x.MemberNo)
                                                        .ToList();
                        List<AttendanceTracking> checkPhone = currentListOfPeople
                                                        .Where(c => c.PhoneNo == x.PhoneNo)
                                                        .ToList();
                        if (checkMemNum.Count == 0 && checkPhone.Count == 0) currentListOfPeople.Add(x);
                    }
                }
                else
                {
                    List<AttendanceTracking> temp = attendanceTrackings
                                            .Distinct()
                                            .ToList();
                    foreach (var x in temp)
                    {
                        List<AttendanceTracking> checkMemNum = currentListOfPeople
                                                        .Where(c => c.MemberNo == x.MemberNo)
                                                        .ToList();
                        List<AttendanceTracking> checkPhone = currentListOfPeople
                                                        .Where(c => c.PhoneNo == x.PhoneNo)
                                                        .ToList();
                        if (checkMemNum.Count == 0 && checkPhone.Count == 0) currentListOfPeople.Add(x);
                    }
                }

                if (ckbOnlyQuizPlayers.IsChecked == true)
                {
                    bool check = false;

                    while (check == false)
                    {
                        int randomNumber = GetRandomNumber(currentListOfPeople.Count);

                        person = currentListOfPeople[randomNumber];
                        List<EventGameUserAnswer> playedGame = eventGameUserAnswers
                                                    .Where(c => c.AttendantID == person.MemberAttendanceID)
                                                    .ToList();
                        if (playedGame.Count > 0) check = true;
                    }
                }
                else
                {
                    int randomNumber = GetRandomNumber(currentListOfPeople.Count);
                    person = currentListOfPeople[randomNumber];
                }

                txtWinnerInfo.Text = "Member Number: " + person.MemberNo +
                                   "/nPerson Name: " + person.FirstName + " " + person.LastName +
                                   "/nPerson Phone: " + person.PhoneNo;

            }
            catch
            {
                Jeeves.ShowMessage("Error", "The was a problem getting your winner");
            }
        }

        private int GetRandomNumber(int count)
        {
            Random random = new Random();
            int number = random.Next(0, count);
            return number;
        }

        private void rdoAllMembers_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void rdoNonMembers_Checked(object sender, RoutedEventArgs e)
        {

        }

    }
}
