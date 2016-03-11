﻿using System;
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
            AFNamedCollectionList<AFBaseElement> baseElements;

            // Paging pattern
            do
            {
                baseElements = elementTemplate.FindInstantiatedElements(includeDerived: true,
                                                                        sortField: AFSortField.Name,
                                                                        sortOrder: AFSortOrder.Ascending,
                                                                        startIndex: startIndex,
                                                                        maxCount: pageSize,
                                                                        totalCount: out totalCount);

                IEnumerable<AFElement> elements = baseElements.OfType<AFElement>();

                IEnumerable<IGrouping<AFElementTemplate, AFElement>> elementGroupings = elements.GroupBy(elm => elm.Template);
                foreach (IGrouping<AFElementTemplate, AFElement> item in elementGroupings)
                {
                    // The passed in attribute template name may belong to a base element template.
                    // GetLastAttributeTemplateOverride searches upwards the template inheritance chain
                    // until it finds the desired attribute template.
                    List<AFAttributeTemplate> attrTemplates = attributesToLoad
                        .Select(atr => GetLastAttributeTemplateOverride(item.Key, atr))
                        .Where(atr => atr != null)
                        .ToList();

                    List<AFElement> elementsToLoad = item.ToList();
                    AFElement.LoadAttributes(elementsToLoad, attrTemplates);
                    results.AddRange(elementsToLoad);
                }     

                startIndex += baseElements.Count;
            } while (startIndex < totalCount && baseElements.Count != 0);
            return results;
        }

        private static AFAttributeTemplate GetLastAttributeTemplateOverride(AFElementTemplate elmTemplate, string attributeName)
        {
            if (elmTemplate == null)
                throw new ArgumentNullException("elmTemplate");
                  
            AFAttributeTemplate attrTemplate = elmTemplate.AttributeTemplates[attributeName];

            return attrTemplate == null ? GetLastAttributeTemplateOverride(elmTemplate.BaseTemplate, attributeName) : attrTemplate;
        }
    }
}
