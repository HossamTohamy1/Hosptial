using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospitel_Project.Migrations
{
    /// <inheritdoc />
    public partial class AddTimeOfDosageAndPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TimeOfdosage",
                table: "Medicaments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "price",
                table: "Medicaments",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeOfdosage",
                table: "Medicaments");

            migrationBuilder.DropColumn(
                name: "price",
                table: "Medicaments");
        }
    }
}
