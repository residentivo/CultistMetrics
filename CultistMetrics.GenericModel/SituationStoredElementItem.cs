﻿using System.ComponentModel.DataAnnotations;

namespace CultistMetrics.GenericModel
{
    public class SituationStoredElementItem
    {
        [Key]
        public int SituationStoredElementItemId { get; set; }

        public string Key { get; set; }
        public string Value { get; set; }

        public int SituationStoredElementId { get; set; }
        public SituationStoredElement SituationStoredElement { get; set; }
    }
}