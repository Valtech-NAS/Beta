namespace SFA.Apprenticeships.Application.Interfaces.Search
{
    public class ApprenticeshipSearchParameters : SearchParametersBase
    {
        public string Keywords { get; set; }

        public string ApprenticeshipLevel { get; set; }

        public string Sector { get; set; }

        public string[] Frameworks { get; set; }

        public override string ToString()
        {
            var joinedFrameworks = (Frameworks == null || Frameworks.Length == 0)
                ? string.Empty
                : string.Join(",", Frameworks);
            return string.Format("{0}, Keywords:{1}, ApprenticeshipLevel:{2}, Sector:{3}, Frameworks:{4}", base.ToString(), Keywords, ApprenticeshipLevel, Sector, joinedFrameworks);
        }
    }
}