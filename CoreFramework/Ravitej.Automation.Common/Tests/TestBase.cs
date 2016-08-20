using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using Ravitej.Automation.Common.Config;
using Ravitej.Automation.Common.Config.SuiteSettings;
using Ravitej.Automation.Common.DataProviders;
using Ravitej.Automation.Common.Drivers;
using Ravitej.Automation.Common.Drivers.CapabilityProviders;
using Ravitej.Automation.Common.Helpers;
using Ravitej.Automation.Common.PageObjects.Session;
using Ravitej.Automation.Common.Utilities;
using Ravitej.Automation.Common.Utilities.Persistance;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using Platform = Ravitej.Automation.Common.Config.DriverSession.Platform;
using NUnit.Framework.Interfaces;
using Ravitej.Automation.Common.Config.DriverSession;
using Ravitej.Automation.Common.DriverFactories;

namespace Ravitej.Automation.Common.Tests
{
    /// <summary>
    /// Base class for all tests to derive from.
    /// </summary>
    /// <typeparam name="TSuiteSettingsType"></typeparam>
    public abstract class TestBase<TSuiteSettingsType> where TSuiteSettingsType : ISuiteSettings, new()
    {
        private const string TestSettingsXPathQuery = "//testSettings/testSetting[@id='{0}']";

        protected string TestBaseNamespace = string.Empty;

        protected string TestResultsBaseFolder = string.Empty;

        public TSuiteSettingsType TestSuiteSettings;

        /// <summary>
        /// Write an Information type log entry
        /// </summary>
        /// <param name="message">Message to write to the log.</param>
        /// <param name="args">Optional parameters to substitute in the <paramref name="message"/>.</param>
        protected virtual void WriteInfoLogEntry(string message, params object[] args)
        {
            Logging.Logging.WriteInfoLogEntry(GetType().ToString(), message, args);
        }

        /// <summary>
        /// Write a Verbose type log entry
        /// </summary>
        /// <param name="message">Message to write to the log.</param>
        /// <param name="args">Optional parameters to substitute in the <paramref name="message"/>.</param>
        protected virtual void WriteVerboseLogEntry(string message, params object[] args)
        {
            Logging.Logging.WriteVerboseLogEntry(GetType().ToString(), message, args);
        }

        /// <summary>
        /// Write a Warning type log entry.
        /// </summary>
        /// <param name="message">Message to write to the log.</param>
        /// <param name="args">Optional parameters to substitute in the <paramref name="message"/>.</param>
        protected virtual void WriteWarningLogEntry(string message, params object[] args)
        {
            Logging.Logging.WriteWarningLogEntry(GetType().ToString(), message, args);
        }

        /// <summary>
        /// Write an Error type log entry.
        /// </summary>
        /// <param name="message">Message to write to the log.</param>
        /// <param name="args">Optional parameters to substitute in the <paramref name="message"/>.</param>
        protected virtual void WriteErrorLogEntry(string message, params object[] args)
        {
            Logging.Logging.WriteErrorLogEntry(GetType().ToString(), message, args);
        }

        /// <summary>
        /// Write a Critical type log entry.
        /// </summary>
        /// <param name="message">Message to write to the log.</param>
        /// <param name="args">Optional parameters to substitute in the <paramref name="message"/>.</param>
        protected virtual void WriteCriticalLogEntry(string message, params object[] args)
        {
            Logging.Logging.WriteCriticalLogEntry(GetType().ToString(), message, args);
        }

        protected Func<string, string> beginTestFixtureSetUp = s => $"Begin OneTimeSetUp for: {s}";
        protected Func<string, string> endTestFixtureSetUp = s => $"End OneTimeSetUp for: {s}";
        protected Func<string, string> beginTestFixtureTearDown = s => $"Begin OneTimeTearDown for: {s}";
        private Func<string, string> endTestFixtureTearDown = s => $"End OneTimeTearDown for: {s}";
        protected Func<string, string> beginTestSetUp = s => $"Begin SetUp for: {s}";
        protected Func<string, string> endTestSetUp = s => $"End SetUp for: {s}";
        protected Func<string, string> beginTestTearDown = s => $"Begin TearDown for: {s}";
        protected Func<string, string> endTestTearDown = s => $"End TearDown for: {s}";

        protected string GetTestResultsPath(bool createFolder)
        {
            string retString = CurrentTestTypeName.Replace(TestBaseNamespace, "").Replace(".", @"\").SanitisePath();
            /* need to sanitise the path before passing in to OutputPath method as it combines the paths 
             * and throws an exception if illegal characters exist when combining paths.
             */
            retString = ExecutionSettings.OutputPath(
                $@"{TestResultsBaseFolder}\{retString}\{DateTime.Now.ToString("yyyyMMdd")}\{DateTime.Now.ToString(
                    "HHmmss")}", createFolder);

            return retString.SanitisePath();
        }

        protected string GetTestResultsPathWithTestName(bool createFolder)
        {
            string retString = CurrentTestMethodName.Replace(TestBaseNamespace, "").Replace(".", @"\").SanitisePath();
            /* need to sanitise the path before passing in to OutputPath method as it combines the paths 
             * and throws an exception if illegal characters exist when combining paths.
             */
            retString = ExecutionSettings.OutputPath(
                $@"{TestResultsBaseFolder}\{retString}\{DateTime.Now.ToString("yyyyMMdd")}\{DateTime.Now.ToString(
                    "HHmmss")}", createFolder);

            return retString.SanitisePath();
        }

        private static string CurrentTestMethodName
        {
            get
            {
                // if the test framework is ready with the name...
                if (TestContext.CurrentContext != null && TestContext.CurrentContext.Test != null)
                {
                    // use it
                    return TestContext.CurrentContext.Test.FullName;
                }

                return _GetEntryMethodName();
            }
        }

        private static string _GetEntryMethodName()
        {
            var reflected = MethodBase.GetCurrentMethod().DeclaringType;

            string entryMethodName = string.Empty;

            var stackTrace = new StackTrace(true);
            var allFrames = stackTrace.GetFrames();

            if (allFrames != null)
            {
                foreach (var r in allFrames)
                {
                    var methName = r.GetMethod();

                    if (methName.ReflectedType.Namespace == reflected.Namespace)
                    {
                        var xNameSpace = methName.ReflectedType.FullName;
                        var xMethodName = methName.ToString();

                        // signature is always:
                        // <return type><space><methodName><open bracket><close bracket>
                        xMethodName = xMethodName.Substring(xMethodName.IndexOf(" ", StringComparison.Ordinal) + 1)
                            .Replace("()", "");

                        string xMeth = $"{xNameSpace}.{xMethodName}";

                        // the last item in the list is always the top-most entty point.
                        entryMethodName = xMeth;
                    }
                }
            }
            return entryMethodName;
        }

        private string CurrentTestTypeName
        {
            get
            {
                var callingType = GetType();
                return callingType.FullName;
            }
        }

        /// <summary>
        /// Initialises new TestBase with default values
        /// </summary>
        protected TestBase()
        {
            SiteLaunchTarget = 0;
        }

        /// <summary>
        /// Initialises new TestBase with the given launch page
        /// </summary>
        /// <param name="launchTarget"></param>
        protected TestBase(int launchTarget)
        {
            SiteLaunchTarget = launchTarget;
        }

        /// <summary>
        /// Initialises new TestBase with the given launch page and user
        /// </summary>
        /// <param name="launchTarget"></param>
        /// <param name="basicAuthusername"></param>
        protected TestBase(int launchTarget, string basicAuthusername)
        {
            SiteLaunchTarget = launchTarget;
            BasicAuthUsername = basicAuthusername;
        }

        protected string BasicAuthUsername { private get; set; }

        protected int SiteLaunchTarget { private get; set; }

        public ISession Session { protected get; set; }

        [OneTimeSetUp]
        public void TestBaseFixtureSetUp()
        {
            ConfigureLogging();
            LoadSuiteSettings();

            CreateSession();
            ExecutionSettings.CurrentTestResultsPath = GetTestResultsPathWithTestName(false);
        }

        [SetUp]
        protected void TestBaseSetUp()
        {
            ExecutionSettings.CurrentTestResultsPath = GetTestResultsPathWithTestName(false);
        }

        [TearDown]
        protected void TestBaseTearDown()
        {
            string alertText = null;

            var currentTestStatus = TestContext.CurrentContext.Result.Outcome;
            if (!currentTestStatus.Equals(TestStatus.Failed))
            {
                return;
            }

            var testName = TestContext.CurrentContext.Test.Name;

            if (IsAlertDisplayed(out alertText))
            {
                AcceptAlert();
            }

            var filename = _GetFailedFilename();

            if (string.IsNullOrEmpty(alertText) == false)
            {
                LogX.Error.Category(testName).Write("Test was stopped due to an unhandled popup with following message - {0}", alertText);
                Console.Error.WriteLine("Test {0} was stopped due to an unhandled popup with following message - {1}", testName, alertText);
                TakeScreenshot(filename, alertText);
            }
            else
            {
                TakeScreenshot(filename);
            }

            #region Commented out code

            //string sToday = DateTime.Today.ToString("yyyy-MMM-dd");
            //string sScreenshotsDirectory = string.Format("{0}\\{1}", ExecutionSettings.ScreenshotsPath, sToday);
            //string sNow = DateTime.Now.ToFileTime().ToString(CultureInfo.InvariantCulture);
            //if (Directory.Exists(sScreenshotsDirectory) == false)
            //{
            //    Directory.CreateDirectory(sScreenshotsDirectory);
            //}
            //string sScreenshotFileName = string.Format("{0}\\{1}_{2}.jpg", sScreenshotsDirectory, sTestName, sNow);
            //Screenshot oScreenshot = RunSession.DriverSession.Driver.GetScreenshot();
            //byte[] screenshotAsByteArray = oScreenshot.AsByteArray;

            //using (var oFile = new FileStream(sScreenshotFileName, FileMode.CreateNew))
            //{
            //    oFile.Write(screenshotAsByteArray, 0, screenshotAsByteArray.Length);
            //    //Console.Error.WriteLine("Exception: " + );
            //    Console.Error.WriteLine("See: " + sScreenshotFileName);
            //}
            //if (RunSession.DriverSession.Driver.PageSource.Contains("Error 1"))
            //{
            //    Console.Error.WriteLine("There was an unexpected Error 1 due to which the test failed.");
            //}
            //if (RunSession.DriverSession.Driver.PageSource.Contains("Error 2"))
            //{
            //    Console.Error.WriteLine("There was an unexpected Error 2 due to which the test failed.");
            //}

            #endregion
        }

        private static string _GetFailedFilename()
        {
            var randomFilename = Path.GetRandomFileName();
            if (randomFilename.Length > 8)
            {
                randomFilename = randomFilename.Substring(0, 8);
            }
            return $"Failed_{randomFilename}";
        }

        [OneTimeTearDown]
        protected void TestBaseFixtureTearDown()
        {
            Session.Quit();
            Session = null;
            WriteInfoLogEntry(endTestFixtureTearDown(GetType().Name));
            Logger.Reset();
        }

        protected void LoadSuiteSettings()
        {
            TestSuiteSettings = GetTestSettings<TSuiteSettingsType>("SuiteSettings", SettingsType.ProjectBound);

            // if there are no values or, it's the default - then hydrate
            if (TestSuiteSettings == null)
            {
                TestSuiteSettings = new TSuiteSettingsType();
                TestSuiteSettings.HydrateWithDefaults();
            }

            /* Add an additional capability called "name" for use in SauceLabs.
             * This is the only logical place we can do this as we need to set the
             * value of the capability to the name of the test fixture class.
             */
            if (TestSuiteSettings.WebDriverSettings.HubType.EnumValue.Equals(HubType.SauceLabs))
            {
                _SetNameCapability();
            }
            PersistTestSettings(TestSuiteSettings, "SuiteSettings", SettingsType.ProjectBound);
        }

        private void _SetNameCapability()
        {
            if (!string.IsNullOrEmpty(TestBaseNamespace) && CurrentTestTypeName.StartsWith(TestBaseNamespace))
            {
                if (TestSuiteSettings.WebDriverSettings.AdditionalCapabilities.Exists(ac => ac.Id == "name"))
                {
                    TestSuiteSettings.WebDriverSettings.AdditionalCapabilities.Find(ac => ac.Id == "name").Value =
                        CurrentTestTypeName;
                }
                else
                {
                    var testNameCapability = new AdditionalCapability { Id = "name", Value = CurrentTestTypeName };
                    TestSuiteSettings.WebDriverSettings.AdditionalCapabilities.Add(testNameCapability);
                }
            }
        }

        /// <summary>
        /// Navigates to a specific launch page
        /// </summary>
        /// <param name="targetPage"></param>
        /// <param name="handleNavigateAwayWarning"></param>
        protected void NavigateToLaunchSite(int targetPage, bool handleNavigateAwayWarning = true)
        {
            string launchPage = TestSuiteSettings.GetLaunchPage(targetPage);

            Session.DriverSession.Driver.Navigate().GoToUrl(launchPage);

            if (handleNavigateAwayWarning)
            {
                string alertMessage;
                if (IsAlertDisplayed(out alertMessage))
                {
                    if (alertMessage == "If you choose to leave the page, you will be logged out of the payroll application and you will lose any data that isn’t saved. Are you sure you want to continue?")
                    {
                        Session.DriverSession.Driver.SwitchTo().Alert().Accept();
                    }
                }
            }
        }

        /// <summary>
        /// Navigates to a specific launch page
        /// </summary>
        /// <param name="targetPage"></param>
        /// <param name="launchPageHandler"></param>
        /// <param name="handleNavigateAwayWarning"></param>
        protected void NavigateToLaunchSite(int targetPage, ILaunchPageHandler launchPageHandler, bool handleNavigateAwayWarning = true)
        {
            string launchPage = TestSuiteSettings.GetLaunchPage(targetPage, launchPageHandler);

            Session.DriverSession.Driver.Navigate().GoToUrl(launchPage);

            if (handleNavigateAwayWarning)
            {
                string alertMessage;
                if (IsAlertDisplayed(out alertMessage))
                {
                    if (alertMessage == "If you choose to leave the page, you will be logged out of the payroll application and you will lose any data that isn’t saved. Are you sure you want to continue?")
                    {
                        Session.DriverSession.Driver.SwitchTo().Alert().Accept();
                    }
                }
            }
        }

        /// <summary>
        /// Starts up a new session using the test suite defaults, but overriding the browser and platform together with version
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="browserVersion"></param>
        /// <param name="platform"></param>
        /// <param name="platformVersion"></param>
        /// <param name="startPage"></param>
        /// <param name="launchPageHandler"></param>
        /// <returns></returns>
        protected ISession StartUpSession(Browser browser, string browserVersion, Platform platform, string platformVersion, int startPage, ILaunchPageHandler launchPageHandler)
        {
            var launchPage = TestSuiteSettings.GetLaunchPage(startPage, launchPageHandler);

            TestSuiteSettings.WebDriverSettings.Browser.Value = browser.ToString();
            TestSuiteSettings.WebDriverSettings.Platform.Value = platform.ToString();
            TestSuiteSettings.WebDriverSettings.BrowserVersion = browserVersion;
            TestSuiteSettings.WebDriverSettings.PlatformVersion = platformVersion;

            return StartUpSession(TestSuiteSettings, launchPage);
        }

        /// <summary>
        /// Starts up a new session using the test suite defaults, but overriding the browser and platform together with version
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="browserVersion"></param>
        /// <param name="platform"></param>
        /// <param name="platformVersion"></param>
        /// <param name="siteUrl"></param>
        /// <returns></returns>
        protected ISession StartUpSession(Browser browser, string browserVersion, Platform platform, string platformVersion, string siteUrl)
        {
            TestSuiteSettings.WebDriverSettings.Browser.Value = browser.ToString();
            TestSuiteSettings.WebDriverSettings.Platform.Value = platform.ToString();
            TestSuiteSettings.WebDriverSettings.BrowserVersion = browserVersion;
            TestSuiteSettings.WebDriverSettings.PlatformVersion = platformVersion;

            return StartUpSession(TestSuiteSettings, siteUrl);
        }

        /// <summary>
        /// Starts up a new session using the given driver settings
        /// </summary>
        /// <param name="suiteSettings"></param>
        /// <param name="siteUrl"></param>
        /// <returns></returns>
        protected ISession StartUpSession(ISuiteSettings suiteSettings, string siteUrl)
        {
            var driverFactory = _GetDriverFactory(suiteSettings.WebDriverSettings);
            var driverSession = new WebDriverSession(driverFactory, suiteSettings);
            driverSession.Start(siteUrl);
            return new Session(driverSession);
        }

        /// <summary>
        /// Starts a new session using the test suite settings defaults
        /// </summary>
        /// <param name="siteUrl"></param>
        /// <returns></returns>
        protected ISession StartUpSession(string siteUrl)
        {
            return StartUpSession(TestSuiteSettings, siteUrl);
        }

        /// <summary>
        /// 
        /// </summary>
        protected void StartUpSessionWithoutLogging()
        {
            CreateSession();
        }

        private XmlDocument GetSettingsXmlFile(SettingsType settingsType)
        {
            var suiteSettings = new XmlDocument();

            string outputFile = _GetOutputFile(settingsType);

            if (File.Exists(outputFile))
            {
                suiteSettings.Load(outputFile);
            }
            else
            {
                XmlNode rootNode = suiteSettings.CreateElement("testSettings");
                suiteSettings.AppendChild(rootNode);
                suiteSettings.Save(outputFile);
            }

            return suiteSettings;
        }

        private void SaveSettingsXmlFile(XmlDocument settings, SettingsType settingsType)
        {
            string outputFile = _GetOutputFile(settingsType);

            settings.Save(outputFile);
        }

        private string _GetOutputFile(SettingsType settingsType)
        {
            switch (settingsType)
            {
                case SettingsType.ProjectBound:
                    {
                        return ExecutionSettings.SettingsFilePath();
                    }
                case SettingsType.CentrallyStored:
                    {
                        return ExecutionSettings.SettingsStorePath();
                    }
                default:
                    {
                        return null;
                    }
            }
        }
        private T GetTestSettings<T>(string matchingId, SettingsType settingsType) where T : IPersistableSettings, new()
        {
            System.Diagnostics.Debug.WriteLine($"Looking for test settings for: {matchingId}");

            XmlDocument suiteSettings = GetSettingsXmlFile(settingsType);

            XmlNode matchingElement = suiteSettings.SelectSingleNode(string.Format(TestSettingsXPathQuery, matchingId));

            if (matchingElement == null)
            {
                return default(T);
            }

            return RehydrateSettings<T>(matchingId, matchingElement.InnerText, settingsType);
        }

        private bool RehydrateChildSettings<T>(T sourceObject) where T : ISelfHydratableSetting
        {
            bool anyChanged = false;

            // check object for any null properties, if there are any, hydrate a new object and copy the values ito retObject
            Type oType = sourceObject.GetType();

            var nullProperties = new List<string>();

            PropertyInfo[] properties = oType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(sourceObject, null);

                // if the object is a IHydratableSettings
                if (typeof(ISelfHydratableSetting).IsAssignableFrom(property.PropertyType))
                {
                    // if the value is null, just add it to the null list and move on
                    if (value == null)
                    {
                        nullProperties.Add(property.Name);
                    }
                    else
                    {
                        anyChanged = RehydrateChildSettings((ISelfHydratableSetting)value);
                        // the property isn't null, so check each property of it
                    }
                }
                else
                {
                    bool bNullable = property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);

                    // if the type is nullable, we want to ignore it as a null might be legit
                    if (value == null && !bNullable)
                    {
                        nullProperties.Add(property.Name);
                    }
                }
            }

            if (nullProperties.Any())
            {
                // if the object can be self hyfdrated
                if (sourceObject is ISelfHydratable<T>)
                {
                    T defaultT = ((ISelfHydratable<T>)sourceObject).SelfHydrate();

                    foreach (string propName in nullProperties)
                    {
                        string name = propName;
                        PropertyInfo property = properties.First(s => s.Name == name);

                        object defaultValue = property.GetValue(defaultT, null);

                        if (defaultValue != null)
                        {
                            property.SetValue(sourceObject, defaultValue);
                            anyChanged = true;
                        }
                    }
                }
            }

            return anyChanged;
        }

        private T RehydrateSettings<T>(string settingId, string jsonString, SettingsType settingsType) where T : IPersistableSettings, new()
        {
            var retObject = SettingsPersistance.Deserialize<T>(jsonString);
            retObject.AssignSettingId(settingId);

            // check object for any null properties, if there are any, hydrate a new object and copy the values ito retObject
            Type oType = typeof(T);

            var nullProperties = new List<string>();

            bool anyChanged = false;

            PropertyInfo[] properties = oType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(retObject, null);

                // if the object is a IHydratableSettings
                if (typeof(ISelfHydratableSetting).IsAssignableFrom(property.PropertyType))
                {
                    // if the value is null, just add it to the null list and move on
                    if (value == null)
                    {
                        nullProperties.Add(property.Name);
                    }
                    else
                    {
                        anyChanged = RehydrateChildSettings((ISelfHydratableSetting)value);
                        // the property isn't null, so check each property of it
                    }
                }
                else
                {
                    bool bNullable = property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);

                    // if the type is nullable, we want to ignore it as a null might be legit
                    if (value == null && !bNullable)
                    {
                        nullProperties.Add(property.Name);
                    }
                }
            }

            if (nullProperties.Any())
            {
                var defaultT = new T();
                defaultT.HydrateWithDefaults();

                foreach (string propName in nullProperties)
                {
                    string name = propName;
                    PropertyInfo property = properties.First(s => s.Name == name);

                    object defaultValue = property.GetValue(defaultT, null);

                    if (defaultValue != null)
                    {
                        property.SetValue(retObject, defaultValue);
                        anyChanged = true;
                    }
                }
            }

            if (anyChanged)
            {
                PersistTestSettings(retObject, settingId, settingsType);
            }

            return retObject;
        }

        protected T GetTestSettings<T>(SettingsType settingsType) where T : IPersistableSettings, new()
        {
            return GetTestSettings<T>(CurrentTestTypeName, settingsType);
        }

        protected T GetTestSettingsAndPersistIfDefault<T>(SettingsType settingsType) where T : IPersistableSettings, new()
        {
            var testSettings = GetTestSettings<T>(settingsType);

            if (testSettings != null)
            {
                return testSettings;
            }

            testSettings = new T();
            testSettings.HydrateWithDefaults();

            PersistTestSettings(settingsType, testSettings);
            return testSettings;
        }

        protected T GetTestSettingsAndPersistIfDefault<T>(SettingsType settingsType, Func<T, T> furtherHydration) where T : IPersistableSettings, new()
        {
            var testSettings = GetTestSettings<T>(settingsType);

            if (testSettings != null)
            {
                return testSettings;
            }

            testSettings = new T();
            testSettings.HydrateWithDefaults();

            if (furtherHydration != null)
            {
                furtherHydration(testSettings);
            }

            PersistTestSettings(settingsType, testSettings);
            return testSettings;
        }

        private void PersistTestSettings<T>(T settings, string matchingId, SettingsType settingsType) where T : IPersistableSettings
        {
            XmlDocument suiteSettings = GetSettingsXmlFile(settingsType);

            XmlNode matchingElement = suiteSettings.SelectSingleNode(string.Format(TestSettingsXPathQuery, matchingId));

            if (matchingElement == null)
            {
                matchingElement = suiteSettings.CreateElement("testSetting");
                XmlAttribute elementId = suiteSettings.CreateAttribute("id");
                elementId.InnerText = matchingId;
                matchingElement.Attributes.Append(elementId);
                suiteSettings.DocumentElement.AppendChild(matchingElement);
            }

            matchingElement.InnerText = SettingsPersistance.Serialize(settings);

            SaveSettingsXmlFile(suiteSettings, settingsType);
        }

        protected void PersistTestSettings<T>(SettingsType settingsType, T settings) where T : IPersistableSettings
        {
            PersistTestSettings(settings, CurrentTestTypeName, settingsType);
        }

        protected void PersistSuiteSettings<T>(T settings) where T : TSuiteSettingsType
        {
            PersistTestSettings(settings, "SuiteSettings", SettingsType.ProjectBound);
        }

        private void CreateSession()
        {
            var driverFactory = _GetDriverFactory(TestSuiteSettings.WebDriverSettings);
            var driverSession = new WebDriverSession(driverFactory, TestSuiteSettings);

            // Print Session ID to the console for SauceLabs plugin to work in Jenkins
            if (TestSuiteSettings.WebDriverSettings.HubType.EnumValue.Equals(HubType.SauceLabs))
            {
                Console.WriteLine("SauceOnDemandSessionID={0} job-name={1}",
                    _GetSessionId(driverSession.Driver),
                    TestSuiteSettings.WebDriverSettings.AdditionalCapabilities.Find(ac => ac.Id.Equals("name")).Value);
            }

            //get the launch page to start the session
            var launchPage = TestSuiteSettings.GetLaunchPage(SiteLaunchTarget);
            //if basic authentication, insert the desired username and password into the url
            if (TestSuiteSettings.ApplicationUnderTestSettings.BasicAuthentication)
            {
                var user =
                    TestSuiteSettings.ApplicationUnderTestSettings.Users.SingleOrDefault(
                        u => u.Username.Equals(BasicAuthUsername));
                if (user != null) launchPage = string.Format(launchPage, user.Username, user.Password);
                else throw new InvalidDataException("The supplied username {0} could not be found in suite settings data");
            }
            driverSession.Start(launchPage);
            Session = new Session(driverSession);
        }

        private static IDriverFactory _GetDriverFactory(DriverSettings driverSettings)
        {
            IDriverFactory driverFactory = null;
            if (driverSettings.HubType.EnumValue.Equals(HubType.None))
            {
                switch (driverSettings.Browser.EnumValue)
                {
                    case Browser.Chrome:
                        {
                            driverFactory = new ChromeDriverFactory();
                            break;
                        }
                    case Browser.Firefox:
                        {
                            driverFactory = new FirefoxDriverFactory();
                            break;
                        }
                    //TODO: handle all cases
                }
            }
            switch (driverSettings.Platform.EnumValue)
            {
                case Platform.Windows:
                case Platform.Linux:
                case Platform.Mac:
                    {
                        driverFactory = new RemoteWebDriverFactory();
                        break;
                    }
                case Platform.iOS:
                    {
                        driverFactory = new IOSDriverFactory();
                        break;
                    }
                case Platform.Android:
                    {
                        driverFactory = new AndroidDriverFactory();
                        break;
                    }
            }
            return driverFactory;
        }
        private static SessionId _GetSessionId(IWebDriver driver)
        {
            var sessionIdProperty = typeof(RemoteWebDriver).GetProperty("SessionId", BindingFlags.Instance | BindingFlags.NonPublic);
            SessionId sessionId = null;
            if (sessionIdProperty != null)
            {
                sessionId = sessionIdProperty.GetValue(driver, null) as SessionId;
            }
            return sessionId;
        }

        private static void ConfigureLogging()
        {
            IConfigurationSource configurationSource = ConfigurationSourceFactory.Create();
            var logWriterFactory = new LogWriterFactory(configurationSource);
            try
            {
                Logger.SetLogWriter(logWriterFactory.Create());
            }
            catch (InvalidOperationException)
            {
                Logger.Reset();
                Logger.SetLogWriter(logWriterFactory.Create());
            }
        }

        /// <summary>
        /// Take a screenshot of the current browser UI
        /// </summary>
        /// <param name="targetFileName">The name of the file to save the results as, **excluding any file extension**</param>
        /// <param name="imageOverlayText">If required, any text to overlay on the image, such as the contents of a dialog message</param>
        /// <param name="isHappyPathImage">If set to true, the text will be green, otherwise it will be red</param>
        protected void TakeScreenshot(string targetFileName, string imageOverlayText = "", bool isHappyPathImage = false)
        {
            string dialogMessage;
            if (Session.DriverSession.Driver.IsAlertBoxDisplayed(out dialogMessage))
            {
                string errorMessage =
                    $"Unable to create a screenshot due to an unhandled popup with following message - {dialogMessage}";
                LogX.Error.Write(errorMessage);
                Console.Error.WriteLine(errorMessage);
                throw new WebDriverException(errorMessage);
            }
            string fullSavePath = Path.Combine(GetTestResultsPathWithTestName(true), targetFileName.SanitiseFilename());
            ScreenshotHelper.TakeScreenshot(Session.DriverSession.Driver, fullSavePath, imageOverlayText, isHappyPathImage);
        }

        /// <summary>
        /// Is an alert displayed
        /// </summary>
        /// <param name="dialogMessage"></param>
        /// <returns></returns>
        protected bool IsAlertDisplayed(out string dialogMessage)
        {
            return Session.DriverSession.Driver.IsAlertBoxDisplayed(out dialogMessage);
        }

        /// <summary>
        /// Accept the alert if present
        /// </summary>
        protected void AcceptAlert()
        {
            string alertText;
            if (IsAlertDisplayed(out alertText))
            {
                Session.DriverSession.Driver.SwitchTo().Alert().Accept();
            }
        }

        /// <summary>
        /// Dismiss the alert if present
        /// </summary>
        protected void DismissAlert()
        {
            string alertText;
            if (IsAlertDisplayed(out alertText))
            {
                Session.DriverSession.Driver.SwitchTo().Alert().Dismiss();
            }
        }

        /// <summary>
        /// For the given external data provider, execute the query against it and return the results
        /// </summary>
        /// <typeparam name="TQuery">Type of query</typeparam>
        /// <typeparam name="TResult">Type of return object</typeparam>
        /// <param name="iProvider">The query provider</param>
        /// <returns></returns>
        protected TResult PerformExteralDataQuery<TQuery, TResult>(IExternalDataProvider<TQuery, TResult> iProvider)
            where TQuery : IExternalDataQuery
            where TResult : IExternalDataResult
        {
            return iProvider.Execute();
        }
    }
}