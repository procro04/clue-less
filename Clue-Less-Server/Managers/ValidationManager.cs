using Clue_Less_Server.Managers.Interfaces;

namespace Clue_Less_Server.Managers
{
    public class ValidationManager : IValidationManager
    {
        private static readonly Lazy<ValidationManager> lazy = new Lazy<ValidationManager>(() => new ValidationManager());
        public static ValidationManager Instance { get { return lazy.Value; } }

        public ValidationManager() { }

        public bool ValidatePlayerAction(bool isValidPlayerAction)
        {
            return isValidPlayerAction;
        }
    }
}
