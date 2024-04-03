using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBanNoiThat.Migrations
{
    public partial class updb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fullname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountId);
                });

            migrationBuilder.CreateTable(
                name: "DanhMucSps",
                columns: table => new
                {
                    MaDm = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnhDm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoTaDm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhMucSps", x => x.MaDm);
                });

            migrationBuilder.CreateTable(
                name: "DonHangs",
                columns: table => new
                {
                    MaDh = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TypePayment = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonHangs", x => x.MaDh);
                    table.ForeignKey(
                        name: "FK_DonHangs_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SanPham",
                columns: table => new
                {
                    MaSP = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDM = table.Column<int>(type: "int", nullable: true),
                    TenSP = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AnhSP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GiaSP = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false),
                    BestSeller = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: false),
                    NgaySua = table.Column<DateTime>(type: "date", nullable: false),
                    MotaSP = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SanPham__2725081C535AFC31", x => x.MaSP);
                    table.ForeignKey(
                        name: "FK__SanPham__MotaSP__286302EC",
                        column: x => x.MaDM,
                        principalTable: "DanhMucSps",
                        principalColumn: "MaDm");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDonHangs",
                columns: table => new
                {
                    MaCtdh = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDh = table.Column<int>(type: "int", nullable: false),
                    MaSp = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    Gia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DonHangMaDh = table.Column<int>(type: "int", nullable: true),
                    SanPhamMaSp = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietDonHangs", x => x.MaCtdh);
                    table.ForeignKey(
                        name: "FK_ChiTietDonHangs_DonHangs_DonHangMaDh",
                        column: x => x.DonHangMaDh,
                        principalTable: "DonHangs",
                        principalColumn: "MaDh");
                    table.ForeignKey(
                        name: "FK_ChiTietDonHangs_SanPham_SanPhamMaSp",
                        column: x => x.SanPhamMaSp,
                        principalTable: "SanPham",
                        principalColumn: "MaSP");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHangs_DonHangMaDh",
                table: "ChiTietDonHangs",
                column: "DonHangMaDh");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHangs_SanPhamMaSp",
                table: "ChiTietDonHangs",
                column: "SanPhamMaSp");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_AccountId",
                table: "DonHangs",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_SanPham_MaDM",
                table: "SanPham",
                column: "MaDM");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietDonHangs");

            migrationBuilder.DropTable(
                name: "DonHangs");

            migrationBuilder.DropTable(
                name: "SanPham");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "DanhMucSps");
        }
    }
}
