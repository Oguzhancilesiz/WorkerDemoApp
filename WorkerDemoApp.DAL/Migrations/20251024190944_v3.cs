using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkerDemoApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FirstReminderUtc",
                table: "VerificationCodes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReminderCount",
                table: "VerificationCodes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstReminderUtc",
                table: "VerificationCodes");

            migrationBuilder.DropColumn(
                name: "ReminderCount",
                table: "VerificationCodes");
        }
    }
}
