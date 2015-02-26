namespace SFA.Apprenticeships.Web.Employer.ViewModels
{
    using System.Collections.Generic;

    public class ReferenceDataListViewModel : ViewModelBase
    {
          public ReferenceDataListViewModel()
            : this(new ReferenceDataViewModel[] { })
        {
        }

        public ReferenceDataListViewModel(string message) : base(message) { }

        public ReferenceDataListViewModel(IEnumerable<ReferenceDataViewModel> referenceData)
        {
            ReferenceData = referenceData;
        }

        public IEnumerable<ReferenceDataViewModel> ReferenceData { get; private set; }
    }
}
