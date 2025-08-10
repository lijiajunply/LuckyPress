using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LuckyPress.NetData.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(128)", nullable: false),
                    Path = table.Column<string>(type: "varchar(128)", nullable: false),
                    Title = table.Column<string>(type: "varchar(32)", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    Intro = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    LastWriteTime = table.Column<DateTime>(type: "DATE", nullable: false),
                    State = table.Column<string>(type: "varchar(20)", nullable: false),
                    Watch = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EMails",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(128)", nullable: false),
                    EMail = table.Column<string>(type: "varchar(128)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Key = table.Column<string>(type: "varchar(32)", nullable: false),
                    Name = table.Column<string>(type: "varchar(32)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(64)", nullable: false),
                    Name = table.Column<string>(type: "varchar(64)", nullable: false),
                    Email = table.Column<string>(type: "varchar(64)", nullable: false),
                    Password = table.Column<string>(type: "varchar(64)", nullable: false),
                    Role = table.Column<string>(type: "varchar(64)", nullable: false),
                    Phone = table.Column<string>(type: "varchar(64)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LoginPos = table.Column<string>(type: "varchar(64)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArticleModelTagModel",
                columns: table => new
                {
                    ArticlesId = table.Column<string>(type: "varchar(128)", nullable: false),
                    TagsKey = table.Column<string>(type: "varchar(32)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleModelTagModel", x => new { x.ArticlesId, x.TagsKey });
                    table.ForeignKey(
                        name: "FK_ArticleModelTagModel_Articles_ArticlesId",
                        column: x => x.ArticlesId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleModelTagModel_Tags_TagsKey",
                        column: x => x.TagsKey,
                        principalTable: "Tags",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleModelTagModel_TagsKey",
                table: "ArticleModelTagModel",
                column: "TagsKey");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleModelTagModel");

            migrationBuilder.DropTable(
                name: "EMails");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Tags");
        }
    }
}
