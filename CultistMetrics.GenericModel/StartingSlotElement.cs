using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CultistMetrics.GenericModel
{
    public class StartingSlotElement
    {
        [Key]
        public int StartingSlotElementId { get; set; }

        public string StartingSlotElementIdentification { get; set; }

        public List<StartingSlotElementItem> StartingSlotElementItems { get;set }

        public int SituationId { get; set; }
        public Situation Situation { get; set; }
    }
    
}
