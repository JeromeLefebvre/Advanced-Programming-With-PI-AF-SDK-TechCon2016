using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using External;

namespace Ex3_Real_Time_Rankings
{
    class Program
    {
        static void Main(string[] args)
        {
            PISystem ps = PISystem.CreatePISystem(Constants.AFSERVERNAME);
            AFDatabase db = ps.Databases["FeederVoltageMonitoring"];
            AFAttributeTemplate attrTemp = db.ElementTemplates["Feeder"].AttributeTemplates["Reactive Power"];

            AssetRankProvider rankProvider = new AssetRankProvider(attrTemp);

            rankProvider.Start();

            // Get rankings every 5 seconds. Do this 10 times.
            foreach (int i in Enumerable.Range(0, 10))
            {
                Thread.Sleep(5000);
                IList<AFRankedValue> rankings = rankProvider.GetRankings();
                foreach (var r in rankings)
                {
                    Console.WriteLine($"{r.Ranking} {r.Value.Attribute.Element.Name} {r.Value.Timestamp} {r.Value.Value}");
                }
                Console.WriteLine();
            }

            // Remove the server-side signup.
            rankProvider.Dispose();

            Console.WriteLine("Press any key to quit");
            Console.ReadKey();
        }
    }
}
