using System;
using System.Collections.Generic;

namespace AdminAPI.Models
{
    public partial class CurrencyMaster
    {
        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsBase { get; set; }
    }
}
