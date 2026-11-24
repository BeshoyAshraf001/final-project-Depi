using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MobileAdvisor.DAL.Migrations;

/// <inheritdoc />
public partial class _20261119172105_init : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Phones",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Brand = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Model = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                GuidePrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                Specifications = table.Column<string>(type: "nvarchar(max)", nullable: true),
                IsActive = table.Column<bool>(type: "bit", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Phones", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Specialties",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Specialties", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Role = table.Column<int>(type: "int", nullable: false),
                Status = table.Column<int>(type: "int", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Consultations",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                CustomerId = table.Column<int>(type: "int", nullable: false),
                ExpertId = table.Column<int>(type: "int", nullable: false),
                Type = table.Column<int>(type: "int", nullable: false),
                Status = table.Column<int>(type: "int", nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Budget = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                PrioritiesAndUsage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                UsedPhoneModel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                UsedPhoneAskingPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                UsedPhoneConditionNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                AgreedPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Consultations", x => x.Id);
                table.ForeignKey(
                    name: "FK_Consultations_Users_CustomerId",
                    column: x => x.CustomerId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Consultations_Users_ExpertId",
                    column: x => x.ExpertId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "ExpertProfiles",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                UserId = table.Column<int>(type: "int", nullable: false),
                Bio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                YearsOfExperience = table.Column<int>(type: "int", nullable: false),
                DefaultFee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                IsApproved = table.Column<bool>(type: "bit", nullable: false),
                ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ExpertProfiles", x => x.Id);
                table.ForeignKey(
                    name: "FK_ExpertProfiles_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Notifications",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                UserId = table.Column<int>(type: "int", nullable: false),
                Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                IsRead = table.Column<bool>(type: "bit", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Notifications", x => x.Id);
                table.ForeignKey(
                    name: "FK_Notifications_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ConsultationAttachments",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ConsultationId = table.Column<int>(type: "int", nullable: false),
                FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                FileSizeBytes = table.Column<long>(type: "bigint", nullable: false),
                UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ConsultationAttachments", x => x.Id);
                table.ForeignKey(
                    name: "FK_ConsultationAttachments_Consultations_ConsultationId",
                    column: x => x.ConsultationId,
                    principalTable: "Consultations",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ExpertRecommendations",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ConsultationId = table.Column<int>(type: "int", nullable: false),
                RecommendationText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                InitialValuation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                InspectionChecklist = table.Column<string>(type: "nvarchar(max)", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ExpertRecommendations", x => x.Id);
                table.ForeignKey(
                    name: "FK_ExpertRecommendations_Consultations_ConsultationId",
                    column: x => x.ConsultationId,
                    principalTable: "Consultations",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Messages",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ConsultationId = table.Column<int>(type: "int", nullable: false),
                SenderId = table.Column<int>(type: "int", nullable: false),
                Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                SentAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Messages", x => x.Id);
                table.ForeignKey(
                    name: "FK_Messages_Consultations_ConsultationId",
                    column: x => x.ConsultationId,
                    principalTable: "Consultations",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Messages_Users_SenderId",
                    column: x => x.SenderId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Payments",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ConsultationId = table.Column<int>(type: "int", nullable: false),
                Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                Status = table.Column<int>(type: "int", nullable: false),
                TransactionReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Payments", x => x.Id);
                table.ForeignKey(
                    name: "FK_Payments_Consultations_ConsultationId",
                    column: x => x.ConsultationId,
                    principalTable: "Consultations",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Reviews",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ConsultationId = table.Column<int>(type: "int", nullable: false),
                CustomerId = table.Column<int>(type: "int", nullable: false),
                ExpertId = table.Column<int>(type: "int", nullable: false),
                Rating = table.Column<int>(type: "int", nullable: false),
                Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Reviews", x => x.Id);
                table.ForeignKey(
                    name: "FK_Reviews_Consultations_ConsultationId",
                    column: x => x.ConsultationId,
                    principalTable: "Consultations",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Reviews_Users_CustomerId",
                    column: x => x.CustomerId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Reviews_Users_ExpertId",
                    column: x => x.ExpertId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "ExpertSpecialties",
            columns: table => new
            {
                ExpertProfileId = table.Column<int>(type: "int", nullable: false),
                SpecialtyId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ExpertSpecialties", x => new { x.ExpertProfileId, x.SpecialtyId });
                table.ForeignKey(
                    name: "FK_ExpertSpecialties_ExpertProfiles_ExpertProfileId",
                    column: x => x.ExpertProfileId,
                    principalTable: "ExpertProfiles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_ExpertSpecialties_Specialties_SpecialtyId",
                    column: x => x.SpecialtyId,
                    principalTable: "Specialties",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ConsultationAttachments_ConsultationId",
            table: "ConsultationAttachments",
            column: "ConsultationId");

        migrationBuilder.CreateIndex(
            name: "IX_Consultations_CustomerId",
            table: "Consultations",
            column: "CustomerId");

        migrationBuilder.CreateIndex(
            name: "IX_Consultations_ExpertId",
            table: "Consultations",
            column: "ExpertId");

        migrationBuilder.CreateIndex(
            name: "IX_ExpertProfiles_UserId",
            table: "ExpertProfiles",
            column: "UserId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_ExpertRecommendations_ConsultationId",
            table: "ExpertRecommendations",
            column: "ConsultationId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_ExpertSpecialties_SpecialtyId",
            table: "ExpertSpecialties",
            column: "SpecialtyId");

        migrationBuilder.CreateIndex(
            name: "IX_Messages_ConsultationId",
            table: "Messages",
            column: "ConsultationId");

        migrationBuilder.CreateIndex(
            name: "IX_Messages_SenderId",
            table: "Messages",
            column: "SenderId");

        migrationBuilder.CreateIndex(
            name: "IX_Notifications_UserId",
            table: "Notifications",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_Payments_ConsultationId",
            table: "Payments",
            column: "ConsultationId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Reviews_ConsultationId",
            table: "Reviews",
            column: "ConsultationId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Reviews_CustomerId",
            table: "Reviews",
            column: "CustomerId");

        migrationBuilder.CreateIndex(
            name: "IX_Reviews_ExpertId",
            table: "Reviews",
            column: "ExpertId");

        migrationBuilder.CreateIndex(
            name: "IX_Users_Email",
            table: "Users",
            column: "Email",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ConsultationAttachments");

        migrationBuilder.DropTable(
            name: "ExpertRecommendations");

        migrationBuilder.DropTable(
            name: "ExpertSpecialties");

        migrationBuilder.DropTable(
            name: "Messages");

        migrationBuilder.DropTable(
            name: "Notifications");

        migrationBuilder.DropTable(
            name: "Payments");

        migrationBuilder.DropTable(
            name: "Phones");

        migrationBuilder.DropTable(
            name: "Reviews");

        migrationBuilder.DropTable(
            name: "ExpertProfiles");

        migrationBuilder.DropTable(
            name: "Specialties");

        migrationBuilder.DropTable(
            name: "Consultations");

        migrationBuilder.DropTable(
            name: "Users");
    }
}
