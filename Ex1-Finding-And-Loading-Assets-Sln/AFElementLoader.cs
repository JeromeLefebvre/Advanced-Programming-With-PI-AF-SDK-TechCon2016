using System;
using System.Collections.Generic;
using System.Linq;
using OSIsoft.AF;
using OSIsoft.AF.Asset;

namespace Ex1_Finding_And_Loading_Assets_Sln
{
    public static class AFElementLoader
    {
        public static IList<AFElement> LoadElements(AFElementTemplate elementTemplate, IEnumerable<string> attributesToLoad)
        {
            int totalCount;
            int startIndex = 0;
            int pageSize = 1000;

            List<AFElement> results = new List<AFElement>();

            // Paging pattern
            do
            {
                var baseElements = elementTemplate.FindInstantiatedElements(
                                includeDerived: true,
                                sortField: AFSortField.Name,
                                sortOrder: AFSortOrder.Ascending,
                                startIndex: startIndex,
                                maxCount: pageSize,
                                totalCount: out totalCount);

                // If there are no elements, break the process
                if (baseElements.Count() == 0)
                    break;

                List<AFElement> elements = baseElements.Select(elm => (AFElement)elm).ToList();

                var elementGroupings = elements.GroupBy(elm => elm.Template);
                foreach (var item in elementGroupings)
                {
                    List<AFAttributeTemplate> attrTemplates = attributesToLoad.Select(atr => GetLastAttributeTemplateOverride(item.Key, atr)).ToList();
                    AFElement.LoadAttributes(item.ToList(), attrTemplates);
                }

                results.AddRange(elements);

                startIndex += baseElements.Count();
            } while (startIndex < totalCount);

            return results;
        }

        private static AFAttributeTemplate GetLastAttributeTemplateOverride(AFElementTemplate elmTemplate, string attributeName)
        {
            if (elmTemplate == null)
                throw new ArgumentNullException("elmTemplate");

            AFElementTemplate currElementTemplate = elmTemplate;
            do
            {
                var attrTemplate = currElementTemplate.AttributeTemplates[attributeName];

                if (attrTemplate != null)
                    return attrTemplate;
                else
                    currElementTemplate = currElementTemplate.BaseTemplate;
            } while (currElementTemplate != null);

            return null;
        }
    }
}
