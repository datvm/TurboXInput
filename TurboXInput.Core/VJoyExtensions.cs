using System;
using System.Collections.Generic;
using System.Text;

namespace vJoyInterfaceWrap
{

    public static class VJoyExtensions
    {
        const int MaxVJoyId = 16;
        
        public static IEnumerable<uint> GetFreeIds(this vJoy vjoy)
        {
            var result = new List<uint>();
            for (uint i = 1; i <= MaxVJoyId; i++)
            {
                if (vjoy.GetVJDStatus(i) == VjdStat.VJD_STAT_FREE)
                {
                    result.Add(i);
                }
            }

            return result;
        }

    }

}
