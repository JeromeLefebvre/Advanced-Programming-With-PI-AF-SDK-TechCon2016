using System;
using System.Collections.Generic;
using OSIsoft.AF.Asset;

namespace Ex5_New_Features_In_PI_AF_SDK_Sln
{
    public class AFValueComparer : IComparer<AFValue>
    {
        public int Compare(AFValue val1, AFValue val2)
        {
            if (val1.ValueTypeCode != val2.ValueTypeCode)
            {
                throw new InvalidOperationException("Value types do not match");
            }

            if (val1.ValueTypeCode == TypeCode.Double)
            {
                return val1.ValueAsDouble().CompareTo(val2.ValueAsDouble());
            }
            else if (val1.ValueTypeCode == TypeCode.Single)
            {
                return val1.ValueAsSingle().CompareTo(val2.ValueAsSingle());
            }
            else if (val1.ValueTypeCode == TypeCode.Int32)
            {
                return val1.ValueAsInt32().CompareTo(val2.ValueAsInt32());
            }
            else
            {
                throw new InvalidOperationException(string.Format("Cannot compare type {0}", val2.ValueType.Name));
            }
        }
    }
}
