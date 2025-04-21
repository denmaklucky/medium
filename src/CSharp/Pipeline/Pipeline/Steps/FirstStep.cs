using System.Threading.Tasks;

namespace Pipeline.Steps
{
    public class FirstStep : IStep
    {
        public async Task<StepResult> Execute(ClientRequest request)
        {
            var stepResult = new StepResult();
            stepResult.Step = RegistrationStep.Step1;

            if (string.IsNullOrEmpty(request.Email))
                stepResult.Errors.Add(new ValidationError(nameof(request.Email), "Null or empty"));

            if (string.IsNullOrEmpty(request.Passowrd))
                stepResult.Errors.Add(new ValidationError(nameof(request.Passowrd), "Null or empty"));

            //Одна секунда
            await Task.Delay(1000);

            return stepResult;
        }
    }
}
