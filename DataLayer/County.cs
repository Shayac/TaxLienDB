//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class County
    {
        public County()
        {
            this.Municipalities = new HashSet<Municipality>();
        }
    
        public int Id { get; set; }
        public string CountyName { get; set; }
        public decimal RecordingFee { get; set; }
    
        public virtual ICollection<Municipality> Municipalities { get; set; }
    }
}
