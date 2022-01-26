using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using SignalBox.Core;

namespace SignalBox.Functions
{
    public class PayloadItem
    {
        public ICollection<RecommendableItem> Items { get; set; } 
    }

    public class InvokeCategoricalOptimiserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public PayloadItem Payload { get; set; } 

        public void Validate()
        {
            if (Payload == null)
            {
                throw new BadRequestException("payload must not be null");
            }

            if (Payload.Items == null)
            {
                throw new BadRequestException("payload:items must not be null");
            }

        }
    }
}