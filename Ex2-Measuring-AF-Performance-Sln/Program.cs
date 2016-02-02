using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.Search;
using External;

namespace Ex2_Measuring_AF_Server_RPCs_Sln
{
    class Program
    {
        static void Main(string[] args)
        {
            PISystem ps = PISystem.CreatePISystem(Constants.AFSERVERNAME);

            string path = @"\Feeder Voltage Monitoring\Assets";

            using (new AFProbe("PrintAttributeCounts", ps))
            {
 
                AFElement assets = AFElement.FindElementsByPath(new[] { path }, ps)[path];

                // Preload all elements so we don't do it one by one during element.Attributes.Count call later
                AFNamedCollectionList<AFElement> elements = AFElement.LoadElementsToDepth(new[] { assets }.ToList(), true, 1, 100);

                // Avoid temptation to use the below
                // AFElements elements = assets.Elements;

                foreach (AFElement element in elements)
                {
                    Console.WriteLine("Element: {0}, # Attributes: {1}", element.Name, element.Attributes.Count);
                }
            }

            Console.WriteLine("Press any key to quit");
            Console.ReadKey();
        }
    }
}
