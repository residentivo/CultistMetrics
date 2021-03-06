﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CultistMetrics.GenericModel
{
    public class Deck
    {
        public Deck()
        {
            DeckItems = new List<DeckItem>();
        }
        [Key]
        public int DeckId { get; set; }
        public string Name { get; set; }
        
        public List<DeckItem> DeckItems { get; set; }

        public int FileId { get; set; }
        public virtual SaveFile SaveFile { get; set; }
    }
}
