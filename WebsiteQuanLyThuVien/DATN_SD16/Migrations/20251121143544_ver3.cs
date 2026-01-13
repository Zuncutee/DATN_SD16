using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DATN_SD16.Migrations
{
    /// <inheritdoc />
    public partial class ver3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 21, 21, 35, 42, 813, DateTimeKind.Local).AddTicks(8642), new DateTime(2025, 11, 21, 21, 35, 42, 813, DateTimeKind.Local).AddTicks(8644) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 21, 21, 35, 42, 813, DateTimeKind.Local).AddTicks(8651), new DateTime(2025, 11, 21, 21, 35, 42, 813, DateTimeKind.Local).AddTicks(8653) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 21, 21, 35, 42, 813, DateTimeKind.Local).AddTicks(8658), new DateTime(2025, 11, 21, 21, 35, 42, 813, DateTimeKind.Local).AddTicks(8660) });

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "SettingId",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2025, 11, 21, 21, 35, 42, 813, DateTimeKind.Local).AddTicks(9594));

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "SettingId",
                keyValue: 2,
                column: "UpdatedAt",
                value: new DateTime(2025, 11, 21, 21, 35, 42, 813, DateTimeKind.Local).AddTicks(9599));

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "SettingId",
                keyValue: 3,
                column: "UpdatedAt",
                value: new DateTime(2025, 11, 21, 21, 35, 42, 813, DateTimeKind.Local).AddTicks(9613));

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "SettingId",
                keyValue: 4,
                column: "UpdatedAt",
                value: new DateTime(2025, 11, 21, 21, 35, 42, 813, DateTimeKind.Local).AddTicks(9630));

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "SettingId",
                keyValue: 5,
                column: "UpdatedAt",
                value: new DateTime(2025, 11, 21, 21, 35, 42, 813, DateTimeKind.Local).AddTicks(9665));

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "SettingId",
                keyValue: 6,
                column: "UpdatedAt",
                value: new DateTime(2025, 11, 21, 21, 35, 42, 813, DateTimeKind.Local).AddTicks(9669));

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "SettingId",
                keyValue: 7,
                column: "UpdatedAt",
                value: new DateTime(2025, 11, 21, 21, 35, 42, 813, DateTimeKind.Local).AddTicks(9673));

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "UserRoleId", "AssignedAt", "AssignedBy", "RoleId", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 21, 21, 35, 43, 389, DateTimeKind.Local).AddTicks(6016), null, 1, 1 },
                    { 2, new DateTime(2025, 11, 21, 21, 35, 43, 389, DateTimeKind.Local).AddTicks(6026), 1, 2, 2 }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 21, 21, 35, 43, 96, DateTimeKind.Local).AddTicks(9083), "$2a$11$pL3wCzJE5.P2oYHDTAkd8u.7l5BkQ45YLWiX4YH16W/GkHKDH8r0G", new DateTime(2025, 11, 21, 21, 35, 43, 96, DateTimeKind.Local).AddTicks(9107) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 21, 21, 35, 43, 389, DateTimeKind.Local).AddTicks(4074), "$2a$11$mMc8oqWJF1ENbc1K4esZwed3cuLg7HftrXIYdeQI/3lvt1KCMILAS", new DateTime(2025, 11, 21, 21, 35, 43, 389, DateTimeKind.Local).AddTicks(4104) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "UserRoleId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "UserRoleId",
                keyValue: 2);

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

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 21, 21, 32, 2, 153, DateTimeKind.Local).AddTicks(7054), "$2a$11$lmE8HgitRYpxYn.SGlONsevLOmia6Z37xc1aoUk.9BnCSq1W8H2BC", new DateTime(2025, 11, 21, 21, 32, 2, 153, DateTimeKind.Local).AddTicks(7079) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 21, 21, 32, 2, 416, DateTimeKind.Local).AddTicks(5433), "$2a$11$HxabxufC9yXNri9QRKAN4.VD9eixZikNkWb0aamR1aKTp3VDfYxBK", new DateTime(2025, 11, 21, 21, 32, 2, 416, DateTimeKind.Local).AddTicks(5451) });
        }
    }
}
