using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_EbayClone.Domain.Regions
{
    public class Region
    {
        public int RegionID { get; set; }
        public int ParentID { get; set; }
        public string RegionName { get; set; } = string.Empty;
        public string RegionCode { get; set; } = string.Empty;
        public string RegionNameNotMark { get; set; } = string.Empty;
        public string RegionNameAlias { get; set; } = string.Empty;

        public int RegionLevel { get; set; }
        public bool IsActive { get; set; }
        public int Order { get; set; }
        public string RegionCodeVHD { get; set; } = string.Empty;
        public bool IsDelete { get; set; }


    }
}
