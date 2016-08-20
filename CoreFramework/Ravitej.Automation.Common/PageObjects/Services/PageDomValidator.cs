using System;
using System.Collections.Generic;
using System.Linq;
using Ravitej.Automation.Common.PageObjects.Exceptions;
using Ravitej.Automation.Common.PageObjects.Interactables.Selenium;
using Ravitej.Automation.Common.PageObjects.SelfValidation;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Ravitej.Automation.Common.PageObjects.Services
{
    /// <summary>
    /// The class that provides methods to validate the displayed page's DOM against its page object.
    /// </summary>
    public class PageDomValidator
    {
        /// <exception cref="InvalidPageDomStructureException">Thrown when DOM structure of the displayed page is invalid.</exception>
        public bool ValidateDisplayedPage<T>(T pageObject, List<string> elementIdsToExcludeFromDomComparison) where T : Interactable, ISelfValidatingPageObject
        {
            var pageObjectSchema = new SelfValidatingPageObjectSchemaGenerator().ExtractSchema(pageObject);

            if (!pageObjectSchema.SchemaValid)
            {
                // log each item which is missing an Id.
                LogX.Info.Write("One or more invalid DOM elements was detected during a ValidatePageObjectAgainstWebElementContents call.", pageObjectSchema.InvalidElementDetails);

                // throw an exception
                throw new InvalidPageDomStructureException(pageObject.Name, pageObjectSchema.InvalidElementDetails);
            }

            return ValidateExpectedDomElementListAgainstActualDomElementList(pageObjectSchema, elementIdsToExcludeFromDomComparison);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="knownExcludeItems"></param>
        /// <returns></returns>
        private bool ValidateExpectedDomElementListAgainstActualDomElementList(SelfValidatingPageObjectSchema schema, List<string> knownExcludeItems)
        {
            if (schema.KnownExclusionElements != null)
            {
                if (knownExcludeItems == null)
                {
                    knownExcludeItems = new List<string>();
                }

                knownExcludeItems.AddRange(schema.KnownExclusionElements.Select(s => s.Id));
            }

            if (knownExcludeItems != null)
            {
                foreach (var item in knownExcludeItems)
                {
                    // handle wildcards differently
                    if (item.Contains("*"))
                    {
                        schema.ActualElements.RemoveAll(s => s.Id.StartsWith(item.Substring(0, item.IndexOf("*", StringComparison.Ordinal))));
                    }
                    else
                    {
                        var matchingIndex = schema.ActualElements.FindIndex(s => s.Id == item);
                        if (matchingIndex >= 0)
                        {
                            schema.ActualElements.RemoveAt(matchingIndex);
                        }
                    }
                }
                //schema.ActualElementIdList.RemoveAll(knownExcludeItems.Contains);
            }

            // get a list of all items on the page that don't have a matching page object item
            List<string> onPageElementsWithoutCustomAttribute = (from item in schema.ActualElements where schema.ExpectedElements.All(s => s.Id != item.Id) select item.Id).ToList();

            //List<string> onPageElementsWithoutCustomAttribute = schema.ActualElements.Where(p => schema.ExpectedElements.All(p2 => p2 != p)).Select(s => s.Id).ToList();

            // now, get a list of all page object items that don't exist on the DOM
            List<string> pageObjectElementsWithoutMatchingPageElement = (from item in schema.ExpectedElements where schema.ActualElements.All(s => s.Id != item.Id) select item.Id).ToList();
            //schema.ExpectedElements.Where(p => schema.ActualElements.All(p2 => p2 != p)).Select(s => s.Id).ToList();

            // the expectation is that both the above lists will be empty as the page objects should match the actual DOM.
            // if there is a difference, log the differences and return a false.

            const string errorMessagePattern = "Mismatch between Page Object '{3}' and actual DOM detected.  Item [{0}] defined in {1} but not found on {2}";

            if (onPageElementsWithoutCustomAttribute.Any() || pageObjectElementsWithoutMatchingPageElement.Any())
            {
                if (onPageElementsWithoutCustomAttribute.Any())
                {
                    LogX.Warning.Write("The following items exist on the Browser Dom but not in the Page Object: [{0}]", string.Join(",", onPageElementsWithoutCustomAttribute));

                    foreach (string thisMessage in onPageElementsWithoutCustomAttribute.Select(item => string.Format(errorMessagePattern, item, "Browser Dom", "Page Object", schema.PageName)))
                    {
                        LogX.Warning.Write(thisMessage);
                        Console.Error.WriteLine(thisMessage);
                    }
                }

                if (pageObjectElementsWithoutMatchingPageElement.Any())
                {
                    LogX.Warning.Write("The following items exist on the Page Object but not in the Browser Dom: [{0}]", string.Join(",", pageObjectElementsWithoutMatchingPageElement));

                    foreach (string thisMessage in pageObjectElementsWithoutMatchingPageElement.Select(item => string.Format(errorMessagePattern, item, "Page Object", "Browser Dom", schema.PageName)))
                    {
                        LogX.Warning.Write(thisMessage);
                        Console.Error.WriteLine(thisMessage);
                    }
                }

                return false;
            }

            return true;
        }
    }
}