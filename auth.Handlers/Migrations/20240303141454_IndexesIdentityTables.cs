using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace auth.Handlers.Migrations
{
    /// <inheritdoc />
    public partial class IndexesIdentityTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "auth",
                table: "users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "auth",
                table: "users",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "auth",
                table: "roles",
                column: "NormalizedName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
               name: "EmailIndex",
               schema: "auth",
               table: "users"
               );

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                schema: "auth",
                table: "users"
                );

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                schema: "auth",
                table: "roles"
                );

        }
    }
}
