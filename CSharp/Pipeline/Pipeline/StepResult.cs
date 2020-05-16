using System.Collections.Generic;

namespace Pipeline
{
    public class StepResult
    {
        public List<ValidationError> Errors { get; set; } = new List<ValidationError>();

        public RegistrationStep Step { get; set; }

        /// <summary>
        /// true - если нет ошибок валидации
        /// </summary>
        public static implicit operator bool(StepResult result)
            => result.Errors == null || result.Errors.Count == 0;
    }
}
