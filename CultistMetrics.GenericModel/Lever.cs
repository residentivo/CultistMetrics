using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CultistMetrics.GenericModel
{
    public enum LeverPartEnum
    {
        Error,
        PastLevers,
        FutureLevers,
        Executions
    }

    public class Lever
    {
        [Key]
        public int LeverId { get; set; }

        public string Key { get; set; }
        public string Value { get; set; }

        public int Part { get; set; }

        public virtual CharacterDetail CharacterDetail { get; set; }
    }
}
