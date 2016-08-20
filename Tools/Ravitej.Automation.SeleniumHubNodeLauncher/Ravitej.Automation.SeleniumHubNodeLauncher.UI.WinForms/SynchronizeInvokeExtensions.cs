using System;
using System.ComponentModel;

namespace Ravitej.Automation.SeleniumHubNodeLauncher.UI.WinForms
{
    public static class SynchronizeInvokeExtensions
    {
        public static void InvokeEx<T>(this T @this, Action<T> action) where T : ISynchronizeInvoke
        {
            if (@this.InvokeRequired)
            {
                @this.Invoke(action, new object[] { @this });
            }
            else
            {
                action(@this);
            }
        }
    }
}
