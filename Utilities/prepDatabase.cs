using CAA_Event_Management.Data;
using CAA_Event_Management.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
/******************************
*  Created By: Jon Yade
*  Edited by: Brian Culp
*  Edited by: Max Cashmore
*  Edited by: Nathan Smith
*******************************/
namespace CAA_Event_Management.Utilities
{
    /// <summary>
    /// This page contains all of the Database initial preparation,
    /// including migration and seeding.
    /// </summary>
    public static class prepDatabase
    {
        public static void Initial()
        {
            using (CAAContext context = new CAAContext())
            {
                #region Database Migration

                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    if (!ex.Message.Contains("already"))
                    {
                        throw;
                    }
                }

                #endregion

                #region Seed Data

                #region Users

                if (!context.UserAccounts.Any())
                {
                    context.UserAccounts.AddRange(
                     new UserAccount
                     {
                         ID = Guid.NewGuid().ToString(),
                         UserName = "CAAUser1",
                         Password = "password",
                         isAdmin = false
                     },
                    new UserAccount
                    {
                        ID = Guid.NewGuid().ToString(),
                        UserName = "a",
                        Password = "a",
                        isAdmin = true
                    },
                     new UserAccount
                     {
                         ID = Guid.NewGuid().ToString(),
                         FirstName = "CAA",
                         LastName = "Admin",
                         UserName = "admin1",
                         Password = "password",
                         isAdmin = true
                     },
                     new UserAccount
                     {
                         ID = Guid.NewGuid().ToString(),
                         FirstName = "Jon",
                         LastName = "Doe",
                         UserName = "jdoe",
                         Password = "password",
                         isAdmin = false
                     });
                    context.SaveChanges();
                }

                #endregion

                #region Games

                if (!context.Games.Any())
                {
                    context.Games.AddRange(
                     new Game
                     {
                         Title = "Food Fest"
                     },
                     new Game
                     {
                         Title = "Canal Days"
                     });
                    context.SaveChanges();
                }
                if (!context.Questions.Any())
                {
                    context.Questions.AddRange(
                    new Question
                    {
                        Text = "CAA Members save 10% on their bill at which restaurants?",
                        TimesUsed = 1
                    },
                    new Question
                    {
                        Text = "True or False: Members buy one jump pass and get one FREE at Sky Zone Trampoline Park.",
                        TimesUsed = 2
                    }
                    );
                    context.SaveChanges();
                }
                if (!context.Answers.Any())
                {
                    context.Answers.AddRange(
                       new Answer
                       {
                           Text = "Harvey's",
                           TimesUsed = 1
                       },
                       new Answer
                       {
                           Text = "Kelsey's",
                           TimesUsed = 1
                       },
                       new Answer
                       {
                           Text = "Montana's",
                           TimesUsed = 1
                       },
                       new Answer
                       {
                           Text = "All of the Above",
                           TimesUsed = 1
                       },
                       new Answer
                       {
                           Text = "True",
                           TimesUsed = 2
                       },
                       new Answer
                       {
                           Text = "False",
                           TimesUsed = 2
                       }
                       );
                    context.SaveChanges();
                }

                if (!context.GameModels.Any())
                {
                    context.GameModels.AddRange(
                        new GameModel
                        {
                            QuestionText = "CAA Members save 10% on their bill at which restaurants?",
                            OptionsText = "Harvey's|Kelsey's|Montana's|All of the Above",
                            AnswerText = "All of the Above",
                            GameID = 1
                        },
                        new GameModel
                        {
                            QuestionText = "True or False: Members buy one jump pass and get one FREE at Sky Zone Trampoline Park.",
                            OptionsText = "True|False",
                            AnswerText = "True",
                            GameID = 1
                        },
                        new GameModel
                        {
                            QuestionText = "True or False: Members buy one jump pass and get one FREE at Sky Zone Trampoline Park.",
                            OptionsText = "True|False",
                            AnswerText = "True",
                            GameID = 2
                        }
                    );
                }

                #endregion

                #region Events

                if (!context.Events.Any())
                {
                    context.Events.AddRange(
                     new Event
                     {
                         EventID = Guid.NewGuid().ToString(),
                         EventName = "Ribfest",
                         EventStart = DateTime.Today.AddDays(3),
                         AbrevEventname = "RF1",
                         MembersOnly = true,
                         DisplayName = "RibFest"
                     },
                     new Event
                     {
                         EventID = Guid.NewGuid().ToString(),
                         EventName = "Heart & Stroke Fundraiser",
                         EventStart = DateTime.Today.AddDays(-1),
                         AbrevEventname = "HS1",
                         MembersOnly = true,
                         DisplayName = "Heart & Stroke Fundraiser"
                     },
                     new Event
                     {
                         EventID = Guid.NewGuid().ToString(),
                         EventName = "CAA Charity BBQ",
                         EventStart = DateTime.Today.AddDays(7),
                         AbrevEventname = "CCB1",
                         MembersOnly = true,
                         DisplayName = "CAA Charity BBQ"
                     },
                     new Event
                     {
                         EventID = Guid.NewGuid().ToString(),
                         EventName = "Food Truck Wars",
                         EventStart = DateTime.Today,
                         AbrevEventname = "FT1",
                         MembersOnly = false,
                         DisplayName = "Food Truck Wars"
                     },
                     new Event
                     {
                         EventID = Guid.NewGuid().ToString(),
                         EventName = "Wingfest",
                         EventStart = DateTime.Today.AddDays(3),
                         AbrevEventname = "WF1",
                         MembersOnly = true,
                         DisplayName = "WingFest"
                     },
                     new Event
                     {
                         EventID = Guid.NewGuid().ToString(),
                         EventName = "Very Important Gala",
                         EventStart = DateTime.Today.AddDays(1),
                         AbrevEventname = "VIG1",
                         MembersOnly = true,
                         DisplayName = "Very Important Gala"
                     },
                     new Event
                     {
                         EventID = Guid.NewGuid().ToString(),
                         EventName = "Arctic Marathon",
                         EventStart = DateTime.Today.AddDays(30),
                         AbrevEventname = "AM1",
                         MembersOnly = true,
                         DisplayName = "Arctic Marathon"
                     },
                     new Event
                     {
                         EventID = Guid.NewGuid().ToString(),
                         EventName = "Beef Jerky Invitational",
                         EventStart = DateTime.Today.AddDays(-5),
                         AbrevEventname = "BJ1",
                         MembersOnly = false,
                         DisplayName = "Beef Jerky Invitational"
                     },
                     new Event
                     {
                         EventID = Guid.NewGuid().ToString(),
                         EventName = "CAA Christmas Party",
                         EventStart = DateTime.Today.AddDays(5),
                         AbrevEventname = "CP1",
                         MembersOnly = true,
                         DisplayName = "CAA Christmas Party"
                     });
                    context.SaveChanges();
                }

                #endregion

                #region AttendanceTracking - Commented out

                string[] firstNames = new string[] { "Brian", "Jon", "Max", "Nathan", "Oli", "Geri", "Joe", "Kaila", "Richard", "Marsha", "Hunter", "Dave", "Nicholas" };
                string[] lastNames = new string[] { "Culp", "Yade", "Smith", "Cashmore", "Crroj", "Johnson", "Brown", "Henderson", "Anderson", "Doe", "Stovell", "Baddeley", "Kendall" };
                Random random = new Random();

                if (context.AttendanceTrackings.Count() == 0)
                {
                    List<AttendanceTracking> attendees = new List<AttendanceTracking>();
                    
                    //loop to create seed data of swipes into Heart & Stroke event(member only)
                    //all inserts are members with unique member number
                    #region loop for MemberOnly event w/ Unique membership numbers

                    //30 records (members with unique membership numbers)
                    for (int i = 0; i <= 30; i++)
                    {
                        //create 2 sets of 8 digit random numbers and concat together in string
                        //(done in 2 sets to avoid long integer and keep int)
                        string memberNum = random.Next(11111111, 99999999).ToString() + random.Next(11111111, 99999999).ToString();
                        
                        //check if the member number has been used already
                        //if it has, lower i by 1 and repeat the process
                        if(context.AttendanceTrackings.Where(e=>e.MemberNo == memberNum).Count() > 0)
                        {
                            i--;
                            return;
                        }

                        //create new Attendance Tracking
                        AttendanceTracking newAttendee = new AttendanceTracking()
                        {
                            MemberAttendanceID = Guid.NewGuid().ToString(),
                            EventID = context.Events.Where(e => e.EventName == "Heart & Stroke Fundraiser").Select(e => e.EventID).FirstOrDefault(),
                            FirstName = firstNames[random.Next(0, 12)].ToString(),
                            LastName = lastNames[random.Next(0, 12)].ToString(),
                            IsMember = "true",
                            PhoneNo = "905" + random.Next(1111111, 9999999).ToString(),
                            MemberNo = memberNum
                        };
                        attendees.Add(newAttendee);
                    }
                    //add records to database
                    context.AttendanceTrackings.AddRange(attendees);
                    context.SaveChanges();

                    #endregion

                    //clear list of attendees for next insert
                    attendees.Clear();

                    //loop to create records for Beef Jerky Invitational(non member event)
                    #region loop for Non-Member exclusive Event w/members, non-members, and a few duplicates
                    
                    //get eventId for the event we are using
                    string eventid = context.Events.Where(e => e.EventName == "Beef Jerky Invitational").Select(e => e.EventID).FirstOrDefault();

                    //20 member records with unique membership numbers
                    for (int i = 0; i < 20; i++)
                    {
                        //create 2 sets of 8 digit random numbers and concat together in string
                        //(done in 2 sets to avoid long integer and keep int)
                        string memberNum = random.Next(11111111, 99999999).ToString() + random.Next(11111111, 99999999).ToString();

                        //check if the member number has been used already
                        //if it has, lower i by 1 and repeat the process
                        if (context.AttendanceTrackings.Where(e => e.MemberNo == memberNum).Count() > 0)
                        {
                            i--;
                            return;
                        }

                        //create new Attendance Tracking
                        AttendanceTracking newAttendee = new AttendanceTracking()
                        {
                            MemberAttendanceID = Guid.NewGuid().ToString(),
                            EventID = context.Events.Where(e => e.EventName == "Beef Jerky Invitational").Select(e => e.EventID).FirstOrDefault(),
                            FirstName = firstNames[random.Next(0, 12)].ToString(),
                            LastName = lastNames[random.Next(0, 12)].ToString(),
                            IsMember = "true",
                            PhoneNo = "905" + random.Next(1111111, 9999999).ToString(),
                            MemberNo = memberNum
                        };
                        attendees.Add(newAttendee);
                    }
                    //add records to database
                    context.AttendanceTrackings.AddRange(attendees);
                    context.SaveChanges();

                    attendees.Clear();

                    //20 non member records with unique phone numbers
                    for (int i = 0; i < 20; i++)
                    {
                        //create phone number
                        string phone = "905" + random.Next(1111111, 9999999).ToString();
                        
                        //search the Attendance Tracking table by the EventID and find if the phone number created already exists
                        //if it exists, decrease i by 1 and restart process
                        if (context.AttendanceTrackings.Where(e=>e.EventID == eventid).Where(e=>e.PhoneNo == phone).Count() > 0)
                        {
                            i--;
                            return;
                        }

                        //create new Attendance Tracking
                        AttendanceTracking newAttendee = new AttendanceTracking()
                        {
                            MemberAttendanceID = Guid.NewGuid().ToString(),
                            EventID = eventid,
                            FirstName = firstNames[random.Next(0, 12)].ToString(),
                            LastName = lastNames[random.Next(0, 12)].ToString(),
                            IsMember = "false",
                            PhoneNo = phone
                        };
                        attendees.Add(newAttendee);
                    }
                    //add records to database
                    context.AttendanceTrackings.AddRange(attendees);
                    context.SaveChanges();

                    attendees.Clear();

                    //5 duplicate members (duplicate membership number especially)
                    attendees = context.AttendanceTrackings.Where(e => e.EventID == eventid).Where(e => e.IsMember == "true").ToList();

                    for (int i = 0; i < 5; i++)
                    {
                        attendees[i].MemberAttendanceID = Guid.NewGuid().ToString();
                        context.AttendanceTrackings.Add(attendees[i]);
                        context.SaveChanges();
                    }

                    attendees.Clear();

                    //5 duplicate non members (duplicate phone number especially)
                    attendees = context.AttendanceTrackings.Where(e => e.EventID == eventid).Where(e => e.IsMember == "false").ToList();

                    for (int i = 0; i < 5; i++)
                    {
                        attendees[i].MemberAttendanceID = Guid.NewGuid().ToString();
                        context.AttendanceTrackings.Add(attendees[i]);
                        context.SaveChanges();
                    }
                }

                #endregion
                //if (!context.AttendanceTrackings.Any())
                //{
                //    context.AttendanceTrackings.AddRange(
                //     new AttendanceTracking
                //     {
                //         EventID = 1,
                //         FirstName = "Brian",
                //         LastName = "Culp",
                //         IsMember = "true",
                //         ArrivalTime = DateTime.Now,
                //         MemberNo = "1234567898765432",
                //         PhoneNo = "9055554444"

                //     },
                //     new AttendanceTracking
                //     {
                //         EventID = 1,
                //         FirstName = "Jon",
                //         LastName = "Yade",
                //         IsMember = "false",
                //         ArrivalTime = DateTime.Now,
                //         MemberNo = "1222267898765432",
                //         PhoneNo = "9055224444"
                //     },
                //     new AttendanceTracking
                //     {
                //         EventID = 1,
                //         FirstName = "Nate",
                //         LastName = "Smith",
                //         IsMember = "true",
                //         ArrivalTime = DateTime.Now,
                //         MemberNo = "1222267555555432",
                //         PhoneNo = "9055222222"
                //     },
                //     new AttendanceTracking
                //     {
                //         EventID = 2,
                //         FirstName = "Oli",
                //         LastName = "Crroj",
                //         IsMember = "false",
                //         ArrivalTime = DateTime.Now,
                //         MemberNo = "1222267898760032",
                //         PhoneNo = "9055224400"
                //     },
                //     new AttendanceTracking
                //     {
                //         EventID = 3,
                //         FirstName = "Max",
                //         LastName = "Cashmore",
                //         IsMember = "true",
                //         ArrivalTime = DateTime.Now,
                //         MemberNo = "0099567898765432",
                //         PhoneNo = "9999954444"
                //     });
                //    context.SaveChanges();
                //}

                #endregion

                #region Items

                if (!context.Items.Any())
                {
                    context.Items.AddRange(
                     new Item
                     {
                         ItemID = Guid.NewGuid().ToString(),
                         ItemName = "How many people in family?",
                         ValueType = "Numbers"
                     },
                     new Item
                     {
                         ItemID = Guid.NewGuid().ToString(),
                         ItemName = "Are you a member?",
                         ValueType = "Yes-No"
                     },
                     new Item
                     {
                         ItemID = Guid.NewGuid().ToString(),
                         ItemName = "What is your primary car's colour?",
                         ValueType = "Words"
                     }); ;
                    context.SaveChanges();
                }

                #endregion

                #region EventItems - Commented out

                //if (!context.EventItems.Any())
                //{
                //    context.EventItems.AddRange(
                //     new EventItem
                //     {
                //         EventID = 1,
                //         ItemID = 1
                //     },
                //     new EventItem
                //     {
                //         EventID = 3,
                //         ItemID = 2
                //     },
                //     new EventItem
                //     {
                //         EventID = 2,
                //         ItemID = 3
                //     });
                //    context.SaveChanges();
                //}

                #endregion

                #region AttendanceItems - Commented out

                //if (!context.AttendanceItems.Any())
                //{
                //    context.AttendanceItems.AddRange(
                //     new AttendanceItem
                //     {
                //         EventItemID = 1,
                //         MemberAttendanceID = 1,
                //         Answer = "Yes"
                //     },
                //     new AttendanceItem
                //     {
                //         EventItemID = 2,
                //         MemberAttendanceID = 3,
                //         Answer = "no"
                //     },
                //     new AttendanceItem
                //     {
                //         EventItemID = 3,
                //         MemberAttendanceID = 2,
                //         Answer = "15"
                //     });
                //    context.SaveChanges();
                //}

                #endregion

                #endregion
            }
        }
    }
}