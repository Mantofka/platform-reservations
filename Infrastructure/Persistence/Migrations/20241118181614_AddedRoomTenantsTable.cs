using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedRoomTenantsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomTenant_Rooms_RoomId",
                table: "RoomTenant");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomTenant_Tenants_TenantId",
                table: "RoomTenant");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "RoomTenant",
                newName: "TenantsId");

            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "RoomTenant",
                newName: "RoomsId");

            migrationBuilder.RenameIndex(
                name: "IX_RoomTenant_TenantId",
                table: "RoomTenant",
                newName: "IX_RoomTenant_TenantsId");

            migrationBuilder.CreateTable(
                name: "RoomTenants",
                columns: table => new
                {
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomTenants", x => new { x.RoomId, x.TenantId });
                    table.ForeignKey(
                        name: "FK_RoomTenants_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomTenants_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomTenants_TenantId",
                table: "RoomTenants",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomTenant_Rooms_RoomsId",
                table: "RoomTenant",
                column: "RoomsId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomTenant_Tenants_TenantsId",
                table: "RoomTenant",
                column: "TenantsId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomTenant_Rooms_RoomsId",
                table: "RoomTenant");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomTenant_Tenants_TenantsId",
                table: "RoomTenant");

            migrationBuilder.DropTable(
                name: "RoomTenants");

            migrationBuilder.RenameColumn(
                name: "TenantsId",
                table: "RoomTenant",
                newName: "TenantId");

            migrationBuilder.RenameColumn(
                name: "RoomsId",
                table: "RoomTenant",
                newName: "RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_RoomTenant_TenantsId",
                table: "RoomTenant",
                newName: "IX_RoomTenant_TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomTenant_Rooms_RoomId",
                table: "RoomTenant",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomTenant_Tenants_TenantId",
                table: "RoomTenant",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
