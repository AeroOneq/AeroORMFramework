using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AeroORMFramework;

namespace TestApplication
{
    public class UserSettings
    {
        [CanBeNull(false)]
        public int Id { get; set; }
    }
}
