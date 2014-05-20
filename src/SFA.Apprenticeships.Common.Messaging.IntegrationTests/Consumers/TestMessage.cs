namespace SFA.Apprenticeships.Common.Messaging.IntegrationTests.Consumers
{
    public class TestMessage
    {
        private string _testString;

        public static int Counter = 0;

        public string TestString
        {
            get { return _testString; }
            set { 
                _testString = value;
                Counter++;
            }
        }
    }
}
