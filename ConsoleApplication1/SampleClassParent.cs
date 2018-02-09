using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class SampleClassParent
    {
        public void SetValue<T>(string name,T value)
        {
            GetType().GetProperty(name).SetValue(this,value);
        }
    }
}
