using System.Collections.Generic;

namespace Pipeline
{
    public class PipelineResult
    {
        public static bool CanChangeStep = true;

        public RegistrationStep CurrentStep { get; set; }

        public List<StepResult> StepResults { get; set; } = new List<StepResult>();
    }
}
