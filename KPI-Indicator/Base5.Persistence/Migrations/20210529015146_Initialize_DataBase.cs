using Microsoft.EntityFrameworkCore.Migrations;

namespace Base5.Persistence.Migrations
{
    public partial class Initialize_DataBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TblKPI",
                columns: table => new
                {
                    KPIDNum = table.Column<int>(type: "int", nullable: false),
                    KPIDDescription = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DepNo = table.Column<int>(type: "int", nullable: false),
                    MeasurmentUnit = table.Column<bool>(type: "bit", nullable: false),
                    TargetValue = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblKPI", x => x.KPIDNum);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblKPI");
        }
    }
}
