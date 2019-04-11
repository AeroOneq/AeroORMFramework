using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeroORMFramework.CustomTypes
{
    public class Name
    {
        private string name;
        public string Value
        {
            get => name;
            set
            {
                if (value.Length == 0 || value.Length > 255)
                {
                    throw new NotAppropriateParamException("Surname length must be greater than 0" +
                        "and less or equal than 255");
                }
                name = value;
            }
        }

        public static explicit operator Name(string str)
        {
            return new Name
            {
                Value = str
            };
        }
        public static implicit operator string(Name name)
        {
            return name.Value;
        }
    }
}
