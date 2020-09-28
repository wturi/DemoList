using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AopDemo2
{
    class Program
    {
        static void Main(string[] args)
        {
        }


        static string GetSomeOne()
        {
            try
            {
                var result = $"GetSomeOne result";
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("GetSomeOne error");
                return null;
            }
        }

         string GetOtherOne()
        {
            try
            {
                var result = $"GetOtherOne result";
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("GetOtherOne error");
                return null;
            }
        }



    }
}
