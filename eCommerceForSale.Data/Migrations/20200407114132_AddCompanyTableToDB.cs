using Microsoft.EntityFrameworkCore.Migrations;

namespace eCommerceForSale.Data.Migrations
{
    public partial class AddCompanyTableToDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(nullable: false),
                    LogoPath = table.Column<string>(nullable: false),
                    OwenerName = table.Column<string>(nullable: false),
                    CompanyEmail = table.Column<string>(nullable: true),
                    TaxNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Company");
        }
    }
}
