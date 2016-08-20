namespace Ravitej.Automation.SeleniumHubNodeLauncher.Library.Data.Entities
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class LauncherDataProcessesProcess
    {

        private string _typeField;

        private string _activeField;

        private string _logFileField;

        private string _idField;

        private string _portField;

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
        public string Active
        {
            get
            {
                return this._activeField;
            }
            set
            {
                this._activeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string LogFile
        {
            get
            {
                return this._logFileField;
            }
            set
            {
                this._logFileField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Id
        {
            get
            {
                return this._idField;
            }
            set
            {
                this._idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Port
        {
            get
            {
                return this._portField;
            }
            set
            {
                this._portField = value;
            }
        }
    }
}