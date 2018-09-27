using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CultistMetrics.GenericModel
{
    public class OngoingSlotElement
    {
        public OngoingSlotElement()
        {
            OngoingSlotElementItems = new List<OngoingSlotElementItem>();
        }

        [Key]
        public int OngoingSlotElementId { get; set; }

        public string OngoingSlotElementIdentification { get; set; }

        public List<OngoingSlotElementItem> OngoingSlotElementItems { get; set; }

        public int SituationId { get; set; }
        public Situation Situation { get; set; }

    }
}
