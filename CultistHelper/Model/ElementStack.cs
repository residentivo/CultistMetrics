using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CultistHelper.Model
{
    public class ElementStack
    {
        [Key]
        public int ElementStackId { get; set; }

        public string ElementStackIdentification { get; set; }
        
        public int FileId { get; set; }
        public SaveFile SaveFile { get; set; }
    }
}
