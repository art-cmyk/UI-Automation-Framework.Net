using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ravitej.Automation.Common.PageObjects.CustomAttributes;
using Ravitej.Automation.Common.PageObjects.Interactables.Selenium;
using OpenQA.Selenium;

namespace Ravitej.Automation.Common.PageObjects.SelfValidation
{
    /// <summary>
    /// For the given page object, extract the expected PageObject schema so that it can be validated against the DOM
    /// </summary>
    public class SelfValidatingPageObjectSchemaGenerator
    {
        /// <summary>
        /// Extract the schema
        /// </summary>
        /// <param name="pageObject"></param>
        /// <returns></returns>
        public SelfValidatingPageObjectSchema ExtractSchema(Interactable pageObject)
        {
            var retObj = new SelfValidatingPageObjectSchema(pageObject.Name);

            // get the type of the page object
            var pageObjectType = pageObject.GetType();

            var containerElement = pageObject.ContentContainer;

            List<Attribute> classLevelElementList = pageObjectType.GetCustomAttributes(typeof(UserInterfaceElementDefinitionSet)).ToList();
            if (classLevelElementList.Any())
            {
                foreach (var excludeElement in classLevelElementList.Cast<UserInterfaceElementDefinitionSet>())
                {
                    if (excludeElement.ExcludeElementIdList != null)
                    {
                        var splitItems = excludeElement.ExcludeElementIdList.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var item in splitItems)
                        {
                            retObj.KnownExclusionElements.Add(new SelfValidatingItem(item.Trim()));
                        }
                    }

                    if (excludeElement.IncludeElementIdList != null)
                    {
                        var splitItems = excludeElement.IncludeElementIdList.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var item in splitItems)
                        {
                            retObj.ExpectedElements.Add(new SelfValidatingItem(item.Trim()));
                        }
                    }
                }
            }

            // get all accessible methods from the page object
            MethodInfo[] allMethods = pageObjectType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

            var expectedList = allMethods.SelectMany(method => Attribute.GetCustomAttributes(method, typeof(UserInterfaceElement), false).OfType<UserInterfaceElement>()).ToList();

            foreach (var item in expectedList)
            {
                var newItem = new SelfValidatingItem(item.ElementId, item.IsNonInteractableElement);
                retObj.ExpectedElements.Add(newItem);
            }

            // get all the properties from the page object
            PropertyInfo[] objectProperties = pageObjectType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

            // enumerate each of the properties
            foreach (PropertyInfo property in objectProperties)
            {
                // as properties can also be defined as a user interface element, do the ckecking here
                // get the custom attribute
                Attribute userInterfaceElementAttribute = property.GetCustomAttribute(typeof(UserInterfaceElement), false);

                // if it isn't null (the attribute is present)
                if (userInterfaceElementAttribute != null)
                {
                    // cast it to a local variable
                    var xAt = (UserInterfaceElement)userInterfaceElementAttribute;

                    // create a new item
                    var newItem = new SelfValidatingItem(xAt.ElementId, xAt.IsNonInteractableElement);

                    // add it to the collection
                    retObj.ExpectedElements.Add(newItem);
                }

                // does the property item implement the ISelfValidatingPageObject interface
                if (typeof(ISelfValidatingPageObject).IsAssignableFrom(property.PropertyType))
                {
                    // get the property value of the property and cast it to a ISelfValidatingPageObject
                    var selfValidatingChildObject = (Interactable)property.GetValue(pageObject);

                    // extract the schema from the child object (recursion here)
                    var childObject = ExtractSchema(selfValidatingChildObject);

                    // if the child object is also IExcludeElementsWhenAChildObject, make sure to add all elements to the ignore list
                    if (typeof(IExcludeElementsWhenAChildObject).IsAssignableFrom(property.PropertyType))
                    {
                        childObject.KnownExclusionElements.AddRange(childObject.ActualElements);
                        childObject.ActualElements.Clear();

                        childObject.KnownExclusionElements.AddRange(childObject.ExpectedElements);
                        childObject.ExpectedElements.Clear();
                    }

                    // append the results to the outer most list
                    retObj.AppendChild(childObject);
                }
            }

            var actualElements = new List<IWebElement>();
            if (containerElement != null)
            {
                // now all defined elements are known, get the elements that are actually on the DOM
                actualElements = containerElement.GetAllInteractableElements().ToList();

                // if there are any items which are not classed as interactable and are expected, make sure they are retrieved from the DOM too
                foreach (var item in retObj.ExpectedElements.Where(s => s.IsNonInteractableElement))
                {
                    var webItem = containerElement.FindElement(By.Id(item.Id));
                    if (webItem == null)
                    {
                        retObj.InvalidElementDetails.Add(string.Concat("Tag: ", item.Id, "; Text:Explicitly specified non interactable element"));
                    }
                    else
                    {
                        actualElements.Add(webItem);
                    }
                }
            }

            // find all elements in the interactable element list which do not have an Id - it is a guideline that they all have one.
            var actualElementsWithoutIds = actualElements.Where(item => string.IsNullOrEmpty(item.GetAttribute("id"))).ToList();
            if (actualElementsWithoutIds.Any())
            {
                // build a list of some form of identifier.
                retObj.InvalidElementDetails.AddRange(actualElementsWithoutIds.Select(item => string.Concat("Tag: ", item.TagName, "; Text:", item.Text, "; Outer Html:", item.GetAttribute("outerHTML"))).ToList());

                // log each item which is missing an Id.
                retObj.SchemaValid = false;
            }

            IEnumerable<string> actuals = actualElements.Select(element => element.GetAttribute("id")).Where(elementId => !string.IsNullOrWhiteSpace(elementId));
            foreach (var item in actuals)
            {
                retObj.ActualElements.Add(new SelfValidatingItem(item));
            }

            return retObj;
        }
    }
}
