using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CAA_Event_Management.Migrations
{
    public partial class CAAMigrationMar04 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    Text = table.Column<string>(maxLength: 50, nullable: true),
                    TimesUsed = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(nullable: true),
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
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    ItemCount = table.Column<int>(nullable: true),
                    ItemName = table.Column<string>(maxLength: 75, nullable: false),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    ValueType = table.Column<string>(maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItemID);
                });

            migrationBuilder.CreateTable(
                name: "Pictures",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    Image = table.Column<byte[]>(nullable: true),
                    ImageFileName = table.Column<string>(nullable: true),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pictures", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    TimesUsed = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 36, nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    FirstName = table.Column<string>(maxLength: 25, nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    LastName = table.Column<string>(maxLength: 25, nullable: true),
                    Password = table.Column<string>(maxLength: 50, nullable: false),
                    UserName = table.Column<string>(nullable: false),
                    isAdmin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventID = table.Column<string>(maxLength: 36, nullable: false),
                    AbrevEventname = table.Column<string>(maxLength: 20, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DisplayName = table.Column<string>(maxLength: 100, nullable: true),
                    EventEnd = table.Column<DateTime>(nullable: true),
                    EventName = table.Column<string>(maxLength: 100, nullable: false),
                    EventStart = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Keywords = table.Column<string>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
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
                name: "GameModels",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AnswerText = table.Column<string>(nullable: true),
                    GameID = table.Column<int>(nullable: false),
                    OptionsText = table.Column<string>(nullable: true),
                    QuestionText = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameModels", x => x.ID);
                    table.ForeignKey(
                        name: "FK_GameModels_Games_GameID",
                        column: x => x.GameID,
                        principalTable: "Games",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnswerPictures",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AnswerID = table.Column<int>(nullable: false),
                    PictureID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerPictures", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AnswerPictures_Answers_AnswerID",
                        column: x => x.AnswerID,
                        principalTable: "Answers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnswerPictures_Pictures_PictureID",
                        column: x => x.PictureID,
                        principalTable: "Pictures",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnswerTags",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AnswerID = table.Column<int>(nullable: false),
                    TagID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerTags", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AnswerTags_Answers_AnswerID",
                        column: x => x.AnswerID,
                        principalTable: "Answers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnswerTags_Tags_TagID",
                        column: x => x.TagID,
                        principalTable: "Tags",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameTags",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GameID = table.Column<int>(nullable: false),
                    TagID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameTags", x => x.ID);
                    table.ForeignKey(
                        name: "FK_GameTags_Games_GameID",
                        column: x => x.GameID,
                        principalTable: "Games",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameTags_Tags_TagID",
                        column: x => x.TagID,
                        principalTable: "Tags",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionTags",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QuestionID = table.Column<int>(nullable: false),
                    TagID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionTags", x => x.ID);
                    table.ForeignKey(
                        name: "FK_QuestionTags_Questions_QuestionID",
                        column: x => x.QuestionID,
                        principalTable: "Questions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionTags_Tags_TagID",
                        column: x => x.TagID,
                        principalTable: "Tags",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttendanceTrackings",
                columns: table => new
                {
                    MemberAttendanceID = table.Column<string>(maxLength: 36, nullable: false),
                    ArrivalTime = table.Column<DateTime>(nullable: false),
                    EventID = table.Column<string>(maxLength: 36, nullable: false),
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
                name: "EventGameUserAnswers",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 36, nullable: false),
                    AttendantID = table.Column<string>(maxLength: 36, nullable: true),
                    EventID = table.Column<string>(maxLength: 36, nullable: false),
                    QuestionID = table.Column<int>(nullable: false),
                    answerID = table.Column<int>(nullable: true),
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
                name: "EventItems",
                columns: table => new
                {
                    EventItemID = table.Column<string>(maxLength: 36, nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    EventID = table.Column<string>(maxLength: 36, nullable: false),
                    ItemID = table.Column<string>(maxLength: 36, nullable: false),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true)
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
                name: "AttendanceItems",
                columns: table => new
                {
                    MemberAttendanceID = table.Column<string>(maxLength: 36, nullable: false),
                    EventItemID = table.Column<string>(maxLength: 36, nullable: false),
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
                name: "IX_AnswerPictures_AnswerID",
                table: "AnswerPictures",
                column: "AnswerID");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerPictures_PictureID",
                table: "AnswerPictures",
                column: "PictureID");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerTags_AnswerID",
                table: "AnswerTags",
                column: "AnswerID");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerTags_TagID",
                table: "AnswerTags",
                column: "TagID");

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
                column: "QuizID");

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
                name: "IX_GameModels_GameID",
                table: "GameModels",
                column: "GameID");

            migrationBuilder.CreateIndex(
                name: "IX_GameTags_GameID",
                table: "GameTags",
                column: "GameID");

            migrationBuilder.CreateIndex(
                name: "IX_GameTags_TagID",
                table: "GameTags",
                column: "TagID");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionTags_QuestionID",
                table: "QuestionTags",
                column: "QuestionID");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionTags_TagID",
                table: "QuestionTags",
                column: "TagID");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_UserName",
                table: "UserAccounts",
                column: "UserName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerPictures");

            migrationBuilder.DropTable(
                name: "AnswerTags");

            migrationBuilder.DropTable(
                name: "AttendanceItems");

            migrationBuilder.DropTable(
                name: "EventGameUserAnswers");

            migrationBuilder.DropTable(
                name: "GameModels");

            migrationBuilder.DropTable(
                name: "GameTags");

            migrationBuilder.DropTable(
                name: "QuestionTags");

            migrationBuilder.DropTable(
                name: "UserAccounts");

            migrationBuilder.DropTable(
                name: "Pictures");

            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "EventItems");

            migrationBuilder.DropTable(
                name: "AttendanceTrackings");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
