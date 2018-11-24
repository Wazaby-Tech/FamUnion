using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace FamUnion.Data.Migrations
{
    public partial class AddReunions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ReunionId",
                table: "Families",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ReunionId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Reunions",
                columns: table => new
                {
                    ReunionId = table.Column<Guid>(nullable: false),
                    CityLocationAddressId = table.Column<Guid>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 4000, nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    StartDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reunions", x => x.ReunionId);
                    table.ForeignKey(
                        name: "FK_Reunions_Address_CityLocationAddressId",
                        column: x => x.CityLocationAddressId,
                        principalTable: "Address",
                        principalColumn: "AddressId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    EventId = table.Column<Guid>(nullable: false),
                    AddressId = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Details = table.Column<string>(maxLength: 2000, nullable: true),
                    Duration = table.Column<double>(nullable: true),
                    EndTime = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    ReunionId = table.Column<Guid>(nullable: true),
                    StartTime = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_Event_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "AddressId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Event_Reunions_ReunionId",
                        column: x => x.ReunionId,
                        principalTable: "Reunions",
                        principalColumn: "ReunionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Families_ReunionId",
                table: "Families",
                column: "ReunionId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ReunionId",
                table: "AspNetUsers",
                column: "ReunionId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_AddressId",
                table: "Event",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_ReunionId",
                table: "Event",
                column: "ReunionId");

            migrationBuilder.CreateIndex(
                name: "IX_Reunions_CityLocationAddressId",
                table: "Reunions",
                column: "CityLocationAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Reunions_ReunionId",
                table: "AspNetUsers",
                column: "ReunionId",
                principalTable: "Reunions",
                principalColumn: "ReunionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Families_Reunions_ReunionId",
                table: "Families",
                column: "ReunionId",
                principalTable: "Reunions",
                principalColumn: "ReunionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Reunions_ReunionId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Families_Reunions_ReunionId",
                table: "Families");

            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "Reunions");

            migrationBuilder.DropIndex(
                name: "IX_Families_ReunionId",
                table: "Families");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ReunionId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ReunionId",
                table: "Families");

            migrationBuilder.DropColumn(
                name: "ReunionId",
                table: "AspNetUsers");
        }
    }
}
