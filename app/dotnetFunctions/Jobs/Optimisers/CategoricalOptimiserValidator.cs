using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using SignalBox.Core;
using SignalBox.Core.Optimisers;

namespace SignalBox.Functions
{
    public class CategoricalOptimiserValidator
    {
        public static void Validate(CategoricalOptimiser info)
        {
            if (string.IsNullOrEmpty(info.Id))
            {
                throw new BadRequestException("id must not be empty");
            }
            if (string.IsNullOrEmpty(info.Name))
            {
                throw new BadRequestException("name must not be empty");
            }
            if (info.Items == null)
            {
                throw new BadRequestException("items must not be null");
            }
            if (info.DefaultItem == null)
            {
                throw new BadRequestException("defaultItem must not be null");
            }
            if (info.NItemsToRecommend <= 0)
            {
                throw new BadRequestException("nItemsToRecommend must not be 0");
            }
        }
    }
}