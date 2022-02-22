using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kasbio.Coupon.Template.Web.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "coupon_template",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Available = table.Column<bool>(nullable: false),
                    Expired = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Logo = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    product_line = table.Column<int>(nullable: true),
                    coupon_count = table.Column<int>(nullable: false),
                    create_time = table.Column<DateTime>(nullable: false),
                    user_id = table.Column<long>(nullable: false),
                    template_key = table.Column<string>(nullable: true),
                    Target = table.Column<int>(nullable: true),
                    Rule = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_coupon_template", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "coupon_template");
        }
    }
}
