using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalaryCalculator.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RankCoefficients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Rank = table.Column<int>(type: "INTEGER", nullable: false),
                    Coefficient = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RankCoefficients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalaryDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Performer = table.Column<string>(type: "TEXT", nullable: true),
                    RankCoefficientId = table.Column<int>(type: "INTEGER", nullable: true),
                    MonthlyBaseRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    HourBaseRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    HoursOfWorkPerDay = table.Column<int>(type: "INTEGER", nullable: false),
                    EffectiveWorkingTimeFund = table.Column<decimal>(type: "TEXT", nullable: false),
                    PremiumCoefficient = table.Column<decimal>(type: "TEXT", nullable: false),
                    Salary = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalaryDetails_RankCoefficients_RankCoefficientId",
                        column: x => x.RankCoefficientId,
                        principalTable: "RankCoefficients",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalaryDetails_RankCoefficientId",
                table: "SalaryDetails",
                column: "RankCoefficientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalaryDetails");

            migrationBuilder.DropTable(
                name: "RankCoefficients");
        }
    }
}
