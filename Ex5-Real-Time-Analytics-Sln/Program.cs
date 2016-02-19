﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OSIsoft.AF;
using OSIsoft.AF.Asset;

namespace Ex5_Real_Time_Analytics_Sln
{
    class Program
    {
        static void Main(string[] args)
        {
            PISystem ps = PISystem.CreatePISystem("PISRV01");
            AFDatabase db = ps.Databases["Feeder Voltage Monitoring"];
            AFAttributeTemplate attrTemp = db.ElementTemplates["Feeder"].AttributeTemplates["Reactive Power"];

            AssetRankProvider rankProvider = new AssetRankProvider(attrTemp);

            rankProvider.Start();

            // Get top 3 Feeders every 5 seconds. Do this 10 times.
            foreach (int i in Enumerable.Range(0,10))
            {           
                IList<AFRankedValue> rankings = rankProvider.GetTopNElements(3);
                foreach (var r in rankings)
                {
                    Console.WriteLine($"{r.Ranking} {r.Value.Attribute.Element.Name} {r.Value.Timestamp} {r.Value.Value}");
                }
                Console.WriteLine();
                Thread.Sleep(5000);
            }

            // Remove the server-side signup.
            rankProvider.Dispose();

            Console.WriteLine("Press any key to quit");
            Console.ReadKey();
        }
    }
}