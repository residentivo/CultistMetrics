using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CultistMetrics.GenericModel
{
    public class ElementStackItem
    {
        [Key]
        public int ElementStackItemId { get; set; }

        public string Key { get; set; }
        public string Value { get; set; }
        
        public int ElementStackId { get; set; }
        public ElementStack ElementStack { get;set;}
    }
}
