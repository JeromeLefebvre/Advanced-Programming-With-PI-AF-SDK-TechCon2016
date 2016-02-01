using System;
using System.Collections.Generic;
using System.Linq;
using OSIsoft.AF.Asset;

namespace Ex5_New_Features_In_PI_AF_SDK_Sln
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random(10);

            List<AFValue> valuesToSort = Enumerable.Range(0, 10).Select(i => new AFValue(rnd.Next(100))).ToList();

            valuesToSort.Sort(new AFValueComparer());

            foreach (AFValue val in valuesToSort)
            {
                if (val.ValueTypeCode == TypeCode.Int32) // should not be false in this case
                {
                    Console.WriteLine(val.ValueAsInt32());
                }
                
            }

            Console.WriteLine("Press any key to quit");
            Console.ReadKey();
        }
    }
}
