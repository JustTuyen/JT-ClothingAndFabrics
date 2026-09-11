using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace clothing_be.Migrations
{
    /// <inheritdoc />
    public partial class third : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "Discounts",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ImageModelId",
                table: "Discounts",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_ImageId",
                table: "Discounts",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_ImageModelId",
                table: "Discounts",
                column: "ImageModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Discounts_Images_ImageId",
                table: "Discounts",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Discounts_Images_ImageModelId",
                table: "Discounts",
                column: "ImageModelId",
                principalTable: "Images",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discounts_Images_ImageId",
                table: "Discounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Discounts_Images_ImageModelId",
                table: "Discounts");

            migrationBuilder.DropIndex(
                name: "IX_Discounts_ImageId",
                table: "Discounts");

            migrationBuilder.DropIndex(
                name: "IX_Discounts_ImageModelId",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "ImageModelId",
                table: "Discounts");
        }
    }
}
