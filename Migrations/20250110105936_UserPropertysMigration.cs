using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Grupp14_CV.Migrations
{
    /// <inheritdoc />
    public partial class UserPropertysMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CVs",
                keyColumn: "CVID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CVs",
                keyColumn: "CVID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CVs",
                keyColumn: "CVID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CVs",
                keyColumn: "CVID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "CVs",
                keyColumn: "CVID",
                keyValue: 5);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ProfileVisitors",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfileVisitors",
                table: "AspNetUsers");

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
        }
    }
}
