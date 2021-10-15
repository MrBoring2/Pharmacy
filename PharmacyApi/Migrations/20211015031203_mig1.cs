using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PharmacyApi.Migrations
{
    public partial class mig1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Analizers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnalizerName = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Analizers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InsuranceСompany",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    INN = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: false),
                    CheckingAccount = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    BIC = table.Column<string>(type: "varchar(9)", unicode: false, maxLength: 9, nullable: false),
                    Country = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsuranceСompany", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LaboratiryServices",
                columns: table => new
                {
                    Code = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaboratiryServices", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvoicesIssued",
                columns: table => new
                {
                    InsuranceCompanyId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    StartPeriod = table.Column<DateTime>(type: "date", nullable: false),
                    EndPeriod = table.Column<DateTime>(type: "date", nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoicesIssued", x => new { x.InsuranceCompanyId, x.UserId });
                    table.ForeignKey(
                        name: "FK_InvoicesIssued_InsuranceСompany",
                        column: x => x.InsuranceCompanyId,
                        principalTable: "InsuranceСompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Login = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Password = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    SosialSecNumber = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    EIN = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    SosialType = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false),
                    Telephone = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    PassportSeries = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: false),
                    PassportNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    DateOfBirth = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    UA = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    InsuranceCompanyId = table.Column<int>(type: "int", nullable: true),
                    IpAddress = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.PatientId);
                    table.ForeignKey(
                        name: "FK_Patients_InsuranceСompany",
                        column: x => x.InsuranceCompanyId,
                        principalTable: "InsuranceСompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Login = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Password = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: false),
                    Ip = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    LastEnterDate = table.Column<DateTime>(type: "date", nullable: false),
                    ServicesCodes = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    DateOfCreation = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Barcode = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_Patients",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AnalizerWork",
                columns: table => new
                {
                    AnalizerId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    OrderDateOfReceipt = table.Column<DateTime>(type: "datetime", nullable: false),
                    OrderDateOFCompletion = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_AnalizerWork_Analizers",
                        column: x => x.AnalizerId,
                        principalTable: "Analizers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnalizerWork_Order",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LaboratoryServicesToOrder",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    LaboratoryServiceId = table.Column<int>(type: "int", nullable: false),
                    Result = table.Column<double>(type: "float", nullable: false),
                    DateOfFinished = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Accepted = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    AnalyzerId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_LaboratoryServicesToOrder_Analizers",
                        column: x => x.AnalyzerId,
                        principalTable: "Analizers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LaboratoryServicesToOrder_LaboratiryServices",
                        column: x => x.LaboratoryServiceId,
                        principalTable: "LaboratiryServices",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LaboratoryServicesToOrder_Order",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LaboratoryServicesToOrder_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnalizerWork_AnalizerId",
                table: "AnalizerWork",
                column: "AnalizerId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalizerWork_OrderId",
                table: "AnalizerWork",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_LaboratoryServicesToOrder_AnalyzerId",
                table: "LaboratoryServicesToOrder",
                column: "AnalyzerId");

            migrationBuilder.CreateIndex(
                name: "IX_LaboratoryServicesToOrder_LaboratoryServiceId",
                table: "LaboratoryServicesToOrder",
                column: "LaboratoryServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_LaboratoryServicesToOrder_OrderId",
                table: "LaboratoryServicesToOrder",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_LaboratoryServicesToOrder_UserId",
                table: "LaboratoryServicesToOrder",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_PatientId",
                table: "Order",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_InsuranceCompanyId",
                table: "Patients",
                column: "InsuranceCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnalizerWork");

            migrationBuilder.DropTable(
                name: "InvoicesIssued");

            migrationBuilder.DropTable(
                name: "LaboratoryServicesToOrder");

            migrationBuilder.DropTable(
                name: "Analizers");

            migrationBuilder.DropTable(
                name: "LaboratiryServices");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "InsuranceСompany");
        }
    }
}
