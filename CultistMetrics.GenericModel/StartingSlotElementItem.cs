using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CultistMetrics.GenericModel
{
    public class StartingSlotElementItem
    {
        [Key]
        public int StartingSlotElementItemId { get; set; }

        public string Key { get; set; }
        public string Value { get; set; }

        public int StartingSlotElementId { get; set; }
        public StartingSlotElement StartingSlotElement { get; set; }
    }
}
