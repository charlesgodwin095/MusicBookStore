﻿namespace MusicBookStore.Data.Migrations
{
    public partial class AddProductionRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder) 
        {
            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0); 
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Products");
        }
    }

}
