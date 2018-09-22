using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormDrawTest
{
    class Histogram
    {
        public Histogram(double[] values)
        {
            Debug($"loading histogram with {values.Length} values");
        }

        private void Debug(string msg)
        {
            Console.WriteLine(msg);
        }
        
    }
}
