using System.ComponentModel.DataAnnotations;

namespace CultistMetrics.GenericModel
{
    public class SituationOutputStackItem
    {
        [Key]
        public int SituationOutputStackItemId { get; set; }

        public string Key { get; set; }
        public string Value { get; set; }

        public int SituationOutputStackId { get; set; }
        public SituationOutputStack SituationOutputStack { get; set; }
    }
}
