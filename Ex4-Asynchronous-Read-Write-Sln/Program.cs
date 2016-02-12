using System;
using OSIsoft.AF;

namespace Ex4_Asynchronous_Read_Write_Sln
{
    class Program
    {
        static void Main(string[] args)
        {
            AFDatabase database = AFDataReadWrite.GetDatabase("PISRV01", "Feeder Voltage Monitoring");
            AFDataReadWrite.UpdateAttributeData(database);
            AFDataReadWrite.GetTotalsAsync(database);
            // For comparison
            AFDataReadWrite.GetTotalsBulk(database);

            Console.WriteLine("Press any key to quit");
            Console.ReadKey();
        }
    }
}
