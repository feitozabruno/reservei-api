using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reservei.Api.Migrations
{
    /// <inheritdoc />
    public partial class CreateProfessionalTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Professionals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    BusinessName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Bio = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Specialty = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ProfilePhotoUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CoverPictureUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Timezone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AppointmentDurationMinutes = table.Column<int>(type: "integer", nullable: false),
                    Address = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professionals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Professionals_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Professionals_UserId",
                table: "Professionals",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Professionals");
        }
    }
}
