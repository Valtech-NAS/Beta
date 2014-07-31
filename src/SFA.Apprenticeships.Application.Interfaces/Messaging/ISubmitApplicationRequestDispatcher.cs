namespace SFA.Apprenticeships.Application.Interfaces.Messaging
{
    public interface ISubmitApplicationRequestDispatcher
    {
        void SubmitApplication(SubmitApplicationRequest request);
    }
}