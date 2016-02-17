using System;
using OSIsoft.AF;
using OSIsoft.AF.Search;
using OSIsoft.AF.Asset;
using External;

namespace Ex2_Measuring_AF_Client_Performance_Sln
{
    class Program
    {
        static void Main(string[] args)
        {
            PISystem ps = PISystem.CreatePISystem("BSHANGE6430S");

            using (new AFProbe("PrintAttributeCounts", ps))
            {
                AFDatabase db = ps.Databases["Feeder Voltage Monitoring"];

                // Build search object
                AFSearchToken searchToken = new AFSearchToken(
                    filter: AFSearchFilter.Root,
                    searchOperator: AFSearchOperator.Equal,
                    value: db.Elements["Assets"].GetPath());

                AFElementSearch elementSearch = new AFElementSearch(db, "Feeders and Transformers", new[] { searchToken });

                Console.WriteLine("Feeders and Transformers");
                // Use full load: true to fully load the elements
                foreach (AFElement element in elementSearch.FindElements(fullLoad: true))
                {
                    Console.WriteLine("Element: {0}, # Attributes: {1}", element.Name, element.Attributes.Count);
                }
            }

            Console.WriteLine("Press any key to quit");
            Console.ReadKey();
        }
    }
}
