﻿using System;
using System.Collections.Generic;
using System.Linq;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using External;

namespace Ex1_Finding_And_Loading_Assets
{
    class Program
    {
        static void Main(string[] args)
        {
            AFDatabase db = ConnectionHelper.GetDatabase("localhost", "Feeder Voltage Monitoring");

            AFElementTemplate elemTemp = db.ElementTemplates["Feeder"];
            IList<string> attributesToLoad = new[] { "Reactive Power", "Total Current" }.ToList();

            GC.Collect(2, GCCollectionMode.Forced, blocking: true);
            var begin = GC.GetTotalMemory(forceFullCollection: true);

            IList<AFElement> elementsLoaded = AFElementLoader.LoadElements(elemTemp, attributesToLoad);

            var end = GC.GetTotalMemory(forceFullCollection: true);

            // Keep below 260 KB
            Console.WriteLine("elementsLoaded Memory: {0:N0}", (end - begin));

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
    }
}
