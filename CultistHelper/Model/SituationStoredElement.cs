using System.ComponentModel.DataAnnotations;

namespace CultistHelper.Model
{
    public class SituationStoredElement
    {
        [Key]
        public int SituationStoredElementId { get; set; }

        public string SituationStoredElementIdentification { get; set; }
        
        public int SituationId { get; set; }
        public Situation Situation { get; set; }
    }
}