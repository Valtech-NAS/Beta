namespace SFA.Apprenticeships.Application.Interfaces.Search
{
    public class ApprenticeshipSearchParameters : SearchParametersBase
    {
        public string Keywords { get; set; }

        public string ApprenticeshipLevel { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, Keywords:{1}, ApprenticeshipLevel:{2}", base.ToString(), Keywords, ApprenticeshipLevel);
        }
    }
}