using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using OSIsoft.AF;
using OSIsoft.AF.Diagnostics;

namespace External
{
    public class AFProbe : IDisposable
    {
        private readonly string _name;
        private readonly PISystem _piSystem;
        private readonly Stopwatch _sw;

        private AFRpcMetric[] _startServer;
        private AFRpcMetric[] _startClient;

        public AFProbe(string name, PISystem piSystem)
        {
            _name = name;
            _piSystem = piSystem;

            _startServer = piSystem.GetRpcMetrics();
            _startClient = piSystem.GetClientRpcMetrics();

            _sw = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            _sw.Stop();
            Console.WriteLine("Operation {0} took: {1} ms\n", _name, _sw.ElapsedMilliseconds);

            AFRpcMetric[] endClient = _piSystem.GetClientRpcMetrics();
            AFRpcMetric[] endServer = _piSystem.GetRpcMetrics();
            var diffClient = AFRpcMetric.SubtractList(endClient, _startClient);
            var diffServer = AFRpcMetric.SubtractList(endServer, _startServer);

            Console.WriteLine("RPC Metrics");
            foreach (var clientMetric in diffClient)
            {
                foreach (var serverMetric in diffServer)
                {
                    if (clientMetric.Name == serverMetric.Name)
                    {
                        Console.WriteLine("   {0}: {1} calls.  {2} ms/call on client. {3} ms/call on server.  Delta: {4} ms/call",
                            clientMetric.Name,
                            clientMetric.Count,
                            Math.Round(clientMetric.MillisecondsPerCall),
                            Math.Round(serverMetric.MillisecondsPerCall),
                            Math.Round(clientMetric.MillisecondsPerCall - serverMetric.MillisecondsPerCall));
                        break;
                    }
                }
            }
        }
    }
}
