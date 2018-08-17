using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CultistMetrics.Migrations
{
    public partial class recriate_situation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MetaInfos",
                columns: table => new
                {
                    MetaId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Version = table.Column<string>(nullable: true),
                    BirdWormSlider = table.Column<int>(nullable: false),
                    WeAwaitSTE = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaInfos", x => x.MetaId);
                });

            migrationBuilder.CreateTable(
                name: "SaveFiles",
                columns: table => new
                {
                    FileId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Processed = table.Column<int>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    FileTime = table.Column<DateTime>(nullable: false),
                    MetaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaveFiles", x => x.FileId);
                    table.ForeignKey(
                        name: "FK_SaveFiles_MetaInfos_MetaId",
                        column: x => x.MetaId,
                        principalTable: "MetaInfos",
                        principalColumn: "MetaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CharacterDetails",
                columns: table => new
                {
                    CharacterId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Profession = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    ActiveLegacy = table.Column<string>(nullable: true),
                    FileId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterDetails", x => x.CharacterId);
                    table.ForeignKey(
                        name: "FK_CharacterDetails_SaveFiles_FileId",
                        column: x => x.FileId,
                        principalTable: "SaveFiles",
                        principalColumn: "FileId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Decks",
                columns: table => new
                {
                    DeckId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    FileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decks", x => x.DeckId);
                    table.ForeignKey(
                        name: "FK_Decks_SaveFiles_FileId",
                        column: x => x.FileId,
                        principalTable: "SaveFiles",
                        principalColumn: "FileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ElementStacks",
                columns: table => new
                {
                    ElementStackId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ElementStackIdentification = table.Column<string>(nullable: true),
                    FileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElementStacks", x => x.ElementStackId);
                    table.ForeignKey(
                        name: "FK_ElementStacks_SaveFiles_FileId",
                        column: x => x.FileId,
                        principalTable: "SaveFiles",
                        principalColumn: "FileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Situations",
                columns: table => new
                {
                    SituationId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SituationIdentification = table.Column<string>(nullable: true),
                    FileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Situations", x => x.SituationId);
                    table.ForeignKey(
                        name: "FK_Situations_SaveFiles_FileId",
                        column: x => x.FileId,
                        principalTable: "SaveFiles",
                        principalColumn: "FileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Levers",
                columns: table => new
                {
                    LeverId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    Part = table.Column<int>(nullable: false),
                    CharacterDetailCharacterId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Levers", x => x.LeverId);
                    table.ForeignKey(
                        name: "FK_Levers_CharacterDetails_CharacterDetailCharacterId",
                        column: x => x.CharacterDetailCharacterId,
                        principalTable: "CharacterDetails",
                        principalColumn: "CharacterId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DeckItems",
                columns: table => new
                {
                    DeckItemId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    Eliminated = table.Column<bool>(nullable: false),
                    DeckId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeckItems", x => x.DeckItemId);
                    table.ForeignKey(
                        name: "FK_DeckItems_Decks_DeckId",
                        column: x => x.DeckId,
                        principalTable: "Decks",
                        principalColumn: "DeckId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ElementStackItems",
                columns: table => new
                {
                    ElementStackItemId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    ElementStackId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElementStackItems", x => x.ElementStackItemId);
                    table.ForeignKey(
                        name: "FK_ElementStackItems_ElementStacks_ElementStackId",
                        column: x => x.ElementStackId,
                        principalTable: "ElementStacks",
                        principalColumn: "ElementStackId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OngoingSlotElements",
                columns: table => new
                {
                    OngoingSlotElementId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OngoingSlotElementIdentification = table.Column<string>(nullable: true),
                    SituationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OngoingSlotElements", x => x.OngoingSlotElementId);
                    table.ForeignKey(
                        name: "FK_OngoingSlotElements_Situations_SituationId",
                        column: x => x.SituationId,
                        principalTable: "Situations",
                        principalColumn: "SituationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SituationItems",
                columns: table => new
                {
                    SituationItemId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Value = table.Column<string>(nullable: true),
                    Key = table.Column<string>(nullable: true),
                    SituationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SituationItems", x => x.SituationItemId);
                    table.ForeignKey(
                        name: "FK_SituationItems_Situations_SituationId",
                        column: x => x.SituationId,
                        principalTable: "Situations",
                        principalColumn: "SituationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SituationOutputNotes",
                columns: table => new
                {
                    SituationOutputNotesId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Index = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    Key = table.Column<string>(nullable: true),
                    SituationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SituationOutputNotes", x => x.SituationOutputNotesId);
                    table.ForeignKey(
                        name: "FK_SituationOutputNotes_Situations_SituationId",
                        column: x => x.SituationId,
                        principalTable: "Situations",
                        principalColumn: "SituationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SituationOutputStacks",
                columns: table => new
                {
                    SituationOutputStacksId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SituationOutputStacksIdentification = table.Column<string>(nullable: true),
                    SituationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SituationOutputStacks", x => x.SituationOutputStacksId);
                    table.ForeignKey(
                        name: "FK_SituationOutputStacks_Situations_SituationId",
                        column: x => x.SituationId,
                        principalTable: "Situations",
                        principalColumn: "SituationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SituationStoredElements",
                columns: table => new
                {
                    SituationStoredElementId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SituationStoredElementIdentification = table.Column<string>(nullable: true),
                    SituationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SituationStoredElements", x => x.SituationStoredElementId);
                    table.ForeignKey(
                        name: "FK_SituationStoredElements_Situations_SituationId",
                        column: x => x.SituationId,
                        principalTable: "Situations",
                        principalColumn: "SituationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StartingSlotElements",
                columns: table => new
                {
                    StartingSlotElementId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartingSlotElementIdentification = table.Column<string>(nullable: true),
                    SituationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StartingSlotElements", x => x.StartingSlotElementId);
                    table.ForeignKey(
                        name: "FK_StartingSlotElements_Situations_SituationId",
                        column: x => x.SituationId,
                        principalTable: "Situations",
                        principalColumn: "SituationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OngoingSlotElementItems",
                columns: table => new
                {
                    OngoingSlotElementItemId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    OngoingSlotElementId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OngoingSlotElementItems", x => x.OngoingSlotElementItemId);
                    table.ForeignKey(
                        name: "FK_OngoingSlotElementItems_OngoingSlotElements_OngoingSlotElementId",
                        column: x => x.OngoingSlotElementId,
                        principalTable: "OngoingSlotElements",
                        principalColumn: "OngoingSlotElementId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SituationOutputStackItems",
                columns: table => new
                {
                    SituationOutputStackItemId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    SituationOutputStackId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SituationOutputStackItems", x => x.SituationOutputStackItemId);
                    table.ForeignKey(
                        name: "FK_SituationOutputStackItems_SituationOutputStacks_SituationOutputStackId",
                        column: x => x.SituationOutputStackId,
                        principalTable: "SituationOutputStacks",
                        principalColumn: "SituationOutputStacksId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SituationStoredElementItems",
                columns: table => new
                {
                    SituationStoredElementItemId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    SituationStoredElementId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SituationStoredElementItems", x => x.SituationStoredElementItemId);
                    table.ForeignKey(
                        name: "FK_SituationStoredElementItems_SituationStoredElements_SituationStoredElementId",
                        column: x => x.SituationStoredElementId,
                        principalTable: "SituationStoredElements",
                        principalColumn: "SituationStoredElementId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StartingSlotElementItems",
                columns: table => new
                {
                    StartingSlotElementItemId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    StartingSlotElementId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StartingSlotElementItems", x => x.StartingSlotElementItemId);
                    table.ForeignKey(
                        name: "FK_StartingSlotElementItems_StartingSlotElements_StartingSlotElementId",
                        column: x => x.StartingSlotElementId,
                        principalTable: "StartingSlotElements",
                        principalColumn: "StartingSlotElementId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterDetails_FileId",
                table: "CharacterDetails",
                column: "FileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeckItems_DeckId",
                table: "DeckItems",
                column: "DeckId");

            migrationBuilder.CreateIndex(
                name: "IX_Decks_FileId",
                table: "Decks",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_ElementStackItems_ElementStackId",
                table: "ElementStackItems",
                column: "ElementStackId");

            migrationBuilder.CreateIndex(
                name: "IX_ElementStacks_FileId",
                table: "ElementStacks",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Levers_CharacterDetailCharacterId",
                table: "Levers",
                column: "CharacterDetailCharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_OngoingSlotElementItems_OngoingSlotElementId",
                table: "OngoingSlotElementItems",
                column: "OngoingSlotElementId");

            migrationBuilder.CreateIndex(
                name: "IX_OngoingSlotElements_SituationId",
                table: "OngoingSlotElements",
                column: "SituationId");

            migrationBuilder.CreateIndex(
                name: "IX_SaveFiles_MetaId",
                table: "SaveFiles",
                column: "MetaId");

            migrationBuilder.CreateIndex(
                name: "IX_SituationItems_SituationId",
                table: "SituationItems",
                column: "SituationId");

            migrationBuilder.CreateIndex(
                name: "IX_SituationOutputNotes_SituationId",
                table: "SituationOutputNotes",
                column: "SituationId");

            migrationBuilder.CreateIndex(
                name: "IX_SituationOutputStackItems_SituationOutputStackId",
                table: "SituationOutputStackItems",
                column: "SituationOutputStackId");

            migrationBuilder.CreateIndex(
                name: "IX_SituationOutputStacks_SituationId",
                table: "SituationOutputStacks",
                column: "SituationId");

            migrationBuilder.CreateIndex(
                name: "IX_Situations_FileId",
                table: "Situations",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_SituationStoredElementItems_SituationStoredElementId",
                table: "SituationStoredElementItems",
                column: "SituationStoredElementId");

            migrationBuilder.CreateIndex(
                name: "IX_SituationStoredElements_SituationId",
                table: "SituationStoredElements",
                column: "SituationId");

            migrationBuilder.CreateIndex(
                name: "IX_StartingSlotElementItems_StartingSlotElementId",
                table: "StartingSlotElementItems",
                column: "StartingSlotElementId");

            migrationBuilder.CreateIndex(
                name: "IX_StartingSlotElements_SituationId",
                table: "StartingSlotElements",
                column: "SituationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeckItems");

            migrationBuilder.DropTable(
                name: "ElementStackItems");

            migrationBuilder.DropTable(
                name: "Levers");

            migrationBuilder.DropTable(
                name: "OngoingSlotElementItems");

            migrationBuilder.DropTable(
                name: "SituationItems");

            migrationBuilder.DropTable(
                name: "SituationOutputNotes");

            migrationBuilder.DropTable(
                name: "SituationOutputStackItems");

            migrationBuilder.DropTable(
                name: "SituationStoredElementItems");

            migrationBuilder.DropTable(
                name: "StartingSlotElementItems");

            migrationBuilder.DropTable(
                name: "Decks");

            migrationBuilder.DropTable(
                name: "ElementStacks");

            migrationBuilder.DropTable(
                name: "CharacterDetails");

            migrationBuilder.DropTable(
                name: "OngoingSlotElements");

            migrationBuilder.DropTable(
                name: "SituationOutputStacks");

            migrationBuilder.DropTable(
                name: "SituationStoredElements");

            migrationBuilder.DropTable(
                name: "StartingSlotElements");

            migrationBuilder.DropTable(
                name: "Situations");

            migrationBuilder.DropTable(
                name: "SaveFiles");

            migrationBuilder.DropTable(
                name: "MetaInfos");
        }
    }
}
