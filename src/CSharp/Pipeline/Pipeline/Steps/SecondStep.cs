using System.Threading.Tasks;

namespace Pipeline.Steps
{
    public class SecondStep : IStep
    {
        public async Task<StepResult> Execute(ClientRequest request)
        {
            var stepResult = new StepResult();
            stepResult.Step = RegistrationStep.Step2;

            if (string.IsNullOrEmpty(request.FirstName))
                stepResult.Errors.Add(new ValidationError(nameof(request.FirstName), "Null or empty"));

            if (string.IsNullOrEmpty(request.LastName))
                stepResult.Errors.Add(new ValidationError(nameof(request.LastName), "Null or empty"));

            //Одна секунда
            await Task.Delay(1000);

            return stepResult;
        }
    }
}
