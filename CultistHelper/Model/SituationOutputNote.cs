using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CultistHelper.Model
{
    public class SituationOutputNote
    {
        [Key]
        public int SituationOutputNotesId { get; set; }
        
        public string Index { get; set; }
        public string Value { get; set; }
        public string Key { get; set; }

        public int SituationId { get; set; }
        public Situation Situation { get; set; }
    }
}
