using System;
using NUnit.Framework;
using Ravitej.Automation.Common.Utilities;

namespace Ravitej.Automation.Common.Tests
{
    [TestFixture]
    public class EnumUtilitiesTests
    {
        [Test]
        [TestCase(TestEnum.Value1, "Value 1 Description")]
        [TestCase(TestEnum.Value2, "Value 2 Description")]
        [TestCase(TestEnum.OddNumbering, "Odd Numbering 9")]
        [TestCase(TestEnum.NoDescription, "NoDescription")]
        public void Description_FromValue_ShouldBeCorrect(TestEnum myEnum, string expectedDesc)
        {
            var desc = EnumUtilities.GetDescriptionForValue(myEnum);

            Assert.That(desc, Is.EqualTo(expectedDesc));
        }

        [Test]
        [TestCase(TestEnum.Value1, "Value 1 Secondary Description")]
        [TestCase(TestEnum.Value2, null)]
        [TestCase(TestEnum.OddNumbering, null)]
        [TestCase(TestEnum.NoDescription, "NoDescription")]
        public void SecondaryDescription_FromValue_ShouldBeCorrect(TestEnum myEnum, string expectedDesc)
        {
            var desc = EnumUtilities.GetSecondaryDescriptionForValue(myEnum);

            Assert.That(desc, Is.EqualTo(expectedDesc));
        }

        [Test]
        [TestCase("NoDescription", TestEnum.NoDescription)]
        [TestCase("Value1", TestEnum.Value1)]
        [TestCase("Value2", TestEnum.Value2)]
        [TestCase("OddNumbering", TestEnum.OddNumbering)]
        public void StringExtension_ToGetEnum_ShouldReturn_CorrectEnumValue(string stringEnum, TestEnum expectedEnumValue)
        {
            var actualEnumValue = stringEnum.ToEnum<TestEnum>();

            Assert.That(actualEnumValue, Is.EqualTo(expectedEnumValue));
        }

        [Test]
        [TestCase(0, TestEnum.Value1)]
        [TestCase(1, TestEnum.Value2)]
        [TestCase(2, TestEnum.NoDescription)]
        [TestCase(9, TestEnum.OddNumbering)]
        public void IntegerExtension_ToGetEnum_ShouldReturn_CorrectEnumValue(int intEnum, TestEnum expectedEnumValue)
        {
            var actualEnumValue = intEnum.ToEnum<TestEnum>();

            Assert.That(actualEnumValue, Is.EqualTo(expectedEnumValue));
        }

        [Test]
        [TestCase("Value 1 Description", TestEnum.Value1)]
        [TestCase("Value 2 Description", TestEnum.Value2)]
        [TestCase("Odd Numbering 9", TestEnum.OddNumbering)]
        public void EnumValueFrom_Description_ShouldBeCorrect(string desc, TestEnum expectedEnumValue)
        {
            var myEnum = EnumUtilities.GetValueFromDescription<TestEnum>(desc);
            Assert.That(myEnum, Is.EqualTo(expectedEnumValue));
        }

        [Test]
        [TestCase("Value 1 Secondary Description", TestEnum.Value1)]
        public void EnumValueFrom_SecondaryDescription_ShouldBeCorrect(string desc, TestEnum expectedEnumValue)
        {
            var myEnum = EnumUtilities.GetValueFromSecondaryDescription<TestEnum>(desc);
            Assert.That(myEnum, Is.EqualTo(expectedEnumValue));
        }

        [Test]
        public void EnumValueFromDescription_WhenNoDescriptionAvailable_ShouldThrowArgumentException()
        {
            const string myEnumDesc = "Some gibberish";
            Assert.That(() => EnumUtilities.GetValueFromDescription<TestEnum>(myEnumDesc),
                Throws.Exception.TypeOf<ArgumentException>());
        }

        [Test]
        public void EnumValueFromDescription_WhenNoDescriptionAvailable_ShouldThrowArgumentException_WithCorrectMessage()
        {
            const string myEnumDesc = "Some gibberish";
            const string expectedExceptionMessage = "Description 'Some gibberish' not found in Ravitej.Automation.Common.Tests.TestEnum enum.\r\nParameter name: description";
            var ex = Assert.Throws<ArgumentException>(() => EnumUtilities.GetValueFromDescription<TestEnum>(myEnumDesc));
            Assert.That(ex.Message, Is.EqualTo(expectedExceptionMessage));
        }

        [Test]
        public void EnumValueFrom_SecondaryDescription_WhenNoSecondaryDescriptionAvailable_ShouldThrowArgumentException()
        {
            const string myEnumDesc = "Some gibberish";
            Assert.That(() => EnumUtilities.GetValueFromSecondaryDescription<TestEnum>(myEnumDesc),
                Throws.Exception.TypeOf<ArgumentException>());
        }

        [Test]
        public void EnumValueFrom_SecondaryDescription_WhenNoSecondaryDescriptionAvailable_ShouldThrowArgumentException_WithCorrectMessage()
        {
            const string myEnumDesc = "Some gibberish";
            const string expectedExceptionMessage = "Secondary Description 'Some gibberish' not found in Ravitej.Automation.Common.Tests.TestEnum enum.\r\nParameter name: secondaryDescription";
            var ex = Assert.Throws<ArgumentException>(() => EnumUtilities.GetValueFromSecondaryDescription<TestEnum>(myEnumDesc));
            Assert.That(ex.Message, Is.EqualTo(expectedExceptionMessage));
        }
    }

    public enum TestEnum
    {
        [CustomDescription("Value 1 Description", "Value 1 Secondary Description")]
        Value1,
        [CustomDescription("Value 2 Description")]
        Value2,
        NoDescription,
        [CustomDescription("Odd Numbering 9")]
        OddNumbering = 9
    }
}
