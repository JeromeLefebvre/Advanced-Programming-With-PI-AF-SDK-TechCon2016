using OSIsoft.AF;
using External;

namespace Ex4_Asynchronous_Read_Write_Sln
{
    class Program
    {
        static void Main(string[] args)
        {
            AFDatabase database = AFDataReadWrite.GetDatabase(Constants.AFSERVERNAME, "Feeder Voltage Monitoring");
            AFDataReadWrite.UpdateAttributeData(database);
            AFDataReadWrite.GetTotalsAsync(database);
            // For comparison
            AFDataReadWrite.GetTotalsBulk(database);
        }
    }
}
