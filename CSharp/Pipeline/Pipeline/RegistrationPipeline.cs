using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pipeline
{
    public class RegistrationPipeline
    {
        public List<IStep> _steps;

        public RegistrationPipeline()
            => _steps = new List<IStep>();

        public void AddStep(IStep step)
            => _steps.Add(step);

        public async Task<PipelineResult> Run(ClientRequest request)
        {
            var pipelineResult = new PipelineResult();

            foreach (var step in _steps)
            {
                var stepResult = await step.Execute(request);

                pipelineResult.StepResults.Add(stepResult);

                pipelineResult.CanChangeStep = pipelineResult.CanChangeStep
                                             ? stepResult
                                             : false;

                if (pipelineResult.CanChangeStep)
                    pipelineResult.CurrentStep = stepResult.Step;
            }

            return pipelineResult;
        }
    }
}
