using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CultistMetrics.GenericModel
{
    public class Situation
    {
        [Key]
        public int SituationId { get; set; }

        public string SituationIdentification { get; set; }

        public List<SituationOutputNote> SituationOutputNotes { get; set; }
        public List<SituationItem> SituationItems { get; set; }
        public List<OngoingSlotElement> OngoingSlotElements { get; set; }
        public List<SituationStoredElement> SituationStoredElements { get; set; }
        public List<StartingSlotElement> StartingSlotElements { get; set; }
        public List<SituationOutputStack> SituationOutputStacks { get; set; }

        public int FileId { get; set; }
        public SaveFile SaveFile { get; set; }
    }
}
