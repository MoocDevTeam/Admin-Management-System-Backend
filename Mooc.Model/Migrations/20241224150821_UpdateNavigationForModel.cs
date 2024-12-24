using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mooc.Model.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNavigationForModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_JudgementQuestion_Marks",
                table: "JudgementQuestion");

            migrationBuilder.AddColumn<long>(
                name: "ChoiceQuestionId",
                table: "User",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "JudgementQuestionId",
                table: "User",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OptionId",
                table: "User",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ShortAnsQuestionId",
                table: "User",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedByUserId1",
                table: "ShortAnsQuestion",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "QuestionTypeId1",
                table: "ShortAnsQuestion",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QuestionTypeName",
                table: "QuestionType",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "QuestionType",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<long>(
                name: "ChoiceQuestionId1",
                table: "Option",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedByUserId1",
                table: "Option",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedByUserId1",
                table: "JudgementQuestion",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "QuestionTypeId1",
                table: "JudgementQuestion",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedByUserId1",
                table: "ChoiceQuestion",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "QuestionTypeId1",
                table: "ChoiceQuestion",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_ChoiceQuestionId",
                table: "User",
                column: "ChoiceQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_User_JudgementQuestionId",
                table: "User",
                column: "JudgementQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_User_OptionId",
                table: "User",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_User_ShortAnsQuestionId",
                table: "User",
                column: "ShortAnsQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_ShortAnsQuestion_CreatedByUserId1",
                table: "ShortAnsQuestion",
                column: "CreatedByUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_ShortAnsQuestion_QuestionTypeId1",
                table: "ShortAnsQuestion",
                column: "QuestionTypeId1");

            migrationBuilder.CreateIndex(
                name: "IX_Option_ChoiceQuestionId1",
                table: "Option",
                column: "ChoiceQuestionId1");

            migrationBuilder.CreateIndex(
                name: "IX_Option_CreatedByUserId1",
                table: "Option",
                column: "CreatedByUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_JudgementQuestion_CreatedByUserId1",
                table: "JudgementQuestion",
                column: "CreatedByUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_JudgementQuestion_QuestionTypeId1",
                table: "JudgementQuestion",
                column: "QuestionTypeId1");

            migrationBuilder.AddCheckConstraint(
                name: "CK_JudgementQuestionn_Marks",
                table: "JudgementQuestion",
                sql: "[Marks] >= 0 AND [Marks] <= 100");

            migrationBuilder.CreateIndex(
                name: "IX_ChoiceQuestion_CreatedByUserId1",
                table: "ChoiceQuestion",
                column: "CreatedByUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_ChoiceQuestion_QuestionTypeId1",
                table: "ChoiceQuestion",
                column: "QuestionTypeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ChoiceQuestion_QuestionType_QuestionTypeId1",
                table: "ChoiceQuestion",
                column: "QuestionTypeId1",
                principalTable: "QuestionType",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChoiceQuestion_User_CreatedByUserId1",
                table: "ChoiceQuestion",
                column: "CreatedByUserId1",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JudgementQuestion_QuestionType_QuestionTypeId1",
                table: "JudgementQuestion",
                column: "QuestionTypeId1",
                principalTable: "QuestionType",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JudgementQuestion_User_CreatedByUserId1",
                table: "JudgementQuestion",
                column: "CreatedByUserId1",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Option_ChoiceQuestion_ChoiceQuestionId1",
                table: "Option",
                column: "ChoiceQuestionId1",
                principalTable: "ChoiceQuestion",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Option_User_CreatedByUserId1",
                table: "Option",
                column: "CreatedByUserId1",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShortAnsQuestion_QuestionType_QuestionTypeId1",
                table: "ShortAnsQuestion",
                column: "QuestionTypeId1",
                principalTable: "QuestionType",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShortAnsQuestion_User_CreatedByUserId1",
                table: "ShortAnsQuestion",
                column: "CreatedByUserId1",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_User_ChoiceQuestion_ChoiceQuestionId",
                table: "User",
                column: "ChoiceQuestionId",
                principalTable: "ChoiceQuestion",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_User_JudgementQuestion_JudgementQuestionId",
                table: "User",
                column: "JudgementQuestionId",
                principalTable: "JudgementQuestion",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Option_OptionId",
                table: "User",
                column: "OptionId",
                principalTable: "Option",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_User_ShortAnsQuestion_ShortAnsQuestionId",
                table: "User",
                column: "ShortAnsQuestionId",
                principalTable: "ShortAnsQuestion",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChoiceQuestion_QuestionType_QuestionTypeId1",
                table: "ChoiceQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_ChoiceQuestion_User_CreatedByUserId1",
                table: "ChoiceQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_JudgementQuestion_QuestionType_QuestionTypeId1",
                table: "JudgementQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_JudgementQuestion_User_CreatedByUserId1",
                table: "JudgementQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_Option_ChoiceQuestion_ChoiceQuestionId1",
                table: "Option");

            migrationBuilder.DropForeignKey(
                name: "FK_Option_User_CreatedByUserId1",
                table: "Option");

            migrationBuilder.DropForeignKey(
                name: "FK_ShortAnsQuestion_QuestionType_QuestionTypeId1",
                table: "ShortAnsQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_ShortAnsQuestion_User_CreatedByUserId1",
                table: "ShortAnsQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_User_ChoiceQuestion_ChoiceQuestionId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_JudgementQuestion_JudgementQuestionId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Option_OptionId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_ShortAnsQuestion_ShortAnsQuestionId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_ChoiceQuestionId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_JudgementQuestionId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_OptionId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_ShortAnsQuestionId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_ShortAnsQuestion_CreatedByUserId1",
                table: "ShortAnsQuestion");

            migrationBuilder.DropIndex(
                name: "IX_ShortAnsQuestion_QuestionTypeId1",
                table: "ShortAnsQuestion");

            migrationBuilder.DropIndex(
                name: "IX_Option_ChoiceQuestionId1",
                table: "Option");

            migrationBuilder.DropIndex(
                name: "IX_Option_CreatedByUserId1",
                table: "Option");

            migrationBuilder.DropIndex(
                name: "IX_JudgementQuestion_CreatedByUserId1",
                table: "JudgementQuestion");

            migrationBuilder.DropIndex(
                name: "IX_JudgementQuestion_QuestionTypeId1",
                table: "JudgementQuestion");

            migrationBuilder.DropCheckConstraint(
                name: "CK_JudgementQuestionn_Marks",
                table: "JudgementQuestion");

            migrationBuilder.DropIndex(
                name: "IX_ChoiceQuestion_CreatedByUserId1",
                table: "ChoiceQuestion");

            migrationBuilder.DropIndex(
                name: "IX_ChoiceQuestion_QuestionTypeId1",
                table: "ChoiceQuestion");

            migrationBuilder.DropColumn(
                name: "ChoiceQuestionId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "JudgementQuestionId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "OptionId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ShortAnsQuestionId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId1",
                table: "ShortAnsQuestion");

            migrationBuilder.DropColumn(
                name: "QuestionTypeId1",
                table: "ShortAnsQuestion");

            migrationBuilder.DropColumn(
                name: "ChoiceQuestionId1",
                table: "Option");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId1",
                table: "Option");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId1",
                table: "JudgementQuestion");

            migrationBuilder.DropColumn(
                name: "QuestionTypeId1",
                table: "JudgementQuestion");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId1",
                table: "ChoiceQuestion");

            migrationBuilder.DropColumn(
                name: "QuestionTypeId1",
                table: "ChoiceQuestion");

            migrationBuilder.AlterColumn<int>(
                name: "QuestionTypeName",
                table: "QuestionType",
                type: "INTEGER",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "QuestionType",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_JudgementQuestion_Marks",
                table: "JudgementQuestion",
                sql: "[Marks] >= 0 AND [Marks] <= 100");
        }
    }
}
