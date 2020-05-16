using Pipeline.Steps;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pipeline
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var request = new ClientRequest
            {
                Email = "den@lucky.net",
                Passowrd = "1q2w3e4r",
                FirstName = "Denis",
                LastName = "Makarenko"
            };

            var pipeline = new RegistrationPipeline();
            pipeline.AddStep(new FirstStep());
            pipeline.AddStep(new SecondStep());
            pipeline.AddStep(new ThirdStep());

            var result = await pipeline.Run(request);

            Console.WriteLine($"Current step for request: {result.CurrentStep}");

            foreach (var error in result.StepResults.SelectMany(s => s.Errors))
            {
                Console.WriteLine($"Property : {error.Property}; Error : {error.Error}");
            }
        }
    }
}
