using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CultistHelper.Model
{
    public class OngoingSlotElementItem
    {
        [Key]
        public int OngoingSlotElementItemId { get; set; }

        public string Key { get; set; }
        public string Value { get; set; }

        public int OngoingSlotElementId { get; set; }
        public OngoingSlotElement OngoingSlotElement { get; set; }
    }
}
