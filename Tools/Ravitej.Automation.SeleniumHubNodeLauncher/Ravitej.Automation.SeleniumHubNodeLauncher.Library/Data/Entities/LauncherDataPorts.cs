using System.Collections.Generic;

namespace Ravitej.Automation.SeleniumHubNodeLauncher.Library.Data.Entities
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class LauncherDataPorts
    {

        private List<LauncherDataPortsPort> _portsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Port", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public List<LauncherDataPortsPort> Portses
        {
            get
            {
                return this._portsField;
            }
            set
            {
                this._portsField = value;
            }
        }
    }
}