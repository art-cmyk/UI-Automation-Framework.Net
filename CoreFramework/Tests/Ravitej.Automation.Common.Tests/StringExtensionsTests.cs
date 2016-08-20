using System.Collections;
using Ravitej.Automation.Common.Utilities;
using NUnit.Framework;

namespace Ravitej.Automation.Common.Tests
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [Test, TestCaseSource(nameof(FilenamesConatiningDosReservedFilenames))]
        public void FilenamesContainingReservedDosFilenamesShouldBeValidFileNames(string filename)
        {
            var actualResult = filename.SanitiseFilename();
            Assert.That(actualResult, Is.EqualTo(filename));
        }

        [Test, TestCaseSource(nameof(FilenamesEqualToDosReservedFilenames))]
        public void FilenamesEqualToReservedDosFilenamesShouldBeInvalidFileNames(string filename)
        {
            var actualResult = filename.SanitiseFilename();
            Assert.That(actualResult, Is.Not.EqualTo(filename));
        }

        [Test, TestCaseSource(nameof(FilenamesConatiningSpecialCharacters))]
        public void FilenamesContainingSpecialCharactersShouldBeSanitisedCorrectly(string filename, string expected)
        {
            var actualResult = filename.SanitiseFilename();
            Assert.That(actualResult, Is.EqualTo(expected));
        }

        private static IEnumerable FilenamesConatiningDosReservedFilenames()
        {
            yield return new TestCaseData("CONstant.doc");
            yield return new TestCaseData("CONPRNLPT1.png");
            yield return new TestCaseData("iheLPT4person.jpg");
            yield return new TestCaseData("contrived.jpg");
        }

        private static IEnumerable FilenamesEqualToDosReservedFilenames()
        {
            yield return new TestCaseData("CON.jpg");
            yield return new TestCaseData("LPT9.png");
            yield return new TestCaseData("$$$$\\.csv");
            yield return new TestCaseData("AUX.jpg");
        }

        private static IEnumerable FilenamesConatiningSpecialCharacters()
        {
            yield return
                new TestCaseData(
                    "ClickingCreateNewCustomReportButtonOnAnyTabAndClosingContextSelectionPanelShouldAlwaysShowCustomTab(\"All\")",
                    "ClickingCreateNewCustomReportButtonOnAnyTabAndClosingContextSelectionPanelShouldAlwaysShowCustomTab(All)");
            yield return
                new TestCaseData(
                    "WholesaleReportsManagementMenuShouldOnlyBeVisibleForUsersRolesThatAreAllowedToSeeIt(\"SELENBANKING\", \"test2468\", \"998788361\", false)",
                    "WholesaleReportsManagementMenuShouldOnlyBeVisibleForUsersRolesThatAreAllowedToSeeIt(SELENBANKING, test2468, 998788361, false)");
        }
    }
}
