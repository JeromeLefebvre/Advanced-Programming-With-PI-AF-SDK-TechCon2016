using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.Data;
using OSIsoft.AF.PI;
using OSIsoft.AF.Time;

namespace Ex4_Asynchronous_Read_Write
{
    public class AFDataReadWrite
    {
        public static IDictionary<AFAttribute, AFValues> GetTotalsAsync(AFDatabase afDb)
        {
            Dictionary<AFAttribute, AFValues> totals = new Dictionary<AFAttribute, AFValues>();
            return totals;
        }

        public static IDictionary<AFAttribute, AFValues> GetTotalsBulk(AFDatabase afDb)
        {
            Dictionary<AFAttribute, AFValues> totals = new Dictionary<AFAttribute, AFValues>();
            return totals;
        }

        private static void WriteSummaryItem(AFValues summaryitems)
        {
            Console.WriteLine(" First total for {0}: {1}", summaryitems.Attribute.Name, AFValueToString(summaryitems[0]));
        }

        public static string AFValueToString(AFValue value)
        {
            return string.Format("{0} @ {1}", value.Value.ToString(), value.Timestamp.LocalTime.ToString("MM-dd HH:mm:ss.F"));
        }

        public static int UpdateAttributeData(AFDatabase database)
        {
            int errorcount = 0;
            return errorcount;
        }

        #region Setup
        public static AFDatabase GetDatabase(string server, string database)
        {
            PISystem piSystem = new PISystems()[server];
            if (piSystem != null)
                return piSystem.Databases[database];
            else
                return piSystem.Databases.DefaultDatabase;
        }

        private static AFAttributeList GetAttributes(AFDatabase database)
        {
            int startIndex = 0;
            int pageSize = 1000;
            int totalCount;

            AFAttributeList attrList = new AFAttributeList();

            do
            {
                AFAttributeList results = AFAttribute.FindElementAttributes(
                     database: database,
                     searchRoot: null,
                     nameFilter: null,
                     elemCategory: null,
                     elemTemplate: database.ElementTemplates["Feeder"],
                     elemType: AFElementType.Any,
                     attrNameFilter: "Random Data",
                     attrCategory: null,
                     attrType: TypeCode.Empty,
                     searchFullHierarchy: true,
                     sortField: AFSortField.Name,
                     sortOrder: AFSortOrder.Ascending,
                     startIndex: startIndex,
                     maxCount: pageSize,
                     totalCount: out totalCount);

                attrList.AddRange(results);

                startIndex += pageSize;
            } while (startIndex < totalCount);

            return attrList;
        }
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
