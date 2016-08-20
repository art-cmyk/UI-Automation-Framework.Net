using System.Collections.Generic;

namespace Ravitej.Automation.SeleniumHubNodeLauncher.Library.Data.Entities
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class LauncherDataProcesses
    {

        private List<LauncherDataProcessesProcess> _processesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Process", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public List<LauncherDataProcessesProcess> Processeses
        {
            get
            {
                return this._processesField;
            }
            set
            {
                this._processesField = value;
            }
        }
    }
}