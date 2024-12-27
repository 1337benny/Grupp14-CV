using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Grupp14_CV.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CVs",
                columns: table => new
                {
                    CVID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Header = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(800)", maxLength: 800, nullable: false),
                    Competence = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Education = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    PreviousExperience = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CVs", x => x.CVID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Firstname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Lastname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PublicSetting = table.Column<bool>(type: "bit", nullable: false),
                    BirthDay = table.Column<DateOnly>(type: "date", nullable: false),
                    CVID = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_CVs_CVID",
                        column: x => x.CVID,
                        principalTable: "CVs",
                        principalColumn: "CVID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    SenderID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReceiverID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MessageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => new { x.SenderID, x.ReceiverID });
                    table.ForeignKey(
                        name: "FK_Messages_Users_ReceiverID",
                        column: x => x.ReceiverID,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Messages_Users_SenderID",
                        column: x => x.SenderID,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ProjectID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ProjectID);
                    table.ForeignKey(
                        name: "FK_Projects_Users_CreatorID",
                        column: x => x.CreatorID,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users_In_Projects",
                columns: table => new
                {
                    ProjectID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users_In_Projects", x => new { x.UserID, x.ProjectID });
                    table.ForeignKey(
                        name: "FK_Users_In_Projects_Projects_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Projects",
                        principalColumn: "ProjectID");
                    table.ForeignKey(
                        name: "FK_Users_In_Projects_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "CVs",
                columns: new[] { "CVID", "Competence", "Content", "Education", "Header", "PreviousExperience" },
                values: new object[,]
                {
                    { 1, "Lösningsorienterad, tålmodig, klok.", "Hej! Jag heter Johan och är född 2001. Jag gillar mysiga hemmakvällar och att programmera.", "Europaskolan Gymnasie & Örebro Universitet Systemvetenskap", "Johans CV", "Ica, Willys, Netbeans, Rewatch, SQL-Server, HTML, Alla språk." },
                    { 2, "Noggrann, initiativrik, samarbetsvillig.", "Hej! Jag heter Anton och är född 1998. Jag gillar sport, teknik och att laga mat.", "Stockholms Universitet Datavetenskap", "Antons CV", "Coop, Elgiganten, C#, React, Node.js, MongoDB." },
                    { 3, "Ledarskapsförmåga, kreativ, kommunikativ.", "Hej! Jag heter William och är född 1995. Jag älskar att leda team, skapa lösningar och resa.", "Chalmers Tekniska Högskola Industriell Ekonomi", "Williams CV", "Rewatch, Microsoft, Azure, Python, SQL, Vue.js." },
                    { 4, "Analytisk, strukturerad, lösningsorienterad.", "Hej! Jag heter Oskar och är född 1997. Jag trivs med att arbeta med detaljer och analysera problem.", "Lunds Universitet Civilingenjör Informationsteknik", "Oskars CV", "Hemköp, Rewatch, Java, Docker, Kubernetes, MySQL, PHP." },
                    { 5, "Innovativ, användarcentrerad, flexibel.", "Hej! Jag heter Oliver och är född 2000. Jag brinner för ny teknik och att bygga intuitiva användarupplevelser.", "KTH Kungliga Tekniska Högskolan Datateknik", "Olivers CV", "NetOnNet, MediaMarkt, Kotlin, Swift, Figma, UX/UI Design." }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ReceiverID",
                table: "Messages",
                column: "ReceiverID");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CreatorID",
                table: "Projects",
                column: "CreatorID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CVID",
                table: "Users",
                column: "CVID",
                unique: true,
                filter: "[CVID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_In_Projects_ProjectID",
                table: "Users_In_Projects",
                column: "ProjectID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Users_In_Projects");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "CVs");
        }
    }
}
