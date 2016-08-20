namespace Ravitej.Automation.Common.Tests.FakeImplementations
{
    public class FakeTestBase : TestBase<FakeSuiteSettings>
    {
        public FakeTestBase()
        {
            TestBaseNamespace = "Ravitej.Automation";
            TestResultsBaseFolder = "UnitTests";
        }

        //public string GetTestResultsPathNoCreate
        //{
        //    get
        //    {
        //        return GetTestResultsPath(false);
        //    }
        //}

        public string GetTestResultsPathWithTestNameNoCreate => GetTestResultsPathWithTestName(false);
    }
}
