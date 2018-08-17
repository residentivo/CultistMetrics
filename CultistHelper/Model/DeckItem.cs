using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CultistHelper.Model
{
    public class DeckItem
    {
        [Key]
        public int DeckItemId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public bool Eliminated { get; set; }

        public int DeckId { get; set; }
        public Deck Deck { get; set; }
    }
}
