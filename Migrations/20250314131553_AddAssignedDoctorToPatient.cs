using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospitel_Project.Migrations
{
    /// <inheritdoc />
    public partial class AddAssignedDoctorToPatient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignedDoctorId",
                table: "Patients",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_AssignedDoctorId",
                table: "Patients",
                column: "AssignedDoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Doctor_AssignedDoctorId",
                table: "Patients",
                column: "AssignedDoctorId",
                principalTable: "Doctor",
                principalColumn: "DoctorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Doctor_AssignedDoctorId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_AssignedDoctorId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "AssignedDoctorId",
                table: "Patients");
        }
    }
}
