using System.Collections.Generic;

namespace Pipeline
{
    public class PipelineResult
    {
        public bool CanChangeStep { get; set; } = true;

        public RegistrationStep CurrentStep { get; set; }

        public List<StepResult> StepResults { get; set; } = new List<StepResult>();
    }
}
