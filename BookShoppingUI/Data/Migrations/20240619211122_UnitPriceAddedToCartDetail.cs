﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShoppingUI.Data.Migrations
{
    /// <inheritdoc />
    public partial class UnitPriceAddedToCartDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "UnitPrice",
                table: "CartDetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "CartDetails");
        }
    }
}
