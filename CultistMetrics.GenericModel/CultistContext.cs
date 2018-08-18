using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CultistMetrics.GenericModel
{
    //Commands for upadte database
    //PM> remove-migration
    //PM> add-migration recriate_situation    
    //PM> update-database
    public class CultistContext : DbContext
    {
        private IConfiguration configuration;

        public CultistContext() { }
        public CultistContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public DbSet<SaveFile> SaveFiles { get; set; }

        public DbSet<MetaInfo> MetaInfos { get; set; }

        public DbSet<CharacterDetail> CharacterDetails { get; set; }
        public DbSet<Lever> Levers { get; set; }

        public DbSet<Deck> Decks { get; set; }
        public DbSet<DeckItem> DeckItems { get; set; }

        public DbSet<ElementStack> ElementStacks { get; set; }
        public DbSet<ElementStackItem> ElementStackItems { get; set; }

        public DbSet<Situation> Situations { get; set; }

        public DbSet<SituationItem> SituationItems { get; set; }
        public DbSet<SituationOutputNote> SituationOutputNotes { get; set; }

        public DbSet<OngoingSlotElement> OngoingSlotElements { get; set; }
        public DbSet<OngoingSlotElementItem> OngoingSlotElementItems { get; set; }

        public DbSet<SituationStoredElement> SituationStoredElements { get; set; }
        public DbSet<SituationStoredElementItem> SituationStoredElementItems { get; set; }

        public DbSet<StartingSlotElement> StartingSlotElements { get; set; }
        public DbSet<StartingSlotElementItem> StartingSlotElementItems { get; set; }

        public DbSet<SituationOutputStack> SituationOutputStacks { get; set; }
        public DbSet<SituationOutputStackItem> SituationOutputStackItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (configuration == null && configuration.GetSection("ConnectionStrings") != null)
                optionsBuilder.UseSqlite("Data Source=Cultist.db");
            else
                optionsBuilder.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
