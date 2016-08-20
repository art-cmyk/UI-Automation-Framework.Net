using System.Drawing;
using OpenQA.Selenium;

namespace Ravitej.Automation.Common.Tests.FakeImplementations
{
    public class FakeWindow : IWindow
    {
        public void Maximize()
        {
        }

        public Point Position
        {
            get;
            set;
        }

        public Size Size
        {
            get;
            set;
        }
    }
}