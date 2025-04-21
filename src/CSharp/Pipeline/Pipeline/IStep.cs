using System.Threading.Tasks;

namespace Pipeline
{
    public interface IStep
    {
        Task<StepResult> Execute(ClientRequest request);
    }
}
