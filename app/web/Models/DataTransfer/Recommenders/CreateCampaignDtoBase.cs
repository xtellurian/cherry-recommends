using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
#nullable enable
    public abstract class CreateCampaignDtoBase : CommonDtoBase
    {
        [Required]
        public override string? Name { get; set; }
        public long? CloneFromId { get; set; }
        [Obsolete]
        public bool? ThrowOnBadInput { get; set; }
        [Obsolete]
        public bool? RequireConsumptionEvent { get; set; }
        public CampaignSettingsDto? Settings { get; set; }
        public IEnumerable<CreateOrUpdateCampaignArgument>? Arguments { get; set; }
        public string? TargetMetricId { get; set; }
        public IEnumerable<long>? SegmentIds { get; set; }
        public IEnumerable<long>? ChannelIds { get; set; }
    }
}