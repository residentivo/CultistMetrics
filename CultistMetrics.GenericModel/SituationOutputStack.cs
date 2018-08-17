using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CultistMetrics.GenericModel
{
    public class SituationOutputStack
    {
        [Key]
        public int SituationOutputStacksId { get; set; }

        public string SituationOutputStacksIdentification { get; set; }
        
        public List<SituationOutputStackItem> SituationOutputStackItems { get; set; }

        public int SituationId { get; set; }
        public Situation Situation { get; set; }
    }
}
