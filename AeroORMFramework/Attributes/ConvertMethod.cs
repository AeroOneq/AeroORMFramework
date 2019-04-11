using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeroORMFramework
{
    public delegate object ConvertionDelegate(object value);
    public class ConvertMethod : Attribute
    {
        public ConvertionDelegate convertionDelegate { get; set; }

        public ConvertMethod(ConvertionDelegate convertionDelegate)
        {
            this.convertionDelegate = convertionDelegate;
        }
    }
}
