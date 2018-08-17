using System.ComponentModel.DataAnnotations;

namespace CultistHelper.Model
{
    public class SituationOutputStack
    {
        [Key]
        public int SituationOutputStacksId { get; set; }

        public string SituationOutputStacksIdentification { get; set; }

        public int SituationId { get; set; }
        public Situation Situation { get; set; }
    }
    
}
