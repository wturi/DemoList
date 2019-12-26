using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicDemo
{
    public class DemoMethod
    {
        public string Msg { get; set; }
        public void Run(string msg)
        {
            Msg = msg;
        }
    }
}
