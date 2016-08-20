using System;
using Ravitej.Automation.Common.Drivers;
using Ravitej.Automation.Common.PageObjects.Session.Navigators;

namespace Ravitej.Automation.Common.PageObjects.Session
{
    public class Session : ISession
    {
        public IDriverSession DriverSession { get; }
        public GoTo GoTo { get; protected internal set; }
        public OnPage OnPage { get; protected internal set; }

        public Session(IDriverSession driverSession)
        {
            DriverSession = driverSession;
            OnPage = new OnPage(driverSession);
            GoTo = new GoTo(driverSession);
        }

        public void Quit()
        {
            try
            {
                DriverSession.Driver.Close();
            }
            catch
            {
                // ignore
            }

            try
            {
                DriverSession.Driver.SwitchTo().Alert().Accept();
                DriverSession.Driver.Quit();
            }
            catch (Exception)
            {
                DriverSession.Driver.Quit();
            }
            DriverSession.Dispose();
        }
    }
}