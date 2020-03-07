using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CAA_Event_Management.Migrations
{
    public partial class CAAMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedBy = table.Column<string>(maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    Keywords = table.Column<string>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 75, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ItemID = table.Column<string>(maxLength: 36, nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ItemCount = table.Column<int>(nullable: false),
                    ItemName = table.Column<string>(maxLength: 75, nullable: false),
                    ValueType = table.Column<string>(maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItemID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(maxLength: 25, nullable: true),
                    LastName = table.Column<string>(maxLength: 25, nullable: true),
                    Password = table.Column<string>(maxLength: 50, nullable: false),
                    UserName = table.Column<string>(nullable: false),
                    isAdmin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventID = table.Column<string>(maxLength: 36, nullable: false),
                    AbrevEventname = table.Column<string>(maxLength: 20, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DisplayName = table.Column<string>(maxLength: 100, nullable: true),
                    EventEnd = table.Column<DateTime>(nullable: true),
                    EventName = table.Column<string>(maxLength: 100, nullable: false),
                    EventStart = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Keywords = table.Column<string>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 75, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    MembersOnly = table.Column<bool>(nullable: false),
                    QuizID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventID);
                    table.ForeignKey(
                        name: "FK_Events_Games_QuizID",
                        column: x => x.QuizID,
                        principalTable: "Games",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CorrectFeedback = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    GameID = table.Column<int>(nullable: false),
                    Keywords = table.Column<string>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 75, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    Phrase = table.Column<string>(nullable: true),
                    TotalFeedback = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Questions_Games_GameID",
                        column: x => x.GameID,
                        principalTable: "Games",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttendanceTrackings",
                columns: table => new
                {
                    MemberAttendanceID = table.Column<string>(maxLength: 36, nullable: false),
                    ArrivalTime = table.Column<DateTime>(nullable: false),
                    EventID = table.Column<string>(nullable: false),
                    ExternalData = table.Column<bool>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 25, nullable: true),
                    IsMember = table.Column<string>(maxLength: 20, nullable: true),
                    LastName = table.Column<string>(maxLength: 25, nullable: true),
                    MemberNo = table.Column<string>(maxLength: 16, nullable: true),
                    PhoneNo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceTrackings", x => x.MemberAttendanceID);
                    table.ForeignKey(
                        name: "FK_AttendanceTrackings_Events_EventID",
                        column: x => x.EventID,
                        principalTable: "Events",
                        principalColumn: "EventID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventItems",
                columns: table => new
                {
                    EventItemID = table.Column<string>(maxLength: 36, nullable: false),
                    EventID = table.Column<string>(nullable: false),
                    ItemID = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventItems", x => x.EventItemID);
                    table.ForeignKey(
                        name: "FK_EventItems_Events_EventID",
                        column: x => x.EventID,
                        principalTable: "Events",
                        principalColumn: "EventID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventItems_Items_ItemID",
                        column: x => x.ItemID,
                        principalTable: "Items",
                        principalColumn: "ItemID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedBy = table.Column<string>(maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    IsCorrect = table.Column<bool>(nullable: true),
                    Keywords = table.Column<string>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 75, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    Phrase = table.Column<string>(maxLength: 50, nullable: true),
                    QuestionID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Answers_Questions_QuestionID",
                        column: x => x.QuestionID,
                        principalTable: "Questions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventGameUserAnswers",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 36, nullable: false),
                    EventID = table.Column<string>(nullable: false),
                    QuestionID = table.Column<int>(nullable: false),
                    answerWasCorrect = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventGameUserAnswers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_EventGameUserAnswers_Events_EventID",
                        column: x => x.EventID,
                        principalTable: "Events",
                        principalColumn: "EventID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventGameUserAnswers_Questions_QuestionID",
                        column: x => x.QuestionID,
                        principalTable: "Questions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttendanceItems",
                columns: table => new
                {
                    MemberAttendanceID = table.Column<string>(nullable: false),
                    EventItemID = table.Column<string>(nullable: false),
                    Answer = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceItems", x => new { x.MemberAttendanceID, x.EventItemID });
                    table.ForeignKey(
                        name: "FK_AttendanceItems_EventItems_EventItemID",
                        column: x => x.EventItemID,
                        principalTable: "EventItems",
                        principalColumn: "EventItemID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttendanceItems_AttendanceTrackings_MemberAttendanceID",
                        column: x => x.MemberAttendanceID,
                        principalTable: "AttendanceTrackings",
                        principalColumn: "MemberAttendanceID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionID",
                table: "Answers",
                column: "QuestionID");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceItems_EventItemID",
                table: "AttendanceItems",
                column: "EventItemID");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceTrackings_EventID",
                table: "AttendanceTrackings",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_Events_QuizID",
                table: "Events",
                column: "QuizID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventGameUserAnswers_EventID",
                table: "EventGameUserAnswers",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_EventGameUserAnswers_QuestionID",
                table: "EventGameUserAnswers",
                column: "QuestionID");

            migrationBuilder.CreateIndex(
                name: "IX_EventItems_EventID",
                table: "EventItems",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_EventItems_ItemID",
                table: "EventItems",
                column: "ItemID");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_GameID",
                table: "Questions",
                column: "GameID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "AttendanceItems");

            migrationBuilder.DropTable(
                name: "EventGameUserAnswers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "EventItems");

            migrationBuilder.DropTable(
                name: "AttendanceTrackings");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
