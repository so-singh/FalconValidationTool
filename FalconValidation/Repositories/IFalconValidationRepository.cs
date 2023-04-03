using FalconValidation.Models;

namespace FalconValidation.Repositories
{
    public interface IFalconValidationRepository
    {
        ValidationFields GetFalconValidations(string validationId , string _username );
        List<ValidationFields> GetAllFalconValidations();
        List<ValidationFields> GetResubmitFalconValidations();
        void SaveFalconValidations(ValidationFields validationFields , string _username);
    }
}
