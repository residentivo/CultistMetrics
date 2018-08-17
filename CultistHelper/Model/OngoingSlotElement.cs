using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CultistHelper.Model
{
    public class OngoingSlotElement
    {
        [Key]
        public int OngoingSlotElementId { get; set; }

        public string OngoingSlotElementIdentification { get; set; }

        public List<OngoingSlotElementItem> OngoingSlotElementItems { get; set; }

        public int SituationId { get; set; }
        public Situation Situation { get; set; }

    }
}
