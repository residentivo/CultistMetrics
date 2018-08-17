using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CultistHelper.Model
{
    public class CharacterDetail
    {
        [Key]
        public int CharacterId { get; set; }
        public string Profession { get; set; }
        public string Name { get; set; }
        public string ActiveLegacy { get; set; }

        public List<Lever> Levers { get; set; }

        public int? FileId { get; set; }
        public SaveFile SaveFile { get; set; }
    }
}
