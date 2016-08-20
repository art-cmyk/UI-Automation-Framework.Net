using System;
using System.Reflection;

namespace Ravitej.Automation.Common.Utilities
{
    public static class EnumUtilities
    {
        public static T GetValueFromDescription<T>(string description) where T : struct
        {
            var type = typeof (T);
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof (CustomDescriptionAttribute)) as CustomDescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T) field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T) field.GetValue(null);
                }
            }
            throw new ArgumentException(
                $"Description '{description}' not found in {(object) type} enum.", nameof(description));
        }

        public static T GetValueFromSecondaryDescription<T>(string secondaryDescription) where T : struct
        {
            var type = typeof(T);
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(CustomDescriptionAttribute)) as CustomDescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.SecondaryDescription == secondaryDescription)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == secondaryDescription)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException(
                $"Secondary Description '{secondaryDescription}' not found in {(object) type} enum.", nameof(secondaryDescription));
        }

        //public static string GetDescriptionForValue<TEnum, TAttribute>(TEnum enumValue) where TEnum : struct where TAttribute : DescriptionAttribute
        //{
        //    var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

        //    var attribute = fieldInfo.GetCustomAttribute(
        //        typeof(TAttribute), false) as TAttribute;
            
        //    return attribute != null ? attribute.Description : enumValue.ToString();
        //}

        public static string GetDescriptionForValue<T>(T enumValue) where T : struct
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            var attribute = fieldInfo.GetCustomAttribute(
                typeof (CustomDescriptionAttribute), false) as CustomDescriptionAttribute;

            return attribute != null ? attribute.Description : enumValue.ToString();
        }

        public static string GetSecondaryDescriptionForValue<T>(T enumValue) where T : struct
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            var attribute = fieldInfo.GetCustomAttribute(
                typeof(CustomDescriptionAttribute), false) as CustomDescriptionAttribute;

            return attribute != null ? attribute.SecondaryDescription : enumValue.ToString();
        }

        public static T ToEnum<T>(this string enumString) where T : struct
        {
            return (T)Enum.Parse(typeof(T), enumString);
        }

        public static T ToEnum<T>(this int enumInt) where T : struct
        {
            return (T)Enum.ToObject(typeof(T), enumInt);
        }
    }
}
