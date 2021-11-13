using System;

namespace TimeConvertions
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = "2021-11-13T01:49:21.000Z";

           var testdata = Convert.ToDateTime(data).ToString("hh tt");
        }
    }
}
