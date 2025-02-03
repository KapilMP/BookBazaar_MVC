﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookBazaar.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updatePropertyOfOrderHeader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "orderHeaders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "orderHeaders");
        }
    }
}
