using System;
using System.Collections.Generic;
using OSIsoft.AF.Asset;
using OSIsoft.AF.Data;

namespace Ex3_Real_Time_Analytics_Sln
{
    interface IRankProvider : IDisposable
    {
        // AFAttributeTemplate specifying which attributes to rank
        AFAttributeTemplate AttributeTemplate { get; set; }

        // The data pipe for getting values
        AFDataPipe DataPipe { get; set; }

        // Initializes sign-ups
        void Start();

        // Returns the rankings
        IList<AFRankedValue> GetRankings();
    }
}
