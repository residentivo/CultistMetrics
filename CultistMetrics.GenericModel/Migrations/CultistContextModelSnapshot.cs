﻿// <auto-generated />
using System;
using CultistMetrics.GenericModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CultistMetrics.Migrations
{
    [DbContext(typeof(CultistContext))]
    partial class CultistContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846");

            modelBuilder.Entity("CultistMetrics.Model.CharacterDetail", b =>
                {
                    b.Property<int>("CharacterId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ActiveLegacy");

                    b.Property<int?>("FileId");

                    b.Property<string>("Name");

                    b.Property<string>("Profession");

                    b.HasKey("CharacterId");

                    b.HasIndex("FileId")
                        .IsUnique();

                    b.ToTable("CharacterDetails");
                });

            modelBuilder.Entity("CultistMetrics.Model.Deck", b =>
                {
                    b.Property<int>("DeckId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("FileId");

                    b.Property<string>("Name");

                    b.HasKey("DeckId");

                    b.HasIndex("FileId");

                    b.ToTable("Decks");
                });

            modelBuilder.Entity("CultistMetrics.Model.DeckItem", b =>
                {
                    b.Property<int>("DeckItemId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DeckId");

                    b.Property<bool>("Eliminated");

                    b.Property<string>("Key");

                    b.Property<string>("Value");

                    b.HasKey("DeckItemId");

                    b.HasIndex("DeckId");

                    b.ToTable("DeckItems");
                });

            modelBuilder.Entity("CultistMetrics.Model.ElementStack", b =>
                {
                    b.Property<int>("ElementStackId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ElementStackIdentification");

                    b.Property<int>("FileId");

                    b.HasKey("ElementStackId");

                    b.HasIndex("FileId");

                    b.ToTable("ElementStacks");
                });

            modelBuilder.Entity("CultistMetrics.Model.ElementStackItem", b =>
                {
                    b.Property<int>("ElementStackItemId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ElementStackId");

                    b.Property<string>("Key");

                    b.Property<string>("Value");

                    b.HasKey("ElementStackItemId");

                    b.HasIndex("ElementStackId");

                    b.ToTable("ElementStackItems");
                });

            modelBuilder.Entity("CultistMetrics.Model.Lever", b =>
                {
                    b.Property<int>("LeverId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CharacterDetailCharacterId");

                    b.Property<string>("Key");

                    b.Property<int>("Part");

                    b.Property<string>("Value");

                    b.HasKey("LeverId");

                    b.HasIndex("CharacterDetailCharacterId");

                    b.ToTable("Levers");
                });

            modelBuilder.Entity("CultistMetrics.Model.MetaInfo", b =>
                {
                    b.Property<int>("MetaId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BirdWormSlider");

                    b.Property<string>("Version");

                    b.Property<string>("WeAwaitSTE");

                    b.HasKey("MetaId");

                    b.ToTable("MetaInfos");
                });

            modelBuilder.Entity("CultistMetrics.Model.OngoingSlotElement", b =>
                {
                    b.Property<int>("OngoingSlotElementId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("OngoingSlotElementIdentification");

                    b.Property<int>("SituationId");

                    b.HasKey("OngoingSlotElementId");

                    b.HasIndex("SituationId");

                    b.ToTable("OngoingSlotElements");
                });

            modelBuilder.Entity("CultistMetrics.Model.OngoingSlotElementItem", b =>
                {
                    b.Property<int>("OngoingSlotElementItemId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Key");

                    b.Property<int>("OngoingSlotElementId");

                    b.Property<string>("Value");

                    b.HasKey("OngoingSlotElementItemId");

                    b.HasIndex("OngoingSlotElementId");

                    b.ToTable("OngoingSlotElementItems");
                });

            modelBuilder.Entity("CultistMetrics.Model.SaveFile", b =>
                {
                    b.Property<int>("FileId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FileName");

                    b.Property<DateTime>("FileTime");

                    b.Property<int?>("MetaId");

                    b.Property<int>("Processed");

                    b.HasKey("FileId");

                    b.HasIndex("MetaId");

                    b.ToTable("SaveFiles");
                });

            modelBuilder.Entity("CultistMetrics.Model.Situation", b =>
                {
                    b.Property<int>("SituationId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("FileId");

                    b.Property<string>("SituationIdentification");

                    b.HasKey("SituationId");

                    b.HasIndex("FileId");

                    b.ToTable("Situations");
                });

            modelBuilder.Entity("CultistMetrics.Model.SituationItem", b =>
                {
                    b.Property<int>("SituationItemId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Key");

                    b.Property<int>("SituationId");

                    b.Property<string>("Value");

                    b.HasKey("SituationItemId");

                    b.HasIndex("SituationId");

                    b.ToTable("SituationItems");
                });

            modelBuilder.Entity("CultistMetrics.Model.SituationOutputNote", b =>
                {
                    b.Property<int>("SituationOutputNotesId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Index");

                    b.Property<string>("Key");

                    b.Property<int>("SituationId");

                    b.Property<string>("Value");

                    b.HasKey("SituationOutputNotesId");

                    b.HasIndex("SituationId");

                    b.ToTable("SituationOutputNotes");
                });

            modelBuilder.Entity("CultistMetrics.Model.SituationOutputStack", b =>
                {
                    b.Property<int>("SituationOutputStacksId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("SituationId");

                    b.Property<string>("SituationOutputStacksIdentification");

                    b.HasKey("SituationOutputStacksId");

                    b.HasIndex("SituationId");

                    b.ToTable("SituationOutputStacks");
                });

            modelBuilder.Entity("CultistMetrics.Model.SituationOutputStackItem", b =>
                {
                    b.Property<int>("SituationOutputStackItemId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Key");

                    b.Property<int>("SituationOutputStackId");

                    b.Property<string>("Value");

                    b.HasKey("SituationOutputStackItemId");

                    b.HasIndex("SituationOutputStackId");

                    b.ToTable("SituationOutputStackItems");
                });

            modelBuilder.Entity("CultistMetrics.Model.SituationStoredElement", b =>
                {
                    b.Property<int>("SituationStoredElementId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("SituationId");

                    b.Property<string>("SituationStoredElementIdentification");

                    b.HasKey("SituationStoredElementId");

                    b.HasIndex("SituationId");

                    b.ToTable("SituationStoredElements");
                });

            modelBuilder.Entity("CultistMetrics.Model.SituationStoredElementItem", b =>
                {
                    b.Property<int>("SituationStoredElementItemId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Key");

                    b.Property<int>("SituationStoredElementId");

                    b.Property<string>("Value");

                    b.HasKey("SituationStoredElementItemId");

                    b.HasIndex("SituationStoredElementId");

                    b.ToTable("SituationStoredElementItems");
                });

            modelBuilder.Entity("CultistMetrics.Model.StartingSlotElement", b =>
                {
                    b.Property<int>("StartingSlotElementId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("SituationId");

                    b.Property<string>("StartingSlotElementIdentification");

                    b.HasKey("StartingSlotElementId");

                    b.HasIndex("SituationId");

                    b.ToTable("StartingSlotElements");
                });

            modelBuilder.Entity("CultistMetrics.Model.StartingSlotElementItem", b =>
                {
                    b.Property<int>("StartingSlotElementItemId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Key");

                    b.Property<int>("StartingSlotElementId");

                    b.Property<string>("Value");

                    b.HasKey("StartingSlotElementItemId");

                    b.HasIndex("StartingSlotElementId");

                    b.ToTable("StartingSlotElementItems");
                });

            modelBuilder.Entity("CultistMetrics.Model.CharacterDetail", b =>
                {
                    b.HasOne("CultistMetrics.Model.SaveFile", "SaveFile")
                        .WithOne("CharacterDetail")
                        .HasForeignKey("CultistMetrics.Model.CharacterDetail", "FileId");
                });

            modelBuilder.Entity("CultistMetrics.Model.Deck", b =>
                {
                    b.HasOne("CultistMetrics.Model.SaveFile", "SaveFile")
                        .WithMany("Decks")
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CultistMetrics.Model.DeckItem", b =>
                {
                    b.HasOne("CultistMetrics.Model.Deck", "Deck")
                        .WithMany("DeckItems")
                        .HasForeignKey("DeckId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CultistMetrics.Model.ElementStack", b =>
                {
                    b.HasOne("CultistMetrics.Model.SaveFile", "SaveFile")
                        .WithMany("ElementStacks")
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CultistMetrics.Model.ElementStackItem", b =>
                {
                    b.HasOne("CultistMetrics.Model.ElementStack", "ElementStack")
                        .WithMany()
                        .HasForeignKey("ElementStackId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CultistMetrics.Model.Lever", b =>
                {
                    b.HasOne("CultistMetrics.Model.CharacterDetail", "CharacterDetail")
                        .WithMany("Levers")
                        .HasForeignKey("CharacterDetailCharacterId");
                });

            modelBuilder.Entity("CultistMetrics.Model.OngoingSlotElement", b =>
                {
                    b.HasOne("CultistMetrics.Model.Situation", "Situation")
                        .WithMany("OngoingSlotElements")
                        .HasForeignKey("SituationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CultistMetrics.Model.OngoingSlotElementItem", b =>
                {
                    b.HasOne("CultistMetrics.Model.OngoingSlotElement", "OngoingSlotElement")
                        .WithMany("OngoingSlotElementItems")
                        .HasForeignKey("OngoingSlotElementId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CultistMetrics.Model.SaveFile", b =>
                {
                    b.HasOne("CultistMetrics.Model.MetaInfo", "MetaInfo")
                        .WithMany("SaveFiles")
                        .HasForeignKey("MetaId");
                });

            modelBuilder.Entity("CultistMetrics.Model.Situation", b =>
                {
                    b.HasOne("CultistMetrics.Model.SaveFile", "SaveFile")
                        .WithMany("Situations")
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CultistMetrics.Model.SituationItem", b =>
                {
                    b.HasOne("CultistMetrics.Model.Situation", "Situation")
                        .WithMany("SituationItems")
                        .HasForeignKey("SituationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CultistMetrics.Model.SituationOutputNote", b =>
                {
                    b.HasOne("CultistMetrics.Model.Situation", "Situation")
                        .WithMany("SituationOutputNotes")
                        .HasForeignKey("SituationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CultistMetrics.Model.SituationOutputStack", b =>
                {
                    b.HasOne("CultistMetrics.Model.Situation", "Situation")
                        .WithMany()
                        .HasForeignKey("SituationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CultistMetrics.Model.SituationOutputStackItem", b =>
                {
                    b.HasOne("CultistMetrics.Model.SituationOutputStack", "SituationOutputStack")
                        .WithMany()
                        .HasForeignKey("SituationOutputStackId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CultistMetrics.Model.SituationStoredElement", b =>
                {
                    b.HasOne("CultistMetrics.Model.Situation", "Situation")
                        .WithMany("SituationStoredElements")
                        .HasForeignKey("SituationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CultistMetrics.Model.SituationStoredElementItem", b =>
                {
                    b.HasOne("CultistMetrics.Model.SituationStoredElement", "SituationStoredElement")
                        .WithMany()
                        .HasForeignKey("SituationStoredElementId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CultistMetrics.Model.StartingSlotElement", b =>
                {
                    b.HasOne("CultistMetrics.Model.Situation", "Situation")
                        .WithMany()
                        .HasForeignKey("SituationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CultistMetrics.Model.StartingSlotElementItem", b =>
                {
                    b.HasOne("CultistMetrics.Model.StartingSlotElement", "StartingSlotElement")
                        .WithMany()
                        .HasForeignKey("StartingSlotElementId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}