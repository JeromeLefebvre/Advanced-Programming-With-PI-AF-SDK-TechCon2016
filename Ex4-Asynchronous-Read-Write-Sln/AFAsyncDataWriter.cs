using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.Data;
using OSIsoft.AF.Time;

namespace Ex4_Asynchronous_Read_Write_Sln
{
    public class AFAsyncDataWriter
    {
        public static string AFValueToString(AFValue value)
        {
            return string.Format("{0} @ {1}", value.Value.ToString(), value.Timestamp.LocalTime.ToString("MM-dd HH:mm:ss.F"));
        }

        public static int UpdateAttributeData(AFAttributeList attributeList)
        {
            AFTimeRange timeRange = new AFTimeRange(new AFTime(string.Format("{0}-{1}-01", DateTime.Now.Year, DateTime.Now.Month)), new AFTime("T"));
            AFTimeSpan hourInterval = new AFTimeSpan(0, 0, 0, 1);

            // Question: What risk is run by the following code?
            List<Task<AFErrors<AFValue>>> processWrites = new List<Task<AFErrors<AFValue>>>();
            foreach (AFAttribute attribute in attributeList)
            {
                AFValues vals = GenerateValueSequence(attribute, timeRange.StartTime, timeRange.EndTime, hourInterval);
                processWrites.Add(attribute.Data.UpdateValuesAsync(vals, AFUpdateOption.Insert, AFBufferOption.DoNotBuffer));
            }

            int errorcount = 0;
            try
            {
                Task.WaitAll(processWrites.ToArray());
                foreach (var item in processWrites)
                {
                    AFErrors<AFValue> errors = item.Result;
                    // Count PIPoint errors
                    if (errors != null && errors.HasErrors)
                        errorcount += errors.Errors.Count;

                    // Report PI Server, AF Server errors
                }
            }
            catch (AggregateException ae)
            {
            }

            return errorcount;
        }

        #region Setup
        private static AFValues GenerateValueSequence(AFAttribute attribute, AFTime start, AFTime end, AFTimeSpan interval)
        {
            float zero = 100.0F;
            float span = 360.0F;
            AFValues values = new AFValues();
            AFTime timestamp = start;
            Random rnd = new Random((int)DateTime.Now.Ticks % 86400);
            int idx = 1;
            while (timestamp <= end)
            {
                values.Add(new AFValue(attribute, zero + (float)rnd.NextDouble() * span, timestamp));
                timestamp = interval.Multiply(start, idx++);
            }

            return values;
        }

        private static AFValue GenerateValue(AFAttribute attribute, Random rnd, float zero, float span, AFTime timestamp)
        {
            return new AFValue(attribute, zero + (float)rnd.NextDouble() * span, timestamp);
        }
        #endregion
    }
}
