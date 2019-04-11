using System;

namespace AeroORMFramework
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CanBeNullAttribute : Attribute
    {
        public bool Value { get; } 

        public CanBeNullAttribute(bool value)
        {
            Value = value;
        }
    }
}
