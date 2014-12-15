namespace SFA.Apprenticeships.Application.Interfaces.Vacancies
{
    using Domain.Entities.Vacancies.Traineeships;

    public class TraineeshipSummaryResponse : TraineeshipSummary
    {
        public double Distance { get; set; }

        public double Score { get; set; }
    }
}
