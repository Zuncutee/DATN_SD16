using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DATN_SD16.Migrations
{
    /// <inheritdoc />
    public partial class ver2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 21, 21, 32, 1, 890, DateTimeKind.Local).AddTicks(7963), new DateTime(2025, 11, 21, 21, 32, 1, 890, DateTimeKind.Local).AddTicks(7965) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 21, 21, 32, 1, 890, DateTimeKind.Local).AddTicks(7969), new DateTime(2025, 11, 21, 21, 32, 1, 890, DateTimeKind.Local).AddTicks(7970) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 21, 21, 32, 1, 890, DateTimeKind.Local).AddTicks(7975), new DateTime(2025, 11, 21, 21, 32, 1, 890, DateTimeKind.Local).AddTicks(7976) });

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "SettingId",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2025, 11, 21, 21, 32, 1, 890, DateTimeKind.Local).AddTicks(8572));

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "SettingId",
                keyValue: 2,
                column: "UpdatedAt",
                value: new DateTime(2025, 11, 21, 21, 32, 1, 890, DateTimeKind.Local).AddTicks(8576));

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "SettingId",
                keyValue: 3,
                column: "UpdatedAt",
                value: new DateTime(2025, 11, 21, 21, 32, 1, 890, DateTimeKind.Local).AddTicks(8579));

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "SettingId",
                keyValue: 4,
                column: "UpdatedAt",
                value: new DateTime(2025, 11, 21, 21, 32, 1, 890, DateTimeKind.Local).AddTicks(8583));

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "SettingId",
                keyValue: 5,
                column: "UpdatedAt",
                value: new DateTime(2025, 11, 21, 21, 32, 1, 890, DateTimeKind.Local).AddTicks(8595));

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "SettingId",
                keyValue: 6,
                column: "UpdatedAt",
                value: new DateTime(2025, 11, 21, 21, 32, 1, 890, DateTimeKind.Local).AddTicks(8607));

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "SettingId",
                keyValue: 7,
                column: "UpdatedAt",
                value: new DateTime(2025, 11, 21, 21, 32, 1, 890, DateTimeKind.Local).AddTicks(8636));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Address", "CreatedAt", "DateOfBirth", "Email", "FullName", "Gender", "IsActive", "LastLoginAt", "LockedUntil", "PasswordHash", "PhoneNumber", "UpdatedAt", "Username" },
                values: new object[,]
                {
                    { 1, "123 Admin Street", new DateTime(2025, 11, 21, 21, 32, 2, 153, DateTimeKind.Local).AddTicks(7054), null, "admin@example.com", "Admin User", "Other", true, null, null, "$2a$11$lmE8HgitRYpxYn.SGlONsevLOmia6Z37xc1aoUk.9BnCSq1W8H2BC", "0123456789", new DateTime(2025, 11, 21, 21, 32, 2, 153, DateTimeKind.Local).AddTicks(7079), "admin" },
                    { 2, "456 John Street", new DateTime(2025, 11, 21, 21, 32, 2, 416, DateTimeKind.Local).AddTicks(5433), null, "john.doe@example.com", "John Doe", "Male", true, null, null, "$2a$11$HxabxufC9yXNri9QRKAN4.VD9eixZikNkWb0aamR1aKTp3VDfYxBK", "0987654321", new DateTime(2025, 11, 21, 21, 32, 2, 416, DateTimeKind.Local).AddTicks(5451), "john_doe" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 21, 21, 26, 40, 71, DateTimeKind.Local).AddTicks(9244), new DateTime(2025, 11, 21, 21, 26, 40, 71, DateTimeKind.Local).AddTicks(9246) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 21, 21, 26, 40, 71, DateTimeKind.Local).AddTicks(9250), new DateTime(2025, 11, 21, 21, 26, 40, 71, DateTimeKind.Local).AddTicks(9251) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 21, 21, 26, 40, 71, DateTimeKind.Local).AddTicks(9255), new DateTime(2025, 11, 21, 21, 26, 40, 71, DateTimeKind.Local).AddTicks(9256) });

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "SettingId",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2025, 11, 21, 21, 26, 40, 72, DateTimeKind.Local).AddTicks(332));

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "SettingId",
                keyValue: 2,
                column: "UpdatedAt",
                value: new DateTime(2025, 11, 21, 21, 26, 40, 72, DateTimeKind.Local).AddTicks(338));

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "SettingId",
                keyValue: 3,
                column: "UpdatedAt",
                value: new DateTime(2025, 11, 21, 21, 26, 40, 72, DateTimeKind.Local).AddTicks(342));

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "SettingId",
                keyValue: 4,
                column: "UpdatedAt",
                value: new DateTime(2025, 11, 21, 21, 26, 40, 72, DateTimeKind.Local).AddTicks(347));

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "SettingId",
                keyValue: 5,
                column: "UpdatedAt",
                value: new DateTime(2025, 11, 21, 21, 26, 40, 72, DateTimeKind.Local).AddTicks(352));

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "SettingId",
                keyValue: 6,
                column: "UpdatedAt",
                value: new DateTime(2025, 11, 21, 21, 26, 40, 72, DateTimeKind.Local).AddTicks(356));

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "SettingId",
                keyValue: 7,
                column: "UpdatedAt",
                value: new DateTime(2025, 11, 21, 21, 26, 40, 72, DateTimeKind.Local).AddTicks(361));
        }
    }
}
