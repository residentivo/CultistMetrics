using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CultistMetrics.GenericModel
{
    public class SituationStoredElement
    {
        [Key]
        public int SituationStoredElementId { get; set; }

        public string SituationStoredElementIdentification { get; set; }
        
        public List<SituationStoredElementItem> SituationStoredElementItems { get; set; }

        public int SituationId { get; set; }
        public Situation Situation { get; set; }
    }
}