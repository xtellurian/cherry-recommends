using SignalBox.Core;

namespace SignalBox.Web.Dto
{
    public class CreateExperimentResultDto : DtoBase
    {
        public CreateExperimentResultDto(Experiment experiment)
        {
            Id = experiment.Id;
        }

        public long Id { get; set; }
    }
}