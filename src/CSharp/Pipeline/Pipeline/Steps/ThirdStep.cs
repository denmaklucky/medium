using System.Threading.Tasks;

namespace Pipeline.Steps
{
    public class ThirdStep : IStep
    {
        public async Task<StepResult> Execute(ClientRequest request)
        {
            var stepResult = new StepResult();
            stepResult.Step = RegistrationStep.Step3;

            if (string.IsNullOrEmpty(request.Phone))
                stepResult.Errors.Add(new ValidationError(nameof(request.Phone), "Null or empty"));

            //Одна секунда
            await Task.Delay(1000);

            return stepResult;
        }
    }
}
