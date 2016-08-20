using System;
using System.Collections.Generic;
using NUnit.Framework;
using Ravitej.Automation.Common.Tests.FakeImplementations;

namespace Ravitej.Automation.Common.Tests
{
    [TestFixture]
    public class TestBaseUnitTests
    {
        [Test]
        public void TestResultsPath_Contains_MethodName()
        {
            var testBase = new FakeTestBase();
            var resultsPath = testBase.GetTestResultsPathWithTestNameNoCreate;
            Assert.IsTrue(resultsPath.Contains("TestResultsPath_Contains_MethodName"));
        }

        [Test]//, Ignore("Something's wrong")]
        [TestCaseSource(nameof(SourceTestCases))]
        public void TestResultsPath_Contains_MethodNameWithTestCaseSourceAndInvalidCharactersRemoved(object argument1, object argument2)
        {
            var testBase = new FakeTestBase();
            var actualResultsPath = testBase.GetTestResultsPathWithTestNameNoCreate;
            //Nunit uses format "MM/dd/yyyy HH:mm:ss" for date times in test cases even though the test code treats it in local format.
            argument1 = argument1 is DateTime ? ((DateTime)argument1).ToString("MM/dd/yyyy HH:mm:ss") : argument1;
            argument2 = argument2 is DateTime ? ((DateTime)argument2).ToString("MM/dd/yyyy HH:mm:ss") : argument2;
            var expectedResultsPath =
                $"TestResultsPath_Contains_MethodNameWithTestCaseSourceAndInvalidCharactersRemoved({argument1},{argument2})";
            Assert.IsTrue(actualResultsPath.Contains(expectedResultsPath));
        }

        private static IEnumerable<TestCaseData> SourceTestCases
        {
            get
            {
                yield return new TestCaseData("value1", "value2");//.Ignore("Something's wrong");
                yield return new TestCaseData(1, 2);//.Ignore("Something's wrong");
                //yield return new TestCaseData(1.1, 2.2); -- this case currently fails. need to handle this case.
                yield return new TestCaseData(new DateTime(2015, 12, 15, 15, 15, 15, 150).ToUniversalTime(), 
                    new DateTime(2005, 05, 05, 06, 05, 05, 005).ToUniversalTime());//.Ignore("Something's wrong");
            }
        }

        [Test]
        public void TestResultsPath_Contains_NameSpace()
        {
            var testBase = new FakeTestBase();
            var resultsPath = testBase.GetTestResultsPathWithTestNameNoCreate;
            Assert.IsTrue(resultsPath.Contains("\\Common\\Tests\\"));
        }

        [Test]
        public void TestResultsPath_Contains_ClassName()
        {
            var testBase = new FakeTestBase();
            var resultsPath = testBase.GetTestResultsPathWithTestNameNoCreate;
            Assert.IsTrue(resultsPath.Contains("\\TestBaseUnitTests\\"));
        }

        [Test]//, Ignore("Something's wrong")]
        public void TestResultsPath_Contains_NameSpaceClassNameAndMethodName()
        {
            var testBase = new FakeTestBase();
            var resultsPath = testBase.GetTestResultsPathWithTestNameNoCreate;
            Assert.IsTrue(resultsPath.Contains("\\Common\\Tests\\TestBaseUnitTests\\TestResultsPath_Contains_NameSpaceClassNameAndMethodName"));
        }
    }
}
