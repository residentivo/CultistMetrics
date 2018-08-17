using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CultistMetrics.GenericModel
{
    public class MetaInfo
    {
        [Key]
        public int MetaId { get; set; }        
        public string Version { get; set; }
        public int BirdWormSlider { get; set; }
        public string WeAwaitSTE { get; set; }
        
        public List<SaveFile> SaveFiles { get; set; }
    }
}
