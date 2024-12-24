using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mooc.Model.Migrations
{
    /// <inheritdoc />
    public partial class CreateUpdateQuestionBankStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuestionType",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QuestionTypeName = table.Column<int>(type: "INTEGER", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChoiceQuestion",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    QuestionTypeId = table.Column<long>(type: "INTEGER", nullable: false),
                    CourseId = table.Column<long>(type: "INTEGER", nullable: false),
                    CreatedByUserId = table.Column<long>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedByUserId = table.Column<long>(type: "INTEGER", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()"),
                    QuestionBody = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    QuestionTitle = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Marks = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChoiceQuestion", x => x.Id);
                    table.CheckConstraint("CK_ChoiceQuestion_Marks", "[Marks] >= 0 AND [Marks] <= 100");
                    table.ForeignKey(
                        name: "FK_ChoiceQuestion_QuestionType_QuestionTypeId",
                        column: x => x.QuestionTypeId,
                        principalTable: "QuestionType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChoiceQuestion_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChoiceQuestion_User_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JudgementQuestion",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    CorrectAnswer = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    QuestionTypeId = table.Column<long>(type: "INTEGER", nullable: false),
                    CourseId = table.Column<long>(type: "INTEGER", nullable: false),
                    CreatedByUserId = table.Column<long>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedByUserId = table.Column<long>(type: "INTEGER", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()"),
                    QuestionBody = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    QuestionTitle = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Marks = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JudgementQuestion", x => x.Id);
                    table.CheckConstraint("CK_JudgementQuestion_Marks", "[Marks] >= 0 AND [Marks] <= 100");
                    table.ForeignKey(
                        name: "FK_JudgementQuestion_QuestionType_QuestionTypeId",
                        column: x => x.QuestionTypeId,
                        principalTable: "QuestionType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JudgementQuestion_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JudgementQuestion_User_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShortAnsQuestion",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    ReferenceAnswer = table.Column<string>(type: "TEXT", maxLength: 1500, nullable: false),
                    QuestionTypeId = table.Column<long>(type: "INTEGER", nullable: false),
                    CourseId = table.Column<long>(type: "INTEGER", nullable: false),
                    CreatedByUserId = table.Column<long>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedByUserId = table.Column<long>(type: "INTEGER", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()"),
                    QuestionBody = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    QuestionTitle = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Marks = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortAnsQuestion", x => x.Id);
                    table.CheckConstraint("CK_ShortAnsQuestion_Marks", "[Marks] >= 0 AND [Marks] <= 100");
                    table.ForeignKey(
                        name: "FK_ShortAnsQuestion_QuestionType_QuestionTypeId",
                        column: x => x.QuestionTypeId,
                        principalTable: "QuestionType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShortAnsQuestion_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShortAnsQuestion_User_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Option",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    ChoiceQuestionId = table.Column<long>(type: "INTEGER", nullable: false),
                    OptionOrder = table.Column<long>(type: "INTEGER", maxLength: 1, nullable: false),
                    OptionValue = table.Column<string>(type: "TEXT", maxLength: 1, nullable: false),
                    CreatedByUserId = table.Column<long>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedByUserId = table.Column<long>(type: "INTEGER", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()"),
                    Field = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()"),
                    ErrorExplanation = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Option", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Option_ChoiceQuestion_ChoiceQuestionId",
                        column: x => x.ChoiceQuestionId,
                        principalTable: "ChoiceQuestion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Option_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Option_User_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChoiceQuestion_CreatedByUserId",
                table: "ChoiceQuestion",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChoiceQuestion_QuestionTypeId",
                table: "ChoiceQuestion",
                column: "QuestionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ChoiceQuestion_UpdatedByUserId",
                table: "ChoiceQuestion",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_JudgementQuestion_CreatedByUserId",
                table: "JudgementQuestion",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_JudgementQuestion_QuestionTypeId",
                table: "JudgementQuestion",
                column: "QuestionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_JudgementQuestion_UpdatedByUserId",
                table: "JudgementQuestion",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Option_ChoiceQuestionId",
                table: "Option",
                column: "ChoiceQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Option_CreatedByUserId",
                table: "Option",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Option_UpdatedByUserId",
                table: "Option",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShortAnsQuestion_CreatedByUserId",
                table: "ShortAnsQuestion",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShortAnsQuestion_QuestionTypeId",
                table: "ShortAnsQuestion",
                column: "QuestionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ShortAnsQuestion_UpdatedByUserId",
                table: "ShortAnsQuestion",
                column: "UpdatedByUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JudgementQuestion");

            migrationBuilder.DropTable(
                name: "Option");

            migrationBuilder.DropTable(
                name: "ShortAnsQuestion");

            migrationBuilder.DropTable(
                name: "ChoiceQuestion");

            migrationBuilder.DropTable(
                name: "QuestionType");
        }
    }
}
