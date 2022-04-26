using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BobReactRemaster.Migrations
{
    public partial class newinit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DiscordCredentials",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClientID = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Token = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordCredentials", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MeetingTemplates",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingTemplates", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    UserName = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DiscordUserName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DiscordDiscriminator = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserRole = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TestWonGiftsCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.UserName);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TwitchCredentials",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClientID = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ChatUserName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Token = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Secret = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    validationKey = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RefreshToken = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpireDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    isMainAccount = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitchCredentials", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MeetingDateTemplates",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TemplateID = table.Column<int>(type: "int", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    Start = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    End = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingDateTemplates", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MeetingDateTemplates_MeetingTemplates_TemplateID",
                        column: x => x.TemplateID,
                        principalTable: "MeetingTemplates",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Meetings",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MeetingDateStart = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    MeetingDateEnd = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ReminderDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    MeetingTemplateID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meetings", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Meetings_MeetingTemplates_MeetingTemplateID",
                        column: x => x.MeetingTemplateID,
                        principalTable: "MeetingTemplates",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ReminderTemplates",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ReminderDay = table.Column<int>(type: "int", nullable: false),
                    ReminderTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    MeetingTemplateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReminderTemplates", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ReminderTemplates_MeetingTemplates_MeetingTemplateId",
                        column: x => x.MeetingTemplateId,
                        principalTable: "MeetingTemplates",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MeetingTemplates_Members",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RegisteredMemberUserName = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MeetingTemplateID = table.Column<int>(type: "int", nullable: false),
                    IsAuthor = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingTemplates_Members", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MeetingTemplates_Members_MeetingTemplates_MeetingTemplateID",
                        column: x => x.MeetingTemplateID,
                        principalTable: "MeetingTemplates",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MeetingTemplates_Members_Members_RegisteredMemberUserName",
                        column: x => x.RegisteredMemberUserName,
                        principalTable: "Members",
                        principalColumn: "UserName",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LiveStream",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    URL = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Started = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Stopped = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    RelayEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UpTimeInterval = table.Column<int>(type: "int", nullable: false),
                    StreamName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VariableRelayChannel = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Discriminator = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StreamID = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    APICredentialId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiveStream", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LiveStream_TwitchCredentials_APICredentialId",
                        column: x => x.APICredentialId,
                        principalTable: "TwitchCredentials",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MeetingSubscriptions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SubscriberUserName = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MeetingID = table.Column<int>(type: "int", nullable: false),
                    IsAuthor = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Message = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingSubscriptions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MeetingSubscriptions_Meetings_MeetingID",
                        column: x => x.MeetingID,
                        principalTable: "Meetings",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MeetingSubscriptions_Members_SubscriberUserName",
                        column: x => x.SubscriberUserName,
                        principalTable: "Members",
                        principalColumn: "UserName",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DiscordTextChannels",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ChannelID = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsPermanentRelayChannel = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsRelayChannel = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Guild = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LiveStreamID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordTextChannels", x => x.id);
                    table.ForeignKey(
                        name: "FK_DiscordTextChannels_LiveStream_LiveStreamID",
                        column: x => x.LiveStreamID,
                        principalTable: "LiveStream",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "IntervalCommands",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AutoInverval = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Response = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LiveStreamId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntervalCommands", x => x.ID);
                    table.ForeignKey(
                        name: "FK_IntervalCommands_LiveStream_LiveStreamId",
                        column: x => x.LiveStreamId,
                        principalTable: "LiveStream",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ManualCommands",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Trigger = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Response = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LiveStreamId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManualCommands", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ManualCommands_LiveStream_LiveStreamId",
                        column: x => x.LiveStreamId,
                        principalTable: "LiveStream",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Quotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LiveStreamID = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Text = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quotes_LiveStream_LiveStreamID",
                        column: x => x.LiveStreamID,
                        principalTable: "LiveStream",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StreamSubscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LiveStreamId = table.Column<int>(type: "int", nullable: false),
                    isSubscribed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    MemberUserName = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StreamSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StreamSubscriptions_LiveStream_LiveStreamId",
                        column: x => x.LiveStreamId,
                        principalTable: "LiveStream",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StreamSubscriptions_Members_MemberUserName",
                        column: x => x.MemberUserName,
                        principalTable: "Members",
                        principalColumn: "UserName",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GiveAways",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TextChannelid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiveAways", x => x.ID);
                    table.ForeignKey(
                        name: "FK_GiveAways_DiscordTextChannels_TextChannelid",
                        column: x => x.TextChannelid,
                        principalTable: "DiscordTextChannels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Gifts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GiveAwayID = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InternalName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Link = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Key = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OwnerUserName = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WinnerUserName = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Turn = table.Column<int>(type: "int", nullable: false),
                    IsCurrent = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gifts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Gifts_GiveAways_GiveAwayID",
                        column: x => x.GiveAwayID,
                        principalTable: "GiveAways",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Gifts_Members_OwnerUserName",
                        column: x => x.OwnerUserName,
                        principalTable: "Members",
                        principalColumn: "UserName",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Gifts_Members_WinnerUserName",
                        column: x => x.WinnerUserName,
                        principalTable: "Members",
                        principalColumn: "UserName",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GiveAway_Member",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MemberUserName = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GiveAwayID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiveAway_Member", x => x.ID);
                    table.ForeignKey(
                        name: "FK_GiveAway_Member_GiveAways_GiveAwayID",
                        column: x => x.GiveAwayID,
                        principalTable: "GiveAways",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GiveAway_Member_Members_MemberUserName",
                        column: x => x.MemberUserName,
                        principalTable: "Members",
                        principalColumn: "UserName",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordTextChannels_LiveStreamID",
                table: "DiscordTextChannels",
                column: "LiveStreamID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gifts_GiveAwayID",
                table: "Gifts",
                column: "GiveAwayID");

            migrationBuilder.CreateIndex(
                name: "IX_Gifts_OwnerUserName",
                table: "Gifts",
                column: "OwnerUserName");

            migrationBuilder.CreateIndex(
                name: "IX_Gifts_WinnerUserName",
                table: "Gifts",
                column: "WinnerUserName");

            migrationBuilder.CreateIndex(
                name: "IX_GiveAway_Member_GiveAwayID",
                table: "GiveAway_Member",
                column: "GiveAwayID");

            migrationBuilder.CreateIndex(
                name: "IX_GiveAway_Member_MemberUserName",
                table: "GiveAway_Member",
                column: "MemberUserName");

            migrationBuilder.CreateIndex(
                name: "IX_GiveAways_TextChannelid",
                table: "GiveAways",
                column: "TextChannelid");

            migrationBuilder.CreateIndex(
                name: "IX_IntervalCommands_LiveStreamId",
                table: "IntervalCommands",
                column: "LiveStreamId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveStream_APICredentialId",
                table: "LiveStream",
                column: "APICredentialId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ManualCommands_LiveStreamId",
                table: "ManualCommands",
                column: "LiveStreamId");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingDateTemplates_TemplateID",
                table: "MeetingDateTemplates",
                column: "TemplateID");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_MeetingTemplateID",
                table: "Meetings",
                column: "MeetingTemplateID");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingSubscriptions_MeetingID",
                table: "MeetingSubscriptions",
                column: "MeetingID");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingSubscriptions_SubscriberUserName",
                table: "MeetingSubscriptions",
                column: "SubscriberUserName");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingTemplates_Members_MeetingTemplateID",
                table: "MeetingTemplates_Members",
                column: "MeetingTemplateID");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingTemplates_Members_RegisteredMemberUserName",
                table: "MeetingTemplates_Members",
                column: "RegisteredMemberUserName");

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_LiveStreamID",
                table: "Quotes",
                column: "LiveStreamID");

            migrationBuilder.CreateIndex(
                name: "IX_ReminderTemplates_MeetingTemplateId",
                table: "ReminderTemplates",
                column: "MeetingTemplateId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StreamSubscriptions_LiveStreamId",
                table: "StreamSubscriptions",
                column: "LiveStreamId");

            migrationBuilder.CreateIndex(
                name: "IX_StreamSubscriptions_MemberUserName",
                table: "StreamSubscriptions",
                column: "MemberUserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscordCredentials");

            migrationBuilder.DropTable(
                name: "Gifts");

            migrationBuilder.DropTable(
                name: "GiveAway_Member");

            migrationBuilder.DropTable(
                name: "IntervalCommands");

            migrationBuilder.DropTable(
                name: "ManualCommands");

            migrationBuilder.DropTable(
                name: "MeetingDateTemplates");

            migrationBuilder.DropTable(
                name: "MeetingSubscriptions");

            migrationBuilder.DropTable(
                name: "MeetingTemplates_Members");

            migrationBuilder.DropTable(
                name: "Quotes");

            migrationBuilder.DropTable(
                name: "ReminderTemplates");

            migrationBuilder.DropTable(
                name: "StreamSubscriptions");

            migrationBuilder.DropTable(
                name: "GiveAways");

            migrationBuilder.DropTable(
                name: "Meetings");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "DiscordTextChannels");

            migrationBuilder.DropTable(
                name: "MeetingTemplates");

            migrationBuilder.DropTable(
                name: "LiveStream");

            migrationBuilder.DropTable(
                name: "TwitchCredentials");
        }
    }
}
