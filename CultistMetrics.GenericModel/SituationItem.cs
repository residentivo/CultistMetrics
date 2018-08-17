using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CultistMetrics.GenericModel
{
    public class SituationItem
    {
        [Key]
        public int SituationItemId { get; set; }

        public string Value { get; set; }
        public string Key { get; set; }

        public int SituationId { get; set; }
        public Situation Situation { get; set; }
    }
}
