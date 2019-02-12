using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ProjectSoft.Migrations
{
    public partial class ProyectosEstadoDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProyectoImg_Id_Proyecto",
                table: "ProyectoImg");

            migrationBuilder.DropIndex(
                name: "IX_ProyectoFile_Id_Proyecto",
                table: "ProyectoFile");

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "ProyectoInfo",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProyectoImg_Id_Proyecto",
                table: "ProyectoImg",
                column: "Id_Proyecto",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProyectoFile_Id_Proyecto",
                table: "ProyectoFile",
                column: "Id_Proyecto",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProyectoImg_Id_Proyecto",
                table: "ProyectoImg");

            migrationBuilder.DropIndex(
                name: "IX_ProyectoFile_Id_Proyecto",
                table: "ProyectoFile");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "ProyectoInfo");

            migrationBuilder.CreateIndex(
                name: "IX_ProyectoImg_Id_Proyecto",
                table: "ProyectoImg",
                column: "Id_Proyecto");

            migrationBuilder.CreateIndex(
                name: "IX_ProyectoFile_Id_Proyecto",
                table: "ProyectoFile",
                column: "Id_Proyecto");
        }
    }
}
