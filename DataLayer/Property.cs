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
    
    public partial class Property
    {
        public Property()
        {
            this.Certificates = new HashSet<Certificate>();
            this.Earnings = new HashSet<Earning>();
            this.Subsequents = new HashSet<Subsequent>();
        }
    
        public int Id { get; set; }
        public double Block { get; set; }
        public double Lot { get; set; }
        public Nullable<double> Qualifier { get; set; }
        public string HouseNum { get; set; }
        public string Zip { get; set; }
        public string StreetName { get; set; }
        public string OwnerFirstName { get; set; }
        public string OwnerLastName { get; set; }
        public int MunicipalityId { get; set; }
        public int PropertyTypeId { get; set; }
    
        public virtual ICollection<Certificate> Certificates { get; set; }
        public virtual ICollection<Earning> Earnings { get; set; }
        public virtual Municipality Municipality { get; set; }
        public virtual PropertyType PropertyType { get; set; }
        public virtual ICollection<Subsequent> Subsequents { get; set; }
    }
}
