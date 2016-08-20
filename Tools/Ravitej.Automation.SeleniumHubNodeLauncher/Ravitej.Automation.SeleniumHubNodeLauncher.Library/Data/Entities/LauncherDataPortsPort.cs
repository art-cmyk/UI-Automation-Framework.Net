namespace Ravitej.Automation.SeleniumHubNodeLauncher.Library.Data.Entities
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class LauncherDataPortsPort
    {

        private string _typeField;

        private string _numberField;

        private string _lockedField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Type
        {
            get
            {
                return this._typeField;
            }
            set
            {
                this._typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Number
        {
            get
            {
                return this._numberField;
            }
            set
            {
                this._numberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Locked
        {
            get
            {
                return this._lockedField;
            }
            set
            {
                this._lockedField = value;
            }
        }
    }
}