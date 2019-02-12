using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ProjectSoft.Migrations
{
    public partial class ProyectDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProyectoInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Costo = table.Column<double>(nullable: false),
                    Creador = table.Column<string>(maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(maxLength: 100, nullable: false),
                    Duracion = table.Column<string>(nullable: false),
                    Id_Categoria = table.Column<int>(nullable: false),
                    Id_Cliente = table.Column<int>(nullable: false),
                    Identificacion = table.Column<string>(maxLength: 10, nullable: false),
                    Nombre = table.Column<string>(maxLength: 100, nullable: false),
                    UpLoader = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProyectoInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProyectoInfo_Categoria_Id_Categoria",
                        column: x => x.Id_Categoria,
                        principalTable: "Categoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProyectoInfo_Cliente_Id_Cliente",
                        column: x => x.Id_Cliente,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProyectoFile",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ArchivoRar = table.Column<string>(nullable: true),
                    Contrato = table.Column<string>(nullable: true),
                    Id_Proyecto = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProyectoFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProyectoFile_ProyectoInfo_Id_Proyecto",
                        column: x => x.Id_Proyecto,
                        principalTable: "ProyectoInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProyectoImg",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Id_Proyecto = table.Column<int>(nullable: false),
                    LogoProyecto = table.Column<string>(nullable: true),
                    Screen1 = table.Column<string>(nullable: true),
                    Screen2 = table.Column<string>(nullable: true),
                    Screen3 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProyectoImg", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProyectoImg_ProyectoInfo_Id_Proyecto",
                        column: x => x.Id_Proyecto,
                        principalTable: "ProyectoInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProyectoFile_Id_Proyecto",
                table: "ProyectoFile",
                column: "Id_Proyecto");

            migrationBuilder.CreateIndex(
                name: "IX_ProyectoImg_Id_Proyecto",
                table: "ProyectoImg",
                column: "Id_Proyecto");

            migrationBuilder.CreateIndex(
                name: "IX_ProyectoInfo_Id_Categoria",
                table: "ProyectoInfo",
                column: "Id_Categoria");

            migrationBuilder.CreateIndex(
                name: "IX_ProyectoInfo_Id_Cliente",
                table: "ProyectoInfo",
                column: "Id_Cliente");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProyectoFile");

            migrationBuilder.DropTable(
                name: "ProyectoImg");

            migrationBuilder.DropTable(
                name: "ProyectoInfo");
        }
    }
}
