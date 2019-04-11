using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeroORMFramework.CustomTypes
{
    public class Surname
    {
        private string surname;
        public string Value
        {
            get => surname;
            set
            {
                if (value.Length == 0 || value.Length > 255)
                {
                    throw new NotAppropriateParamException("Surname length must be greater than 0" +
                        "and less or equal than 255");
                }
                surname = value;
            }
        }
    }
}
