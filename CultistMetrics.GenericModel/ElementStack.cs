using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CultistMetrics.GenericModel
{
    public class ElementStack
    {
        public ElementStack()
        {
            ElementStackItems = new List<ElementStackItem>();
        }

        [Key]
        public int ElementStackId { get; set; }

        public string ElementStackIdentification { get; set; }
        
        public List<ElementStackItem> ElementStackItems { get; set; }

        public int FileId { get; set; }
        public SaveFile SaveFile { get; set; }
    }
}
