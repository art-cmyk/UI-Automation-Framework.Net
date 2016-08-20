using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using OpenQA.Selenium;

namespace Ravitej.Automation.Common.PageObjects.Generator
{
    /// <summary>
    /// Base class for all page object generation
    /// </summary>
    public abstract class GeneratorBase
    {
        /// <summary>
        /// 
        /// </summary>
        protected IWebElement FrameBody;

        /// <summary>
        /// 
        /// </summary>
        protected IEnumerable<string> ElementsToExcludeFromDomParsing;

        /// <summary>
        /// 
        /// </summary>
        protected string NameSpace;

        /// <summary>
        /// 
        /// </summary>
        protected string FileName;

        /// <summary>
        /// 
        /// </summary>
        protected string FriendlyName;

        /// <summary>
        /// 
        /// </summary>
        protected string FileSystemRootFolder;

        private string ConvertFileSystemRootFolderToAbsoluteFolder(string pageObjectPart)
        {
            string localPath = new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath;
            string rootFolder = localPath.Substring(0, localPath.IndexOf("Ravitej.Automation", StringComparison.OrdinalIgnoreCase));
            return System.IO.Path.Combine(rootFolder, pageObjectPart);
        }

        /// <summary>
        /// Protected constructor
        /// </summary>
        /// <param name="pageObjectPart"></param>
        /// <param name="frameBody"></param>
        /// <param name="elementsToExcludeFromDomParsing"></param>
        /// <param name="nameSpace"></param>
        /// <param name="fileName"></param>
        /// <param name="friendlyName"></param>
        protected GeneratorBase(string pageObjectPart, IWebElement frameBody, IEnumerable<string> elementsToExcludeFromDomParsing, string nameSpace, string fileName, string friendlyName)
        {
            FrameBody = frameBody;
            ElementsToExcludeFromDomParsing = elementsToExcludeFromDomParsing;
            NameSpace = nameSpace;
            FileName = fileName;
            FriendlyName = friendlyName;
            FileSystemRootFolder = ConvertFileSystemRootFolderToAbsoluteFolder(pageObjectPart);
        }

        /// <summary>
        /// For the passed in control name, convert it into something human readable
        /// </summary>
        /// <param name="controlName"></param>
        /// <returns></returns>
        protected string NamePageControlAccordingToConvention(string controlName)
        {
            // go from say, txtFieldName to FieldName

            if (controlName.StartsWith("txt") || controlName.StartsWith("cbo"))
            {
                // strip the first 3 chatacters
                controlName = controlName.Substring(3);
            }

            var r = new Regex("(?<=[a-z])(?<x>[A-Z])|(?<=.)(?<x>[A-Z])(?=[a-z])");
            string newVal = r.Replace(controlName, "${x}");

            if (newVal.EndsWith("ID"))
            {
                newVal = newVal.Substring(0, newVal.Length - 2) + "Id";
            }

            if (newVal.Substring(0,1) == newVal.Substring(0,1).ToLowerInvariant())
            {
                newVal = newVal.Substring(0, 1).ToUpperInvariant() + newVal.Substring(1);
            }

            if (newVal == newVal.ToUpperInvariant())
            {
                newVal = newVal.Substring(0, 1).ToUpperInvariant() + newVal.Substring(1).ToLowerInvariant();
            }

            return newVal;

            //return char.ToUpper(controlName[0]) + controlName.Substring(1);
        }

        /// <summary>
        /// Generate the necessary Get/Set methods for the passed in control
        /// </summary>
        /// <param name="targetControl"></param>
        /// <param name="className"></param>
        /// <param name="targetFrameId"></param>
        /// <returns></returns>
        protected string GenerateGetSetCodeForControl(ControlType targetControl, string className, string targetFrameId)
        {
            switch (targetControl.ControlElementType.ToLowerInvariant())
            {
                case "select":
                    {
                        return GenerateSelectControlCode(targetControl, className, targetFrameId);
                    }
                case "button":
                case "a":
                    {
                        return GenerateClickableControlCode(targetControl, className, targetFrameId);
                    }
                default:
                    {
                        return GenerateInputControlCode(targetControl, className, targetFrameId);
                    }
            }            
        }

        /// <summary>
        /// Generate the necessary Interface lines for the passed in control
        /// </summary>
        /// <param name="targetControl"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        protected string GenerateInterfaceForControl(ControlType targetControl, string className)
        {
            switch (targetControl.ControlElementType.ToLowerInvariant())
            {
                case "select":
                    {
                        return GenerateSelectControlCodeInterface(targetControl, className);
                    }
                case "button":
                case "a":
                    {
                        return GenerateClickableControlCodeInterface(targetControl, className);
                    }
                default:
                    {
                        return GenerateInputControlCodeInterface(targetControl, className);
                    }
            }
        }

        private string GenerateClickableControlCodeInterface(ControlType targetControl, string className)
        {
            var sBuild = new StringBuilder();

            sBuild.AppendLine($"\t\tI{className} Click{targetControl.ControlName}();");

            sBuild.AppendLine("");

            return sBuild.ToString();
        }

        private string GenerateClickableControlCode(ControlType targetControl, string className, string targetFrameId)
        {
            var sBuild = new StringBuilder();

            // write out the click
            sBuild.AppendLine($"\t\t[UserInterfaceElement(ElementId = \"{targetControl.ControlId}\")]");
            sBuild.AppendLine($"\t\tpublic I{className} Click{targetControl.ControlName}()");
            sBuild.AppendLine("\t\t{");

            sBuild.AppendLine(string.IsNullOrEmpty(targetFrameId) ?
                $"\t\t\tClick(By.Id(\"{targetControl.ControlId}\"), \"{targetControl.ControlName}\");"
                : $"\t\t\tClick(By.Id(\"{targetControl.ControlId}\"), \"{targetControl.ControlName}\", \"{targetFrameId}\");");

            sBuild.AppendLine("\t\t\treturn this;");
            sBuild.AppendLine("\t\t}");

            sBuild.AppendLine("");

            return sBuild.ToString();

        }

        private string GenerateSelectControlCodeInterface(ControlType targetControl, string className)
        {
            var sBuild = new StringBuilder();

            // write out the set 
            sBuild.AppendLine($"\t\tI{className} Enter{targetControl.ControlName}(string value);");

            // write out the get
            sBuild.AppendLine($"\t\tstring Get{targetControl.ControlName}Text();");

            sBuild.AppendLine("");

            return sBuild.ToString();
        }

        private string GenerateSelectControlCode(ControlType targetControl, string className, string targetFrameId)
        {
            var sBuild = new StringBuilder();

            sBuild.AppendLine($"\t\tpublic I{className} Enter{targetControl.ControlName}(string value)");
            sBuild.AppendLine("\t\t{");

            sBuild.AppendLine(string.IsNullOrEmpty(targetFrameId) ?
                $"\t\t\tSelectByText(By.Id(\"{targetControl.ControlId}\"), \"{targetControl.ControlName}\", value);"
                : $"\t\t\tSelectByText(By.Id(\"{targetControl.ControlId}\"), \"{targetControl.ControlName}\", value, \"{targetFrameId}\");");

            sBuild.AppendLine("\t\t\treturn this;");
            sBuild.AppendLine("\t\t}");
            sBuild.AppendLine("");

            // write out the get
            sBuild.AppendLine($"\t\t[UserInterfaceElement(ElementId = \"{targetControl.ControlId}\")]");
            sBuild.AppendLine($"\t\tpublic string Get{targetControl.ControlName}Text()");
            sBuild.AppendLine("\t\t{");

            sBuild.AppendLine(string.IsNullOrEmpty(targetFrameId) ?
                $"\t\t\tstring returnValue = GetSelectedText(WebElementType.JQueryCombo, By.Id(\"{targetControl.ControlId}\"), \"{targetControl.ControlName}\");"
                : $"\t\t\tstring returnValue = GetSelectedText(WebElementType.JQueryCombo, By.Id(\"{targetControl.ControlId}\"), \"{targetControl.ControlName}\", \"{targetFrameId}\");");

            sBuild.AppendLine("\t\t\treturn returnValue;");
            sBuild.AppendLine("\t\t}");

            sBuild.AppendLine("");

            return sBuild.ToString();
        }

        private string GenerateInputControlCodeInterface(ControlType targetControl, string className)
        {
            if (!targetControl.Attributes.ContainsKey("type"))
            {
                return GenerateInput_TextBox_ControlCodeInterface(targetControl, className);
            }

            switch (targetControl.Attributes["type"].ToLowerInvariant())
            {
                case "password":
                    {
                        return GenerateInput_Password_ControlCodeInterface(targetControl, className);
                    }
                case "checkbox":
                    {
                        return Generate_Togglable_ControlCodeInterface(targetControl, className);
                    }
                case "radio":
                    {
                        return Generate_Togglable_ControlCodeInterface(targetControl, className);
                    }
                case "submit":
                    {
                        return GenerateClickableControlCodeInterface(targetControl, className);
                    }
                case "button":
                    {
                        return GenerateClickableControlCodeInterface(targetControl, className);
                    }
                default:
                    {
                        return GenerateInput_TextBox_ControlCodeInterface(targetControl, className);
                    }
            }
        }

        private string GenerateInput_TextBox_ControlCodeInterface(ControlType targetControl, string className)
        {
            var sBuild = new StringBuilder();

            // write out the set 
            sBuild.AppendLine($"\t\tI{className} Enter{targetControl.ControlName}(string value);");
            // write out the get
            sBuild.AppendLine($"\t\tstring Get{targetControl.ControlName}();");

            sBuild.AppendLine("");

            return sBuild.ToString();
        }

        private string GenerateInput_Password_ControlCodeInterface(ControlType targetControl, string className)
        {
            var sBuild = new StringBuilder();

            // write out the set 
            sBuild.AppendLine($"\t\tI{className} Enter{targetControl.ControlName}(string value);");

            sBuild.AppendLine("");

            return sBuild.ToString();
        }

        private string GenerateInput_TextBox_ControlCode(ControlType targetControl, string className, string targetFrameId)
        {
            var sBuild = new StringBuilder();

            // write out the set 
            sBuild.AppendLine($"\t\tpublic I{className} Enter{targetControl.ControlName}(string value)");
            sBuild.AppendLine("\t\t{");

            sBuild.AppendLine(string.IsNullOrEmpty(targetFrameId) ?
                $"\t\t\tEnterText(By.Id(\"{targetControl.ControlId}\"), \"{targetControl.ControlName}\", value);"
                : $"\t\t\tEnterText(By.Id(\"{targetControl.ControlId}\"), \"{targetControl.ControlName}\", value, false, \"{targetFrameId}\");");

            sBuild.AppendLine("\t\t\treturn this;");
            sBuild.AppendLine("\t\t}");
            sBuild.AppendLine("");

            // write out the get
            sBuild.AppendLine($"\t\t[UserInterfaceElement(ElementId = \"{targetControl.ControlId}\")]");
            sBuild.AppendLine($"\t\tpublic string Get{targetControl.ControlName}()");
            sBuild.AppendLine("\t\t{");

            sBuild.AppendLine(string.IsNullOrEmpty(targetFrameId) ?
                $"\t\t\tstring returnValue = GetText(By.Id(\"{targetControl.ControlId}\"), \"{targetControl.ControlName}\");"
                : $"\t\t\tstring returnValue = GetText(By.Id(\"{targetControl.ControlId}\"), \"{targetControl.ControlName}\", \"{targetFrameId}\");");

            sBuild.AppendLine("\t\t\treturn returnValue;");
            sBuild.AppendLine("\t\t}");

            return sBuild.ToString();            
        }

        private string GenerateInput_Password_ControlCode(ControlType targetControl, string className, string targetFrameId)
        {
            var sBuild = new StringBuilder();

            // write out the set 
            sBuild.AppendLine($"\t\tpublic I{className} Enter{targetControl.ControlName}(string value)");
            sBuild.AppendLine("\t\t{");

            sBuild.AppendLine(string.IsNullOrEmpty(targetFrameId) ?
                $"\t\t\tEnterText(By.Id(\"{targetControl.ControlId}\"), \"{targetControl.ControlName}\", value);"
                : $"\t\t\tEnterText(By.Id(\"{targetControl.ControlId}\"), \"{targetControl.ControlName}\", value, false, \"{targetFrameId}\");");

            sBuild.AppendLine("\t\t\treturn this;");
            sBuild.AppendLine("\t\t}");
            sBuild.AppendLine("");

            return sBuild.ToString();
        }

        private string Generate_Togglable_ControlCodeInterface(ControlType targetControl, string className)
        {
            var sBuild = new StringBuilder();

            sBuild.AppendLine($"\t\tbool IsTicked{targetControl.ControlName}();");

            sBuild.AppendLine($"\t\tI{className} Tick{targetControl.ControlName}();");

            sBuild.AppendLine($"\t\tI{className} Untick{targetControl.ControlName}();");

            sBuild.AppendLine("");

            return sBuild.ToString();
        }

        private string Generate_Togglable_ControlCode(ControlType targetControl, string className, string targetFrameId)
        {
            var sBuild = new StringBuilder();

            sBuild.AppendLine($"\t\t[UserInterfaceElement(ElementId = \"{targetControl.ControlId}\")]");

            sBuild.AppendLine($"\t\tpublic bool IsTicked{targetControl.ControlName}()");
            sBuild.AppendLine("\t\t{");

            sBuild.AppendLine(string.IsNullOrEmpty(targetFrameId) ?
                $"\t\t\treturn GetElementByIdOrThrow(\"{targetControl.ControlId}\", \"{targetControl.ControlName}\").Selected;"
                : $"\t\t\treturn GetElementByIdOrThrow(\"{targetControl.ControlId}\", \"{targetControl.ControlName}\", \"{targetFrameId}\").Selected;");

            sBuild.AppendLine("\t\t}");

            sBuild.AppendLine($"\t\tpublic I{className} Tick{targetControl.ControlName}()");
            sBuild.AppendLine("\t\t{");
            sBuild.AppendLine($"\t\t\tif (!IsTicked{targetControl.ControlName}())");
            sBuild.AppendLine("\t\t\t{");

            sBuild.AppendLine(string.IsNullOrEmpty(targetFrameId) ?
                $"\t\t\t\tGetElementByIdOrThrow(\"{targetControl.ControlName}\", \"{className}\").Click();"
                : $"\t\t\t\tGetElementByIdOrThrow(\"{targetControl.ControlName}\", \"{className}\", \"{targetFrameId}\").Click();");

            sBuild.AppendLine("\t\t\t}");
            sBuild.AppendLine("\t\t\treturn this;");
            sBuild.AppendLine("\t\t}");

            sBuild.AppendLine($"\t\tpublic I{className} Untick{targetControl.ControlName}()");
            sBuild.AppendLine("\t\t{");
            sBuild.AppendLine($"\t\t\tif (IsTicked{targetControl.ControlName}())");
            sBuild.AppendLine("\t\t\t{");

            sBuild.AppendLine(string.IsNullOrEmpty(targetFrameId) ?
                $"\t\t\t\tGetElementByIdOrThrow(\"{targetControl.ControlName}\", \"{className}\").Click();"
                : $"\t\t\t\tGetElementByIdOrThrow(\"{targetControl.ControlName}\", \"{className}\", \"{targetFrameId}\").Click();");

            sBuild.AppendLine("\t\t\t}");
            sBuild.AppendLine("\t\t\treturn this;");
            sBuild.AppendLine("\t\t}");

            sBuild.AppendLine("");

            return sBuild.ToString();
        }

        private string GenerateInputControlCode(ControlType targetControl, string className, string targetFrameId)
        {
            if (!targetControl.Attributes.ContainsKey("type"))
            {
                return GenerateInput_TextBox_ControlCode(targetControl, className, targetFrameId);                    
            }

            switch (targetControl.Attributes["type"].ToLowerInvariant())
            {
                case "password":
                {
                    return GenerateInput_Password_ControlCode(targetControl, className, targetFrameId);
                }
                case "checkbox":
                {
                    return Generate_Togglable_ControlCode(targetControl, className, targetFrameId);
                }
                case "radio":
                {
                    return Generate_Togglable_ControlCode(targetControl, className, targetFrameId);
                }
                case "submit":
                {
                    return GenerateClickableControlCode(targetControl, className, targetFrameId);
                }
                case "button":
                {
                    return GenerateClickableControlCode(targetControl, className, targetFrameId);
                }
                default:
                {
                    return GenerateInput_TextBox_ControlCode(targetControl, className, targetFrameId);                    
                }
            }
        }

        private ControlType FakeControlType(string id, string elementType, string typeAttribute)
        {
            var controlType = new ControlType();

            if (!string.IsNullOrEmpty(typeAttribute))
            {
                controlType.Attributes.Add("type", typeAttribute);
            }

            controlType.ControlId = string.Concat("fake", id, "Control");
            controlType.ControlName = string.Concat("Fake", id, "ControlName");
            controlType.ControlElementType = elementType;
            return controlType;
        }

        /// <summary>
        /// Generate Fake code for the control types supported by the framework
        /// </summary>
        /// <returns></returns>
        protected List<ControlType> FakeMethodsForEachSupportedControlType()
        {
            var retList = new List<ControlType>
                          {
                              FakeControlType("password", "input", "password"), 
                              FakeControlType("checkbox", "input", "checkbox"), 
                              FakeControlType("radio", "input", "radio"), 
                              FakeControlType("button", "input", "button"), 
                              FakeControlType("submit", "input", "submit"), 
                              FakeControlType("select", "select", ""), 
                              FakeControlType("hidden", "input", "hidden"), 
                              FakeControlType("missingTypeAttribute", "input", ""), 
                              FakeControlType("anchor", "a", "")
                          };

            return retList;
        }

        /// <summary>
        /// Fro the Dom Element, parse all the elements returning a list of control types - excluding those not to be read
        /// </summary>
        /// <param name="elementsToExcludeFromDomParsing"></param>
        /// <param name="domContainer"></param>
        /// <returns></returns>
        protected List<ControlType> GetControlTypesList(List<string> elementsToExcludeFromDomParsing, IWebElement domContainer)
        {
            // get all the web elements that are classed as interactable
            List<IWebElement> actualElements = domContainer.GetAllInteractableElements().ToList();

            var retlist = new List<ControlType>();

            // get them all that have an Id
            foreach (var item in actualElements.Where(s => s.GetAttribute("id") != null))
            {
                string itemId = item.GetAttribute("id");
                if (elementsToExcludeFromDomParsing.Any(s => s == itemId))
                {
                    continue;
                }


                var cType = new ControlType
                {
                    ControlId = item.GetAttribute("id"),
                    ControlElementType = item.TagName.ToLower()
                };

                var expectedAttributes = new[]
                                              {
                                                "accept", 
                                                "align", 
                                                "alt", 
                                                "autocomplete", 
                                                "autofocus", 
                                                "checked", 
                                                "disabled", 
                                                "form", 
                                                "formaction", 
                                                "formenctype", 
                                                "formmethod", 
                                                "formnovalidate", 
                                                "formtarget", 
                                                "framename", 
                                                "max", 
                                                "maxlength", 
                                                "min", 
                                                "multiple", 
                                                "name", 
                                                "pattern", 
                                                "placeholder", 
                                                "readonly", 
                                                "required", 
                                                "size", 
                                                "src", 
                                                "step", 
                                                "type", 
                                                "value",  
                                                "width"
                                              };

                foreach (var attr in expectedAttributes)
                {
                    var elementAttributeValue = item.GetAttribute(attr);
                    if (!string.IsNullOrEmpty(elementAttributeValue))
                    {
                        cType.Attributes.Add(attr, elementAttributeValue);
                    }
                }

                if (string.IsNullOrEmpty(cType.ControlId))
                {
                    continue;
                }

                cType.ControlName = NamePageControlAccordingToConvention(cType.ControlId);

                retlist.Add(cType);
            }

            return retlist;
        }

        /// <summary>
        /// Return a list of all elements that are actually hidden
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        protected string HiddenFormControls(IEnumerable<ControlType> elements)
        {
            var sList = (from item in elements where item.Attributes.ContainsKey("type") && item.Attributes["type"] == "hidden" select item.ControlId).ToList();
            return string.Join(",", sList);
        }
    }
}
