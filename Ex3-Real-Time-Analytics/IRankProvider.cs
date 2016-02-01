using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSIsoft.AF.Asset;
using OSIsoft.AF.Data;

namespace Ex3_Real_Time_Analytics
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
