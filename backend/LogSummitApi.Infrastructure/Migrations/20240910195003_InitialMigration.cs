using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogSummitApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    username = table.Column<string>(type: "character varying(155)", maxLength: 155, nullable: false),
                    email = table.Column<string>(type: "character varying(155)", maxLength: 155, nullable: false),
                    password = table.Column<string>(type: "character varying(155)", maxLength: 155, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Summit",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    country = table.Column<string>(type: "text", nullable: false),
                    is_public = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    coordinate = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Summit", x => x.id);
                    table.ForeignKey(
                        name: "FK_Summit_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Route",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    summit_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(155)", maxLength: 155, nullable: false),
                    description = table.Column<string>(type: "character varying(1555)", maxLength: 1555, nullable: false),
                    distance = table.Column<double>(type: "double precision", nullable: false),
                    elevation_gain = table.Column<double>(type: "double precision", nullable: false),
                    elevation_loss = table.Column<double>(type: "double precision", nullable: false),
                    is_public = table.Column<bool>(type: "boolean", nullable: false),
                    coordinates = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Route", x => x.id);
                    table.ForeignKey(
                        name: "FK_Route_Summit_summit_id",
                        column: x => x.summit_id,
                        principalTable: "Summit",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Route_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RouteAttempt",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    summit_id = table.Column<Guid>(type: "uuid", nullable: false),
                    route_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    is_public = table.Column<bool>(type: "boolean", nullable: false),
                    time = table.Column<TimeSpan>(type: "interval", nullable: false),
                    coordinates = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteAttempt", x => x.id);
                    table.ForeignKey(
                        name: "FK_RouteAttempt_Route_route_id",
                        column: x => x.route_id,
                        principalTable: "Route",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RouteAttempt_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Route_summit_id",
                table: "Route",
                column: "summit_id");

            migrationBuilder.CreateIndex(
                name: "IX_Route_user_id",
                table: "Route",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_RouteAttempt_route_id",
                table: "RouteAttempt",
                column: "route_id");

            migrationBuilder.CreateIndex(
                name: "IX_RouteAttempt_user_id",
                table: "RouteAttempt",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Summit_user_id",
                table: "Summit",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_email",
                table: "Users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RouteAttempt");

            migrationBuilder.DropTable(
                name: "Route");

            migrationBuilder.DropTable(
                name: "Summit");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
