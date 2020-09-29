using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BobReactRemaster.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiscordCredentials",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClientID = table.Column<string>(nullable: true),
                    Token = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordCredentials", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Guild",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guild", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    UserName = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: true),
                    UserRole = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.UserName);
                });

            migrationBuilder.CreateTable(
                name: "TwitchCredentials",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClientID = table.Column<string>(nullable: true),
                    Token = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Secret = table.Column<string>(nullable: true),
                    RefreshToken = table.Column<string>(nullable: true),
                    isTwitchCheckerClient = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitchCredentials", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DiscordTextChannels",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    IsPermanentRelayChannel = table.Column<bool>(nullable: false),
                    IsRelayChannel = table.Column<bool>(nullable: false),
                    GuildName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordTextChannels", x => x.id);
                    table.ForeignKey(
                        name: "FK_DiscordTextChannels_Guild_GuildName",
                        column: x => x.GuildName,
                        principalTable: "Guild",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TwitchStreams",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    URL = table.Column<string>(nullable: true),
                    Started = table.Column<DateTime>(nullable: false),
                    Stopped = table.Column<DateTime>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    RelayChannelid = table.Column<int>(nullable: true),
                    StreamName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitchStreams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TwitchStreams_DiscordTextChannels_RelayChannelid",
                        column: x => x.RelayChannelid,
                        principalTable: "DiscordTextChannels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StreamSubscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LiveStreamId = table.Column<int>(nullable: true),
                    isSubscribed = table.Column<bool>(nullable: false),
                    MemberUserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StreamSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StreamSubscriptions_TwitchStreams_LiveStreamId",
                        column: x => x.LiveStreamId,
                        principalTable: "TwitchStreams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StreamSubscriptions_Members_MemberUserName",
                        column: x => x.MemberUserName,
                        principalTable: "Members",
                        principalColumn: "UserName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscordTextChannels_GuildName",
                table: "DiscordTextChannels",
                column: "GuildName");

            migrationBuilder.CreateIndex(
                name: "IX_StreamSubscriptions_LiveStreamId",
                table: "StreamSubscriptions",
                column: "LiveStreamId");

            migrationBuilder.CreateIndex(
                name: "IX_StreamSubscriptions_MemberUserName",
                table: "StreamSubscriptions",
                column: "MemberUserName");

            migrationBuilder.CreateIndex(
                name: "IX_TwitchStreams_RelayChannelid",
                table: "TwitchStreams",
                column: "RelayChannelid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscordCredentials");

            migrationBuilder.DropTable(
                name: "StreamSubscriptions");

            migrationBuilder.DropTable(
                name: "TwitchCredentials");

            migrationBuilder.DropTable(
                name: "TwitchStreams");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "DiscordTextChannels");

            migrationBuilder.DropTable(
                name: "Guild");
        }
    }
}
