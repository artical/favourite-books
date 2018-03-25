using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MyFavouriteBooks.Migrations.ApplicationDb
{
    public partial class ForeignKeysChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBooks_Books_UserId",
                table: "UserBooks");

            migrationBuilder.DropIndex(
                name: "IX_UserBooks_UserId",
                table: "UserBooks");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBooks_Books_ISBN",
                table: "UserBooks",
                column: "ISBN",
                principalTable: "Books",
                principalColumn: "ISBN",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBooks_Books_ISBN",
                table: "UserBooks");

            migrationBuilder.CreateIndex(
                name: "IX_UserBooks_UserId",
                table: "UserBooks",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBooks_Books_UserId",
                table: "UserBooks",
                column: "UserId",
                principalTable: "Books",
                principalColumn: "ISBN",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
