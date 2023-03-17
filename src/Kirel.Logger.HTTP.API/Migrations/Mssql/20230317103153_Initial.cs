using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kirel.Logger.HTTP.API.Migrations.Mssql
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HttpLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", maxLength: 36, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Source = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Host = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Path = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Method = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Protocol = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ClientIp = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    QueryString = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RequestHeaders = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseHeaders = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseCode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HttpLogs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HttpLogs_Username_Source_Host_Path_Method_Protocol_ClientIp_RequestId_ResponseCode",
                table: "HttpLogs",
                columns: new[] { "Username", "Source", "Host", "Path", "Method", "Protocol", "ClientIp", "RequestId", "ResponseCode" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HttpLogs");
        }
    }
}
