using System.Linq;
using NUnit.Framework;
using Ravitej.Automation.Sample.PageObjects.Components.Search;
using Ravitej.Automation.Sample.Ui.Tests.Settings;

namespace Ravitej.Automation.Sample.Ui.Tests.Tests
{
    public class SearchTests : MoonpigTestBase<MoonpigPersistableSettings>
    {
        private ISearch _search;

        #region SetUp & TearDown

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            WriteInfoLogEntry(beginTestFixtureSetUp(GetType().Name));
            _search = MoonpigSession.OnPage.MoonpigHome.Menu.Click<ISearch>("Search");
            WriteInfoLogEntry(endTestFixtureSetUp(GetType().Name));
        }

        #endregion

        [Test]
        public void SearchContainerShouldBeDisplayed()
        {
            Assert.That(MoonpigSession.OnPage.MoonpigHome.Search.IsDisplayed(), Is.True, "Expected Search panel to be displayed");
        }

        [Test]
        public void TypingTheLetterGIntoSearchShouldDisplayGrannyAndGrandadInTheSuggestions()
        {
            var contains = _search.EnterSearchTerm("g").GetSearchSuggestions().Contains("granny & grandad");
            Assert.That(contains, Is.True, "Expected suggestion 'granny & grandad' to be displayed");
        }

        [Test]
        public void TypingTheLetterGIntoSearchShouldDisplayElevenSuggestions()
        {
            var contains = _search.EnterSearchTerm("g").GetSearchSuggestions().Count();
            Assert.That(contains, Is.EqualTo(11), "Expected 11 suggestions to be displayed");
        }
    }
}
