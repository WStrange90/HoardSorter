//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HoardSorter.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CardCollection
    {
        public int CardCollectionID { get; set; }
        public int CardID { get; set; }
        public Nullable<bool> ToTrade { get; set; }
        public Nullable<bool> Wanted { get; set; }
        public Nullable<int> OwnedQty { get; set; }
        public Nullable<int> TradeQty { get; set; }
        public Nullable<int> WantQty { get; set; }
        public int collectorID { get; set; }
    
        public virtual CardDetail CardDetail { get; set; }
        public virtual Collection Collection { get; set; }
    }
}
