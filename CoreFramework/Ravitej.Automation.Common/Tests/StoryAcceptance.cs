using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Ravitej.Automation.Common.Config;
using Ravitej.Automation.Common.Config.SuiteSettings;
using Ravitej.Automation.Common.PageObjects.Session;
using Ravitej.Automation.Common.Utilities.Imaging;
using OpenQA.Selenium;

namespace Ravitej.Automation.Common.Tests
{
    public class StoryAcceptance : IDisposable
    {
        private readonly string _storyId;
        private readonly string _storyName;
        private readonly int _acceptanceId;
        private readonly Dictionary<string, string> _acceptanceCriteria = new Dictionary<string, string>();
        private readonly Dictionary<string, bool> _acceptanceCriteriaCovered = new Dictionary<string, bool>();
        private readonly List<string> _screenshots = new List<string>();
        private readonly ISuiteSettings _testSuiteSettings;
        private readonly ISession _session;
        private bool _testStarted;

        public enum StoryScreenshotType
        {
            Basic,
            AcceptanceCriteria,
            DialogMessage,
            Initial,
            Completion
        }

        public StoryAcceptance(ISession session, ISuiteSettings testSuiteSettings, string storyId, string storyName)
            : this(session, testSuiteSettings, storyId, storyName, 0)
        {
        }

        public StoryAcceptance(ISession session, ISuiteSettings testSuiteSettings, string storyId, string storyName, int acceptanceId)
        {
            _storyId = storyId;
            _storyName = storyName;
            _acceptanceId = acceptanceId;
            _testSuiteSettings = testSuiteSettings;
            _session = session;
        }

        private void TakeScreenshot(string overlayText, StoryScreenshotType screenshotType)
        {
            string rootPath = ExecutionSettings.OutputPath(
                $@"StoryAcceptance\{_storyId}\{DateTime.Now.ToString("yyyyMMdd_HHmmss")}", true);

            string fileName = GetScreenshotFileName();

            string fullSavePath = Path.Combine(rootPath, fileName);

            Screenshot screenshot = _session.DriverSession.Driver.TakeScreenshot();
            if (screenshot.PersistScreenshot(fullSavePath))
            {
                if (!string.IsNullOrWhiteSpace(overlayText))
                {
                    GenerateOverlayBlock(fullSavePath, overlayText, screenshotType);
                    //Utility.Imaging.ImagingUtilities.OverlayTextOntoImage(fullSavePath, overlayText, true);
                }
            }

            _screenshots.Add(fileName);
        }

        private void GenerateOverlayBlock(string sourceFileName, string overlayText, StoryScreenshotType screenshotType)
        {
            var bubble = new SpeechBubble(overlayText);

            switch (screenshotType)
            {
                case StoryScreenshotType.Initial:
                {
                    bubble.Shape = BalloonShape.Rectangle;
                    bubble.FillColor = Color.Maroon;
                    bubble.TextColor = Color.White;
                    bubble.BubbleWidth = 800;
                    bubble.Height = 400;
                    break;
                }
                case StoryScreenshotType.Basic:
                {
                    bubble.Shape = BalloonShape.Ellipse;
                    bubble.FillColor = Color.White;
                    break;
                }
                case StoryScreenshotType.Completion:
                {
                    bubble.Shape = BalloonShape.Rectangle;
                    bubble.FillColor = Color.Chartreuse;
                    bubble.BubbleWidth = 800;
                    bubble.Height = 400;
                    break;
                }
                case StoryScreenshotType.DialogMessage:
                {
                    bubble.Shape = BalloonShape.Ellipse;
                    bubble.FillColor = Color.PowderBlue;
                    break;
                }
                case StoryScreenshotType.AcceptanceCriteria:
                {
                    bubble.Shape = BalloonShape.Rectangle;
                    bubble.FillColor = Color.Thistle;
                    break;
                }
            }

            bubble.RenderText(sourceFileName, true);
        }

        private string GetScreenshotFileName()
        {
            // using some convention, generate a file name for the image
            string retVal = $"{_storyId}_{_acceptanceId}_{_screenshots.Count}";
            return retVal;
        }

        private string InitialScreenshotComment()
        {
            var sBuild = new StringBuilder();

            sBuild.AppendFormat("Acceptance Script for Story:{0} ({1}){2}", _storyId, _storyName, Environment.NewLine);
            sBuild.AppendFormat("Executed commenced on: {0}{1}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"), Environment.NewLine);
            sBuild.AppendLine("Acceptance Criteria:");

            foreach (var item in _acceptanceCriteria)
            {
                sBuild.AppendLine($"{item.Value}{Environment.NewLine}");    
            }

            return sBuild.ToString();
        }

        private string FinishingScreenshotComment()
        {
            var sBuild = new StringBuilder();

            sBuild.AppendFormat("Acceptance Summary for Story:{0} ({1}){2}", _storyId, _storyName, Environment.NewLine);
            sBuild.AppendFormat("Execution completed on: {0}{1}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"), Environment.NewLine);
            sBuild.AppendLine("Acceptance Criteria:");

            foreach (var item in _acceptanceCriteria)
            {
                sBuild.AppendLine($"{item.Value}{Environment.NewLine}");
            }

            sBuild.AppendLine("Summary: All acceptance criteria have been met");

            return sBuild.ToString();
        }

        /// <summary>
        /// Take a screenshot giving some context as to the meaning of the next images
        /// </summary>
        private void TakeInitialScreenshot()
        {
            TakeScreenshot(InitialScreenshotComment(), StoryScreenshotType.Initial);
        }

        /// <summary>
        /// Starts off the acceptance test process
        /// </summary>
        public void StartAcceptanceTest()
        {
            _testStarted = true;
            TakeInitialScreenshot();
        }

        public void FinishAcceptanceTest()
        {
            if (_acceptanceCriteria.Count != _acceptanceCriteriaCovered.Count)
            {
                throw new Exception("Unable to accept the story as not all acceptance criteria have been covered.  Please check the test steps to validate that all items have been covered");
            }

            TakeFinalAcceptanceScreenshot();
        }

        /// <summary>
        /// generate an image here listing each of the acceptance criteria together with an indication as to whether each has been covered by this set of acceptance tests
        /// </summary>
        private void TakeFinalAcceptanceScreenshot()
        {
            TakeScreenshot(FinishingScreenshotComment(), StoryScreenshotType.Completion);
        }

        public void AddAcceptanceCriteria(string key, string description)
        {
            if (_testStarted)
            {
                throw new Exception("Unable to add an acceptance criteria after the test sequence has started.  Please ensure these are added prior to starting the test sequence.");
            }

            _acceptanceCriteria.Add(key, description);
        }

        public void AcceptanceScreenshot(string acceptanceCriteriaKey, string comment)
        {
            if (_acceptanceCriteriaCovered.ContainsKey(acceptanceCriteriaKey))
            {
                throw new Exception($"Acceptance criteria item {acceptanceCriteriaKey} has already been covered");
            }

            if (!_acceptanceCriteria.ContainsKey(acceptanceCriteriaKey))
            {
                throw new Exception(
                    $"Acceptance criteria item {acceptanceCriteriaKey} is not in the recognised acceptance criteria list");
            }

            _acceptanceCriteriaCovered.Add(acceptanceCriteriaKey, true);

            TakeScreenshot(string.Format("Key: {0}{2}Comment:{1}", acceptanceCriteriaKey, comment, Environment.NewLine), StoryScreenshotType.AcceptanceCriteria);
        }

        public void IncidentalScreenshot(string comment)
        {
            TakeScreenshot($"Comment:{comment}{Environment.NewLine}", StoryScreenshotType.Basic);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
