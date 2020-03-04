using CAA_Event_Management.Data;
using CAA_Event_Management.Models;
using Microsoft.EntityFrameworkCore;
using System;
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
                         UserName = "CAAUser1",
                         Password = "password",
                         isAdmin = false
                     },
                     new UserAccount
                     {
                         FirstName = "CAA",
                         LastName = "Admin",
                         UserName = "admin1",
                         Password = "password",
                         isAdmin = true
                     },
                     new UserAccount
                     {
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

                //if (!context.Questions.Any())
                //{
                //    context.Questions.AddRange(
                //    new Question
                //    {
                //        Phrase = "CAA Members save 10% on their bill at which restaurants?",
                //        GameID = 1,
                //    },
                //    new Question
                //    {
                //        Phrase = "True or False: Members buy one jump pass and get one FREE at Sky Zone Trampoline Park. ",
                //        GameID = 1,
                //    }
                //    );
                //    context.SaveChanges();
                //}
                //if (!context.Answers.Any())
                //{
                //    context.Answers.AddRange(
                //       new Answer
                //       {
                //           Phrase = "Harvey's",
                //           IsCorrect = false,
                //           QuestionID = 1
                //       },
                //       new Answer
                //       {
                //           Phrase = "Kelsey's",
                //           IsCorrect = false,
                //           QuestionID = 1
                //       },
                //       new Answer
                //       {
                //           Phrase = "Montana's",
                //           IsCorrect = false,
                //           QuestionID = 1
                //       },
                //       new Answer
                //       {
                //           Phrase = "All of the Above",
                //           IsCorrect = true,
                //           QuestionID = 1
                //       },
                //       new Answer
                //       {
                //           Phrase = "True",
                //           IsCorrect = true,
                //           QuestionID = 2
                //       },
                //       new Answer
                //       {
                //           Phrase = "False",
                //           IsCorrect = false,
                //           QuestionID = 2
                //       }
                //       );
                //    context.SaveChanges();
                //}

                #endregion

                #region Events

                if (!context.Events.Any())
                {
                    context.Events.AddRange(
                     new Event
                     {
                         EventID = Guid.NewGuid().ToString(),
                         EventName = "TestEvent1",
                         EventStart = DateTime.Today,
                         AbrevEventname = "TE1",
                         MembersOnly = true,
                         DisplayName = "Test Event 1"
                     },
                     new Event
                     {
                         EventID = Guid.NewGuid().ToString(),
                         EventName = "TestEvent2",
                         EventStart = DateTime.Today,
                         AbrevEventname = "TE2",
                         MembersOnly = false,
                         DisplayName = "Test Event 2"
                     },
                     new Event
                     {
                         EventID = Guid.NewGuid().ToString(),
                         EventName = "TestEvent3",
                         EventStart = DateTime.Today,
                         AbrevEventname = "TE3",
                         MembersOnly = true,
                         DisplayName = "Test Event 3"
                     });
                    context.SaveChanges();
                }

                #endregion

                #region AttendanceTracking - Commented out

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