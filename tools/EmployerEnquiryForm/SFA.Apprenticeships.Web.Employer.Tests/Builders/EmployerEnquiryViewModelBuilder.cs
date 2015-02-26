namespace SFA.Apprenticeships.Web.Employer.Tests.Builders
{
    using ViewModels;

    public class EmployerEnquiryViewModelBuilder
    {
        private string _firstname = "First";
        private string _lastname = "Last";
        private string _phoneNumber;
        private string _mobileNumber;
        private string _position;
        private string _companyName;
        private string _title;
        private string _enquirySource;
        private string _email;
        private string _employeeCount;
        private string _enquiryDescription;
        private string _prevExp;

        public EmployerEnquiryViewModelBuilder EnquiryDescription(string enquiryDescription)
        {
            _enquiryDescription = enquiryDescription;
            return this;
        }
        public EmployerEnquiryViewModelBuilder PrevExp(string prevExp)
        {
            _prevExp = prevExp;
            return this;
        }
        public EmployerEnquiryViewModelBuilder MobileNumber(string mobile)
        {
            _mobileNumber = mobile;
            return this;
        }
        public EmployerEnquiryViewModelBuilder Position(string postion)
        {
            _position = postion;
            return this;
        }
        public EmployerEnquiryViewModelBuilder Companyname(string companyname)
        {
            _companyName = companyname;
            return this;
        }
        public EmployerEnquiryViewModelBuilder Title(string title)
        {
            _title = title;
            return this;
        }
        public EmployerEnquiryViewModelBuilder EnquirySource(string enquirySource)
        {
            _enquirySource = enquirySource;
            return this;
        }
        public EmployerEnquiryViewModelBuilder Email(string email)
        {
            _email = email;
            return this;
        }
        public EmployerEnquiryViewModelBuilder EmployeeCount(string employeeCount)
        {
            _employeeCount = employeeCount;
            return this;
        }


        public EmployerEnquiryViewModelBuilder Firstname(string firstname)
        {
            _firstname = firstname;
            return this;
        }

        public EmployerEnquiryViewModelBuilder Lastname(string lastname)
        {
            _lastname = lastname;
            return this;
        }

        public EmployerEnquiryViewModelBuilder PhoneNumber(string phoneNumber)
        {
            _phoneNumber = phoneNumber;
            return this;
        }


        public EmployerEnquiryViewModel Build()
        {
            var model = new EmployerEnquiryViewModel()
            {
                Firstname = _firstname,
                Lastname = _lastname,
                WorkPhoneNumber = _phoneNumber,
                MobileNumber = _mobileNumber,
                Position = _position,
                Companyname = _companyName,
                Title = _title,
                EnquirySource = _enquirySource,
                Email = _email,
                EmployeesCount = _employeeCount,
                EnquiryDescription = _enquiryDescription,
                PreviousExperienceType = _prevExp
            };

            return model;
        }
    }
}