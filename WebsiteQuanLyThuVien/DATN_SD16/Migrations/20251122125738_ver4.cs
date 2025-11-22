using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN_SD16.Migrations
{
    /// <inheritdoc />
    public partial class ver4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 22, 19, 57, 36, 912, DateTimeKind.Local).AddTicks(6958), new DateTime(2025, 11, 22, 19, 57, 36, 912, DateTimeKind.Local).AddTicks(6960) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 22, 19, 57, 36, 912, DateTimeKind.Local).AddTicks(6967), new DateTime(2025, 11, 22, 19, 57, 36, 912, DateTimeKind.Local).AddTicks(6968) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 22, 19, 57, 36, 912, DateTimeKind.Local).AddTicks(6974), new DateTime(2025, 11, 22, 19, 57, 36, 912, DateTimeKind.Local).AddTicks(6976) });

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "SettingId",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2025, 11, 22, 19, 57, 36, 912, DateTimeKind.Local).AddTicks(7730));

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "SettingId",
                keyValue: 2,
                column: "UpdatedAt",
                value: new DateTime(2025, 11, 22, 19, 57, 36, 912, DateTimeKind.Local).AddTicks(7735));

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "SettingId",
                keyValue: 3,
                column: "UpdatedAt",
                value: new DateTime(2025, 11, 22, 19, 57, 36, 912, DateTimeKind.Local).AddTicks(7751));

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "SettingId",
                keyValue: 4,
                column: "UpdatedAt",
                value: new DateTime(2025, 11, 22, 19, 57, 36, 912, DateTimeKind.Local).AddTicks(7772));

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "SettingId",
                keyValue: 5,
                column: "UpdatedAt",
                value: new DateTime(2025, 11, 22, 19, 57, 36, 912, DateTimeKind.Local).AddTicks(7803));

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "SettingId",
                keyValue: 6,
                column: "UpdatedAt",
                value: new DateTime(2025, 11, 22, 19, 57, 36, 912, DateTimeKind.Local).AddTicks(7808));

            migrationBuilder.UpdateData(
                table: "SystemSettings",
                keyColumn: "SettingId",
                keyValue: 7,
                column: "UpdatedAt",
                value: new DateTime(2025, 11, 22, 19, 57, 36, 912, DateTimeKind.Local).AddTicks(7812));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "UserRoleId",
                keyValue: 1,
                column: "AssignedAt",
                value: new DateTime(2025, 11, 22, 19, 57, 37, 456, DateTimeKind.Local).AddTicks(5000));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "UserRoleId",
                keyValue: 2,
                columns: new[] { "AssignedAt", "RoleId" },
                values: new object[] { new DateTime(2025, 11, 22, 19, 57, 37, 456, DateTimeKind.Local).AddTicks(5004), 3 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 22, 19, 57, 37, 180, DateTimeKind.Local).AddTicks(5702), "$2a$11$OsNBdqNn4zlxnSW99z5W.OYBUZkCOeoxBidJudIfbyPCWgXrk.Dke", new DateTime(2025, 11, 22, 19, 57, 37, 180, DateTimeKind.Local).AddTicks(5724) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 22, 19, 57, 37, 456, DateTimeKind.Local).AddTicks(4244), "$2a$11$4N4Cx9DT8uakjTiqWiUYiexfO.J2NsrZGebwk2eovpMCdFwfYz1Mq", new DateTime(2025, 11, 22, 19, 57, 37, 456, DateTimeKind.Local).AddTicks(4263) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "UserRoleId",
                keyValue: 1,
                column: "AssignedAt",
                value: new DateTime(2025, 11, 21, 21, 35, 43, 389, DateTimeKind.Local).AddTicks(6016));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "UserRoleId",
                keyValue: 2,
                columns: new[] { "AssignedAt", "RoleId" },
                values: new object[] { new DateTime(2025, 11, 21, 21, 35, 43, 389, DateTimeKind.Local).AddTicks(6026), 2 });

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
    }
}
