using System.Collections.Generic;
using Ravitej.Automation.Common.PageObjects.CustomAttributes;
using Ravitej.Automation.Common.PageObjects.Interactables.Selenium;
using Ravitej.Automation.Common.PageObjects.SelfValidation;
using Ravitej.Automation.Common.Tests.FakeImplementations;
using NUnit.Framework;
using OpenQA.Selenium;

namespace Ravitej.Automation.Common.Tests
{
    [TestFixture]
    public class SelfValidatingPageObjectSchemaGeneratorTest
    {
        [Test]
        public void ClassThatIsSelfValidating()
        {
            // Arrange
            var actual = new ClassThatIsSelfValidating();

            // Act
            var pageObjectSchema = new SelfValidatingPageObjectSchemaGenerator().ExtractSchema(actual);

            // Assert
            Assert.IsTrue(pageObjectSchema.SchemaValid);
            Assert.AreEqual(pageObjectSchema.PageName, "ClassThatIsSelfValidating");

            Assert.IsFalse(pageObjectSchema.HasChildObjects);
            Assert.AreEqual(pageObjectSchema.ChildPageObjectNames.Count, 0);

            Assert.AreEqual(pageObjectSchema.ExpectedElements.Count, 4); // 2 properties and 2 in the definition set
            Assert.AreEqual(pageObjectSchema.ExpectedElements[0].Id, "678");
            Assert.AreEqual(pageObjectSchema.ExpectedElements[1].Id, "901");
            Assert.AreEqual(pageObjectSchema.ExpectedElements[2].Id, "Property1");
            Assert.AreEqual(pageObjectSchema.ExpectedElements[3].Id, "Property2");

            Assert.AreEqual(pageObjectSchema.KnownExclusionElements.Count, 2); // 2 from the definition set
            Assert.AreEqual(pageObjectSchema.KnownExclusionElements[0].Id, "123");
            Assert.AreEqual(pageObjectSchema.KnownExclusionElements[1].Id, "456");

            Assert.AreEqual(pageObjectSchema.ActualElements.Count, 0); // will be zero as no selenium is used in the unit tests
        }

        [Test]
        public void ClassThatIsSelfValidatingWithChildProperties()
        {
            // Arrange
            var actual = new ClassThatIsSelfValidatingWithChildSelfValidatingObject();

            // Act
            var pageObjectSchema = new SelfValidatingPageObjectSchemaGenerator().ExtractSchema(actual);

            // Assert
            Assert.IsTrue(pageObjectSchema.SchemaValid);
            Assert.AreEqual(pageObjectSchema.PageName, "ClassThatIsSelfValidatingWithChildSelfValidatingObject");

            Assert.AreEqual(pageObjectSchema.ChildPageObjectNames.Count, 2);
            Assert.AreEqual(pageObjectSchema.ChildPageObjectNames[0], "ClassThatIsSelfValidating");
            Assert.AreEqual(pageObjectSchema.ChildPageObjectNames[1], "ClassThatIsSelfValidatingButNotAsAChildElement");

            Assert.IsTrue(pageObjectSchema.HasChildObjects);
            Assert.AreEqual(pageObjectSchema.ExpectedElements.Count, 10); // 2 properties and 2 in the definition set
            Assert.AreEqual(pageObjectSchema.ExpectedElements[0].Id, "hij");
            Assert.AreEqual(pageObjectSchema.ExpectedElements[1].Id, "klm");
            Assert.AreEqual(pageObjectSchema.ExpectedElements[2].Id, "PropertyA");
            Assert.AreEqual(pageObjectSchema.ExpectedElements[2].IsNonInteractableElement, false);

            Assert.AreEqual(pageObjectSchema.ExpectedElements[3].Id, "PropertyB");
            Assert.AreEqual(pageObjectSchema.ExpectedElements[3].IsNonInteractableElement, true);

            Assert.AreEqual(pageObjectSchema.ExpectedElements[4].Id, "PropertyC");
            Assert.AreEqual(pageObjectSchema.ExpectedElements[4].IsNonInteractableElement, true);

            Assert.AreEqual(pageObjectSchema.ExpectedElements[5].Id, "PropertyD");
            Assert.AreEqual(pageObjectSchema.ExpectedElements[5].IsNonInteractableElement, false);

            Assert.AreEqual(pageObjectSchema.ExpectedElements[6].Id, "678");
            Assert.AreEqual(pageObjectSchema.ExpectedElements[7].Id, "901");
            Assert.AreEqual(pageObjectSchema.ExpectedElements[8].Id, "Property1");
            Assert.AreEqual(pageObjectSchema.ExpectedElements[9].Id, "Property2");

            Assert.AreEqual(pageObjectSchema.KnownExclusionElements.Count, 10); // 2 from the definition set
            Assert.AreEqual(pageObjectSchema.KnownExclusionElements[0].Id, "abcd");
            Assert.AreEqual(pageObjectSchema.KnownExclusionElements[1].Id, "efg");

            Assert.AreEqual(pageObjectSchema.KnownExclusionElements[2].Id, "123");
            Assert.AreEqual(pageObjectSchema.KnownExclusionElements[3].Id, "456");

            Assert.AreEqual(pageObjectSchema.KnownExclusionElements[4].Id, "987");

            Assert.AreEqual(pageObjectSchema.ActualElements.Count, 0); // will be zero as no selenium is used in the unit tests
        }
    }

    [UserInterfaceElementDefinitionSet(ExcludeElementIdList = "abcd,efg", IncludeElementIdList = "hij,klm")]
    public class ClassThatIsSelfValidatingWithChildSelfValidatingObject : Interactable, ISelfValidatingPageObject
    {
        public ClassThatIsSelfValidatingWithChildSelfValidatingObject()
            : base(new FakeSession())
        {
            Name = "ClassThatIsSelfValidatingWithChildSelfValidatingObject";
        }
        protected override IWebElement ContentContainer { get; set; }

        public override bool IsDisplayed(bool throwWhenNotDisplayed = false)
        {
            throw new System.NotImplementedException();
        }

        [UserInterfaceElement(ElementId = "PropertyA")]
        public object PropertyA()
        {
            return null;
        }

        [UserInterfaceElement(ElementId = "PropertyB", IsNonInteractableElement = true)]
        protected object PropertyB()
        {
            return null;
        }

        [UserInterfaceElement(ElementId = "PropertyC", IsNonInteractableElement = true)]
        protected object PropertyC => null;

        [UserInterfaceElement(ElementId = "PropertyD", IsNonInteractableElement = false)]
        protected object PropertyD => null;

        private object PropertyE => null;

        public ClassThatIsSelfValidating ChildPageObject => new ClassThatIsSelfValidating();

        public ClassThatIsSelfValidatingButNotAsAChildElement IgnoredChildPageObject => new ClassThatIsSelfValidatingButNotAsAChildElement();

        public bool ValidateDisplayedPage(List<string> elementIdsToExcludeFromDomComparison)
        {
            throw new System.NotImplementedException();
        }
    }

    [UserInterfaceElementDefinitionSet(ExcludeElementIdList = "123,456", IncludeElementIdList = "678,901")]
    public class ClassThatIsSelfValidating : Interactable, ISelfValidatingPageObject
    {
        public ClassThatIsSelfValidating()
            : base(new FakeSession())
        {
            Name = "ClassThatIsSelfValidating";
        }
        protected override IWebElement ContentContainer { get; set; }

        public override bool IsDisplayed(bool throwWhenNotDisplayed = false)
        {
            throw new System.NotImplementedException();
        }

        [UserInterfaceElement(ElementId = "Property1")]
        public object Property1()
        {
            return null;
        }

        [UserInterfaceElement(ElementId = "Property2")]
        public object Property2()
        {
            return null;
        }

        public bool ValidateDisplayedPage(List<string> elementIdsToExcludeFromDomComparison)
        {
            throw new System.NotImplementedException();
        }
    }

    [UserInterfaceElementDefinitionSet(ExcludeElementIdList = "987,654", IncludeElementIdList = "741,852")]
    public sealed class ClassThatIsSelfValidatingButNotAsAChildElement : Interactable, ISelfValidatingPageObject, IExcludeElementsWhenAChildObject
    {
        public ClassThatIsSelfValidatingButNotAsAChildElement()
            : base(new FakeSession())
        {
            Name = "ClassThatIsSelfValidatingButNotAsAChildElement";
        }

        protected override IWebElement ContentContainer { get; set; }

        [UserInterfaceElement(ElementId = "PropertyZ")]
        public object PropertyZ()
        {
            return null;
        }

        [UserInterfaceElement(ElementId = "PropertyW")]
        public object PropertyW()
        {
            return null;
        }

        public override bool IsDisplayed(bool throwWhenNotDisplayed = false)
        {
            throw new System.NotImplementedException();
        }

        public bool ValidateDisplayedPage(List<string> elementIdsToExcludeFromDomComparison)
        {
            throw new System.NotImplementedException();
        }
    }
}
