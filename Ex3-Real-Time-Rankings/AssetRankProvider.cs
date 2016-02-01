using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.Data;

namespace Ex3_Real_Time_Analytics
{
    public class AssetRankProvider : IObserver<AFDataPipeEvent>, IRankProvider
    {
        public AFAttributeTemplate AttributeTemplate { get; set; }
        public AFDataPipe DataPipe { get; set; }

        // Define other instance members here

        public AssetRankProvider(AFAttributeTemplate attrTemplate)
        {
            AttributeTemplate = attrTemplate;
            DataPipe = new AFDataPipe();

            // Your code here
        }

        public void Start()
        {
            // Gets all attributes from the AttributeTemplate
            AFAttributeList attrList = GetAttributes();

            // Your code here
            // 1. Subscribe this instance to the data pipe.
            // 2. Signup the attributes in attrList above to the data pipe.
            // 3. Start polling for data pipe events.
        }

        public void OnCompleted()
        {
            Console.WriteLine("{0} | AssetRankProvider has completed", DateTime.Now);
        }

        public void OnError(Exception error)
        {
            throw error;
        }

        public void OnNext(AFDataPipeEvent dpEvent)
        {
            // Your code here
        }

        public IList<AFRankedValue> GetRankings()
        {
            IList<AFRankedValue> rankings = null;

            // Your code here

            return rankings;
        }

        public void Dispose()
        {
            // Your code here
        }

        private AFAttributeList GetAttributes()
        {
            int startIndex = 0;
            int totalCount;
            int pageSize = 1000;

            AFAttributeList attrList = new AFAttributeList();
            do
            {
                AFAttributeList attrListTemp = AFAttribute.FindElementAttributes(
                    database: AttributeTemplate.Database,
                    searchRoot: null,
                    nameFilter: "*",
                    elemCategory: null,
                    elemTemplate: AttributeTemplate.ElementTemplate,
                    elemType: AFElementType.Any,
                    attrNameFilter: AttributeTemplate.Name,
                    attrCategory: null,
                    attrType: TypeCode.Empty,
                    searchFullHierarchy: true,
                    sortField: AFSortField.Name,
                    sortOrder: AFSortOrder.Ascending,
                    startIndex: startIndex,
                    maxCount: pageSize,
                    totalCount: out totalCount);

                attrList.AddRange(attrListTemp);

                startIndex += pageSize;
            } while (startIndex < totalCount);

            return attrList;
        }
    }
}
