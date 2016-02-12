using System;
using System.Linq;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.Search;
using External;

namespace Ex2_Measuring_AF_Client_Performance
{
    class Program
    {
        static void Main(string[] args)
        {
            PISystem ps = PISystem.CreatePISystem("PISRV01");
            AFDatabase db = ps.Databases["Feeder Voltage Monitoring"];

            AFElementTemplate elemTemplate = db.ElementTemplates["Substation Transformer"];

            // This is an example
            using (new AFProbe("FindElementsByAttribute", ps))
            {
                var avq = new[]
                {
                    new AFAttributeValueQuery(elemTemplate.AttributeTemplates["Model"], AFSearchOperator.Equal, "506A"),
                };

                var elements = AFElement.FindElementsByAttribute(null, "*", avq, true, AFSortField.Name, AFSortOrder.Ascending, 100);
            }

            using (new AFProbe("FindElementsByAttribute", ps))
            {
                // Your code here
            }

            Console.WriteLine("Press any key to quit");
            Console.ReadKey();
        }
    }
}
