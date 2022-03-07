using System;
using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class CreateTargetVariableValue
    {
        [Required]
        public DateTimeOffset Start { get; set; }
        [Required]
        public DateTimeOffset End { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }
        [Required]
        public double Value { get; set; }
    }
}