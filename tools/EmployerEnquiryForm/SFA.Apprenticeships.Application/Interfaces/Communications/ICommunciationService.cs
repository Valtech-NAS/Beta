namespace SFA.Apprenticeships.Application.Interfaces.Communications
{
    using Domain.Entities;

    public interface ICommunciationService
    {
        void SubmitEnquiry(EmployerEnquiry message);
    }

}