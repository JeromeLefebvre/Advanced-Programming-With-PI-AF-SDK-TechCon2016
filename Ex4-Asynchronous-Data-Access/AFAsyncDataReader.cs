using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using OSIsoft.AF.Asset;
using OSIsoft.AF.Data;
using OSIsoft.AF.Time;

namespace Ex4_Asynchronous_Data_Access
{
    public class AFAsyncDataReader
    {
        public static async Task<IList<IDictionary<AFSummaryTypes, AFValue>>> GetSummariesAsync(AFAttributeList attributeList)
        {
            Console.WriteLine("Calling GetSummariesAsync");

            // Your code here

            // Change the return from null
            return null;
        }
    }
}
