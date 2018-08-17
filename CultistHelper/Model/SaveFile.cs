using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CultistHelper.Model
{
    public enum SaveFileProcessedEnum
    {
        NotProcessed =0,
        Processed,
        Error,
    }

    public class SaveFile
    {
        [Key]
        public int FileId { get; set; }        
        public int Processed { get; set; }
        public string FileName { get; set; }
        public DateTime FileTime { get; set; }

        public List<Situation> Situations { get; set; }
        public List<Deck> Decks { get; set; }
        public List<ElementStack> ElementStacks { get; set; }

        //public int? CharacterId { get; set; }
        public CharacterDetail CharacterDetail { get; set; }
                
        public int? MetaId { get; set; }
        public MetaInfo MetaInfo { get; set; }
    }
}
